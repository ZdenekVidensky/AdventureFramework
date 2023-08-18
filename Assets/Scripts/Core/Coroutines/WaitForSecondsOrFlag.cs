namespace TVB.Core.Coroutines
{
    using UnityEngine;

    public class WaitForSecondsAndPredicate : CustomYieldInstruction
    {
        // PRIVATE MEMBERS

        private System.Func<bool> m_Predicate;
        private float m_EndTime;

        // YIELD INSTRUCTION INTERFACE

        public override bool keepWaiting
        {
            get
            {
                return m_Predicate() == true && m_EndTime > Time.realtimeSinceStartup;
            }
        }

        public WaitForSecondsAndPredicate(float seconds, System.Func<bool> prediate)
        {
            m_Predicate = prediate;
            m_EndTime = Time.realtimeSinceStartup + seconds;
        }
    }
}
