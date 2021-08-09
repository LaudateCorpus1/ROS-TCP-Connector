using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Robotics.MessageVisualizers
{
    public enum ClickState
    {
        None,
        Started,
        Held
    }

    public interface IHudButton
    {
        void DrawGUI();
    }

    public abstract class VisualizationTrigger : MonoBehaviour
    {
        List<Interaction> m_Interactions = new List<Interaction>();
        protected ClickState m_State;

        public void Reset()
        {
            m_State = ClickState.None;
        }

        public void Subscribe(Interaction interaction)
        {
            m_Interactions.Add(interaction);
        }

        protected void Interact()
        {
            OnNext();
            foreach (var obs in m_Interactions) obs.OnNext(m_State);
        }

        void OnNext()
        {
            if (m_State != ClickState.Started)
            {
                m_State = ClickState.Started;
            }
            else
            {
                m_State = ClickState.None;
            }
        }
    }
}
