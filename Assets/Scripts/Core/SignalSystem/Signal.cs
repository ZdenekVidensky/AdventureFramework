namespace TVB.Core.SignalSystem
{
    using System;

    public class Signal
    {
        private event Action<int> m_EventHandler;
        private event Action m_EventHandlerWithoutID;

        public void Connect(Action<int> action)
        {
            m_EventHandler += action;
        }

        public void Disconnect(Action<int> action)
        {
            m_EventHandler -= action;
        }

        public void Connect(Action action)
        {
            m_EventHandlerWithoutID += action;
        }

        public void Disconnect(Action action)
        {
            m_EventHandlerWithoutID -= action;
        }

        public void Emit(int ID)
        {
            if (m_EventHandler == null)
                return;

            m_EventHandler.Invoke(ID);
        }

        public void Emit()
        {
            if (m_EventHandlerWithoutID == null)
                return;

            m_EventHandlerWithoutID.Invoke();
        }
    }

    public class Signal<T>
    {
        private event Action<int, T> m_EventHandler;
        private event Action<T> m_EventHandlerWithoutID;

        public void DisconnectAll()
        {
            m_EventHandler = null;
            m_EventHandlerWithoutID = null;
        }

        public void Connect(Action<T> action)
        {
            m_EventHandlerWithoutID += action;
        }

        public void Disconnect(Action<T> action)
        {
            m_EventHandlerWithoutID -= action;
        }

        public void Connect(Action<int, T> action)
        {
            m_EventHandler += action;
        }

        public void Disconnect(Action<int, T> action)
        {
            m_EventHandler -= action;
        }

        public void Emit(int ID, T args)
        {
            if (m_EventHandler == null)
                return;

            m_EventHandler.Invoke(ID, args);
        }

        public void Emit(T args)
        {
            if (m_EventHandlerWithoutID == null)
                return;

            m_EventHandlerWithoutID.Invoke(args);
        }
    }
}
