using System;
using UnityEngine;

namespace Unity.Robotics.MessageVisualizers
{
    public class HudButton : VisualizationTrigger, IHudButton
    {
        [SerializeField]
        string m_DeselectedLabel;
        [SerializeField]
        string m_SelectedLabel;
        string m_Label => m_State == ClickState.None ? m_DeselectedLabel : m_SelectedLabel;

        public void DrawGUI()
        {
            if (GUILayout.Button(m_Label))
            {
                Interact();
            }
        }
    }
}
