using System;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

namespace Unity.Robotics.MessageVisualizers
{
    public abstract class PoseRaycastPublisher : RaycastPublisher
    {
        [SerializeField]
        Color m_Color;
        BasicDrawing m_ArrowDrawing;
        Vector3[] m_MouseClicks = new Vector3[2];

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            m_ArrowDrawing = BasicDrawingManager.CreateDrawing();
        }

        // Update is called once per frame
        void Update()
        {
            if (!ValidClick())
            {
                return;
            }

            if (Input.GetMouseButtonDown(0)) // Begin click
            {
                BeginClick();
            }

            if (Input.GetMouseButton(0)) // Update arrow during drag
            {
                UpdateClick();
            }

            if (Input.GetMouseButtonUp(0)) // Release click
            {
                ReleaseClick();
            }
        }

        void BeginClick()
        {
            var (didHit, hit) = RaycastCheck(ClickState.Started);
            if (didHit)
            {
                m_MouseClicks[0] = hit.point;
                m_State = ClickState.Held;
            }
        }

        void UpdateClick()
        {
            var (didHit, hit) = RaycastCheck(ClickState.Held);
            if (didHit)
            {
                m_ArrowDrawing.Clear();
                // Draw normalized arrow in direction of mouse position
                m_ArrowDrawing.DrawArrow(m_MouseClicks[0],
                    1.5f * Vector3.Normalize(hit.point - m_MouseClicks[0]) + m_MouseClicks[0], m_Color,
                    arrowheadScale: 2f);
            }
        }

        protected virtual bool ReleaseClick()
        {
            m_ArrowDrawing.Clear();
            var (didHit, hit) = RaycastCheck(ClickState.Held);
            if (didHit)
            {
                m_MouseClicks[1] = hit.point;
                m_State = ClickState.None;
            }

            return didHit;
        }

        /// <summary>
        /// Calculates and formats ROS-formatted PoseMsg based on mouse interactions
        /// </summary>
        protected PoseMsg CalculatePoseMsg()
        {
            var diff = (m_MouseClicks[1] - m_MouseClicks[0]).normalized;
            var rot = diff == Vector3.zero ? Quaternion.identity : Quaternion.LookRotation(diff);
            return new PoseMsg
            {
                position = m_MouseClicks[0].To<FLU>(),
                orientation = rot.To<FLU>()
            };
        }
    }
}
