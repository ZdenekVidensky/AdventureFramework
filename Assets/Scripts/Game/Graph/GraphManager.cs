namespace TVB.Core.Graph
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using UnityEngine;
	using XNode;

	using TVB.Core.Coroutines;
	using TVB.Core.Interactable;
    using TVB.Game;
	using TVB.Game.GUI;
	using TVB.Game.GameSignals;
	using TVB.Game.Navigation;
    using TVB.Game.Options;
    using TVB.Game.Utilities;

    public class GraphManager : MonoBehaviour
	{
		// PRIVATE MEMBERS

		private GUIIngameView                             m_IngameView;
		private NavigationManager                         m_NavigationManager;
		private int                                       m_SelectedDecision = -1;
		private IInteractable                             m_InteractableObject;
		private bool                                      m_SkipTalkPerformed;
		private bool                                      m_BackPerformed;
		private bool                                      m_Talking;
		private IEnumerator                               m_MainCoroutine;
		private bool                                      m_SkipProcessingPerformed;
		private bool                                      m_DisplaySubtitles = true;

		private Dictionary<ETalkableCharacter, ITalkable> m_TalkableCharacters = new Dictionary<ETalkableCharacter, ITalkable>(8);

		// MONOBEHAVIOUR INTERFACE

		private void Awake()
		{
			InitializeOnSceneLoaded();
		}

		private void OnDestroy()
		{
			if (m_IngameView != null)
            {
				m_IngameView.SelectDecisionEvent.RemoveListener(OnSelectDecision);
            }

			m_IngameView = null;
			m_NavigationManager = null;
		}

		// PUBLIC METHODS

		public void Initialize()
        {
			InitializeOnSceneLoaded();

			PlayerOptions options = PlayerOptionsUtility.GetPlayerOptions();
			m_DisplaySubtitles = options.SubtitlesOn;

			Signals.GameplaySignals.OptionsChanged.Connect(OnOptionsChanged);
		}

		public void InitializeOnSceneLoaded()
		{
			m_IngameView        = FindObjectOfType<GUIIngameView>();
			m_NavigationManager = FindObjectOfType<NavigationManager>();

			if (m_IngameView != null)
            {
				m_IngameView.SelectDecisionEvent.AddListener(OnSelectDecision);
            }

			m_TalkableCharacters.Clear();
			IEnumerable talkableObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<ITalkable>();
			foreach(ITalkable talkable in talkableObjects)
            {
				m_TalkableCharacters.Add(talkable.Character, talkable);
			}
		}

		public IEnumerator ProcessInteractiveGraph(InteractiveGraph interactiveGraph, IInteractable interactableObject = null)
		{
			AdventureGame.Instance.IsBusy = true;

			//ForceStopProcessing();

			m_InteractableObject = interactableObject;
			BaseInteractiveNode firstNode = interactiveGraph.GetFirstNode() as BaseInteractiveNode;

			if (firstNode == null)
			{
				Debug.LogError($"There is no starting node in graph {interactableObject.InteractiveGraph.name}");
				yield break;
			}

			m_MainCoroutine = ProcessNode(firstNode);

			while (m_MainCoroutine != null && m_MainCoroutine.MoveNext() == true && m_SkipProcessingPerformed == false)
				yield return m_MainCoroutine.Current;

			m_SkipProcessingPerformed = false;
			m_MainCoroutine = null;
			AdventureGame.Instance.IsBusy = false;
		}

		public void ForceStopProcessing()
		{
			if (m_MainCoroutine == null)
				return;

			m_SkipProcessingPerformed = true;
		}

		public void OnSkipPerformed()
		{
			if (m_Talking == false)
				return;

			m_SkipTalkPerformed = true;
		}

		public void OnBackPerformed()
		{
			m_BackPerformed = true;
		}

		// PRIVATE METHODS

		private void OnOptionsChanged(PlayerOptions options)
        {
			m_DisplaySubtitles = options.SubtitlesOn;
        }

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
				case TalkNode dialogueNode:
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
				case GoToNode goToNode:
					yield return GoTo(goToNode);
					break;
				case ChangeSceneNode changeSceneNode:
					AdventureGame.Instance.LoadSceneAsync(changeSceneNode.SceneName);
					yield break;
				case SetDirectionNode setDirectionNode:
					AdventureGame.Instance.Player.SetDirection(setDirectionNode.Direction);
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
				isNegative = true;
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

		private IEnumerator GoTo(GoToNode node)
		{
			Player player = AdventureGame.Instance.Player;
			AdventureGame.Instance.IsBusy = false;

			m_NavigationManager.GoTo(node.Destination, player);

			while (player.IsGoing == true && m_SkipProcessingPerformed == false)
				yield return null;

			AdventureGame.Instance.IsBusy = true;
		}

		private IEnumerator Condition(ConditionNode conditionNode)
		{
			bool conditionSatisfied = false;
			bool condition = AdventureGame.Instance.GetCondition(conditionNode.ConditionName);

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

		private IEnumerator Talk(TalkNode dialogueNode)
		{
			yield return null; // Wait one frame for processing skip
			m_Talking = true;
			string text = dialogueNode.Text;
			ETalkableCharacter character = dialogueNode.Character;
			bool playTalkAnimation = dialogueNode.PlayTalkAnimation;

			if (text == null)
			{
				Debug.LogError("Text cannot be null!", this);
				yield break;
			}

			if (m_DisplaySubtitles == true)
            {
				m_IngameView.SetSubtitlesText(text);
				m_IngameView.SetSubtitlesVisibility(true);
            }

			if (playTalkAnimation == true)
			{
				ITalkable talkable = m_TalkableCharacters[character];
				if (talkable != null)
                {
					talkable.SetIsTalking(true);
				}
			}

			float secondsDelay = Mathf.Clamp(text.Length / 10f, 1f, 2.5f);
			m_SkipTalkPerformed = false;
			yield return new WaitForSecondsAndPredicate(secondsDelay, () => m_SkipTalkPerformed == false);
			m_SkipTalkPerformed = false;

			if (playTalkAnimation == true)
			{
				ITalkable talkable = m_TalkableCharacters[character];
				if (talkable != null)
				{
					talkable.SetIsTalking(false);
				}
			}

			if (m_DisplaySubtitles == true)
            {
				m_IngameView.SetSubtitlesVisibility(false);
            }

			m_Talking = false;
		}

		// HANDLERS

		private void OnSelectDecision(int index)
		{
			m_SelectedDecision = index;
		}

		// HELPERS


	}
}
