using System;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.Robotics.MessageVisualizers
{
    public abstract class RaycastPublisher : MonoBehaviour
    {
        [SerializeField]
        protected string m_Topic;
        protected ROSConnection m_Ros;
        protected ClickState m_State = ClickState.None;
        protected string m_DeselectedLabel;
        protected string m_SelectedLabel;
        public string CurrentLabel => m_State == ClickState.None ? m_DeselectedLabel : m_SelectedLabel;

        // Start is called before the first frame update
        public virtual void Start()
        {
            m_Ros = ROSConnection.GetOrCreateInstance();
        }

        protected bool ValidClick()
        {
            return m_State != ClickState.None && !EventSystem.current.IsPointerOverGameObject();
        }

        /// <summary>
        /// Returns true if the mouse raycast hit, as well as the RaycastHit
        /// </summary>
        /// <param name="state">ClickState to check raycast during</param>
        /// <returns></returns>
        protected virtual (bool, RaycastHit) RaycastCheck(ClickState state)
        {
            var isHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit) &&
                m_State == state;
            return (isHit, hit);
        }

        public void ToggleInteraction()
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

        protected enum ClickState
        {
            None,
            Started,
            Held
        }
    }
}
