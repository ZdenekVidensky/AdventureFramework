﻿namespace TVB.Game.Graph
{
	using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
	using UnityEngine;
	using XNode;

    using TVB.Core.Coroutines;
    using TVB.Game.GUI;
    using TVB.Game.Interactable;
    using TVB.Game.GameSignals;

    public class GraphManager : MonoBehaviour
	{
		// PRIVATE MEMBERS

		//[SerializeField]
		private GUIIngameView      m_IngameView;
		private int                m_SelectedDecision = -1;
		private IInteractable      m_InteractableObject;
		private bool               m_SkipPerformed;
		private bool               m_BackPerformed;
		private bool               m_Talking;

        // MONOBEHAVIOUR INTERFACE

        private void Awake()
        {
			Initialize();
        }

        private void OnDestroy()
        {
			m_IngameView.SelectDecisionEvent.RemoveListener(OnSelectDecision);
			m_IngameView = null;
		}

        // PUBLIC METHODS

        public void Initialize()
        {
			m_IngameView = FindObjectOfType<GUIIngameView>();
			m_IngameView.SelectDecisionEvent.AddListener(OnSelectDecision);
		}

		public IEnumerator ProcessInteractiveGraph(IInteractable interactableObject, InteractiveGraph interactiveGraph)
        {
			AdventureGame.Instance.IsBusy = true;

			m_InteractableObject          = interactableObject;
			BaseInteractiveNode firstNode = interactiveGraph.GetFirstNode() as BaseInteractiveNode;

			if (firstNode == null)
			{
				Debug.LogError($"There is no starting node in graph {interactableObject.InteractiveGraph.name}");
				yield break;
			}


			yield return ProcessNode(firstNode);
		}

		public void OnSkipPerformed()
        {
			if (m_Talking == false)
				return;

			m_SkipPerformed = true;
		}

		public void OnBackPerformed()
		{
			m_BackPerformed = true;
		}

		// PRIVATE METHODS

		private IEnumerator ProcessNode(BaseInteractiveNode node)
		{
			if (node == null)
				yield break;

			switch (node)
			{
				case DestroyObjectNode _:
					m_InteractableObject.Destroy();
					break;
				case UnlockAchievementNode achievementNode:
					AdventureGame.Instance.UnlockAchievement(achievementNode.ID);
					break;
				case SpecialFunctionNode specialNode:
					(m_InteractableObject as MonoBehaviour).Invoke(specialNode.MethodName, specialNode.Delay);
					break;
				case WaitForSecondsNode waitNode:
					yield return new WaitForSeconds(waitNode.Seconds);
					break;
				case EndGameNode _:
					Signals.GameplaySignals.EndGame.Emit();
					yield break;
				case DialogueLineNode dialogueNode:
					yield return Talk(dialogueNode);
					break;
				case TakeItemNode takeItemNode:
					TakeItem(takeItemNode);
					break;
				case WaitForBackNode _:
					m_BackPerformed = false;
					yield return new WaitWhile(() => m_BackPerformed == false);
					m_BackPerformed = false;
					break;
				case DropItemNode dropItemNode:
					DropItem(dropItemNode);
					break;
				case ConditionNode conditionNode:
					yield return Condition(conditionNode);
					yield break;
				case SetConditionNode setConditionNode:
					SetCondition(setConditionNode);
					break;
				case DecisionNode decisionNode:
					m_SelectedDecision = -1;

					List<int> textIDs = GetDecisionsTextIDs(decisionNode.Decisions);

					if (textIDs.Count == 0)
						yield break;

					m_IngameView.SetDecisions(textIDs);
					m_IngameView.DisplayDecisions(textIDs.Count > 0);

					while (m_SelectedDecision < 0)
						yield return null;

					Decision selectedDecision = decisionNode.Decisions.FirstOrDefault(m => m.TextID == textIDs[m_SelectedDecision]);
					int realIndex = System.Array.IndexOf(decisionNode.Decisions, selectedDecision);

					NodePort decOutput = decisionNode.GetOutputPort($"{nameof(DecisionNode.Decisions)} {realIndex}");
					m_SelectedDecision = -1;

					m_IngameView.DisplayDecisions(false);

					if (decOutput != null && decOutput.Connection != null)
					{
						yield return ProcessNode(decOutput.Connection.node as BaseInteractiveNode);
						yield break;
					}
					break;
			}

			NodePort output = node.GetPort("Output")?.Connection;
			
			if (output == null) // End of graph
            {
				m_IngameView.EndedInteraction();
				AdventureGame.Instance.IsBusy = false;
				yield break;
			}

			yield return ProcessNode(output.node as BaseInteractiveNode);
		}

		private List<int> GetDecisionsTextIDs(Decision[] decisions)
		{
			int decisionsCount = decisions.Length;
			List<int> textIDs = new List<int>(decisions.Length);

			for (int idx = 0; idx < decisionsCount; idx++)
			{
				Decision decision = decisions[idx];
				bool add = true;

				if (string.IsNullOrEmpty(decision.Condition) == false)
				{
					add = GetConditionValueByName(decision.Condition);
				}

				if (add == true)
				{
					textIDs.Add(decision.TextID);
				}
			}

			return textIDs;
		}

		private bool GetConditionValueByName(string conditionName)
		{
			bool isNegative = false;

			if (conditionName.StartsWith("!") == true)
			{
				conditionName = conditionName.Substring(1);
				isNegative    = true;
			}

			if (conditionName.StartsWith("HasItem(") == true)
			{
				string substring = conditionName.Substring(conditionName.IndexOf('(') + 1, 1);
				string itemID = substring;
				return isNegative == true ? AdventureGame.Instance.Inventory.HasItem(itemID) == false : AdventureGame.Instance.Inventory.HasItem(itemID) == true;
			}
			else
			{
				return isNegative == true ? AdventureGame.Instance.GetCondition(conditionName) == false : AdventureGame.Instance.GetCondition(conditionName) == true;
			}
		}

		private void SetCondition(SetConditionNode node)
        {
			AdventureGame.Instance.SetCondition(node.ConditionName, node.BooleanValue);
        }

		private IEnumerator Condition(ConditionNode conditionNode)
        {
			bool conditionSatisfied = false;
			bool condition          = AdventureGame.Instance.GetCondition(conditionNode.ConditionName);

            switch (conditionNode.ConditionType)
            {
				case EConditionType.HasItem:
					conditionSatisfied = AdventureGame.Instance.Inventory.HasItem(conditionNode.Item.ID) == true;
					break;
				case EConditionType.Boolean:
					conditionSatisfied = condition;
					break;
			}

			if (conditionSatisfied == true)
			{
				NodePort trueOutput = conditionNode.GetOutputPort(nameof(ConditionNode.True));
				yield return ProcessNode(trueOutput.Connection.node as BaseInteractiveNode);
			}
			else
			{
				NodePort falseOutput = conditionNode.GetOutputPort(nameof(ConditionNode.False));
				yield return ProcessNode(falseOutput.Connection.node as BaseInteractiveNode);
			}
		}

		private void DropItem(DropItemNode dropItemNode)
        {
			AdventureGame.Instance.Inventory.RemoveItem(dropItemNode.Item);
			UpdateInventoryGUI();
		}

		private void TakeItem(TakeItemNode takeItemNode)
        {
			if (takeItemNode.Item == null)
            {
				Debug.LogError("There is no item assigned to this node!");
				return;
            }

			AdventureGame.Instance.Inventory.AddItem(takeItemNode.Item);
			UpdateInventoryGUI();
			
			if (takeItemNode.DestroyAfterTake == true)
			{
				AdventureGame.Instance.SetCondition($"{m_InteractableObject.Name}-takencondition", true);
				m_InteractableObject.Destroy();
			}
        }

		private void UpdateInventoryGUI()
        {
			Inventory inventory = AdventureGame.Instance.Inventory;
			m_IngameView.SetInventoryData(inventory.Items);
		}

		private IEnumerator Talk(DialogueLineNode dialogueNode)
		{
			yield return null; // Wait one frame for processing skip
			m_Talking = true;
			string text            = dialogueNode.Text;
			ECharacter character   = dialogueNode.Character;
			bool playTalkAnimation = dialogueNode.PlayTalkAnimation;

			if (text == null)
			{
				Debug.LogError("Text cannot be null!", this);
				yield break;
			}

			m_IngameView.SetSubtitlesText(text);
			m_IngameView.SetSubtitlesVisibility(true);

			if (playTalkAnimation == true)
			{
				if (character == ECharacter.This)
				{
					if (m_InteractableObject is ITalkable talkable)
					{
						talkable.SetIsTalking(true);
					}
				}
			}

			float secondsDelay = Mathf.Clamp(text.Length / 10f, 1f, 2.5f);
			m_SkipPerformed = false;
			yield return new WaitForSecondsAndPredicate(secondsDelay, () => m_SkipPerformed == false);
			m_SkipPerformed = false;

			if (playTalkAnimation == true)
			{
				if (character == ECharacter.This)
				{
					if (m_InteractableObject is ITalkable talkable)
					{
						talkable.SetIsTalking(false);
					}
				}
			}

			m_IngameView.SetSubtitlesVisibility(false);
			m_Talking = false;
		}

		// HANDLERS

		private void OnSelectDecision(int index)
        {
			m_SelectedDecision = index;
        }
	}
}