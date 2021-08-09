using System;
using UnityEngine;

namespace Unity.Robotics.MessageVisualizers
{
    [RequireComponent(typeof(RosPublish))]
    public class InteractionManager : MonoBehaviour
    {
        VisualizationTrigger m_HudButton;
        Interaction m_Interaction;

        // Start is called before the first frame update
        void Start()
        {
            m_HudButton = GetComponent<VisualizationTrigger>();
            if (m_HudButton == null)
            {
                Debug.LogError($"Requires component of type VisualizationTrigger!");
            }
            m_Interaction = GetComponent<Interaction>();
            if (m_Interaction == null)
            {
                Debug.LogError($"Requires component of type Interaction!");
            }

            m_HudButton.Subscribe(m_Interaction);
        }
    }
}
