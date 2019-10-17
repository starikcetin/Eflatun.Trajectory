using System.Collections.Generic;
using UnityEngine;

namespace Eflatun.Trajectory
{
    /// <summary>
    /// A component for drawing 2D trajectories on XY plane using <see cref="LineRenderer"/>.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class TrajectoryDrawer2D : MonoBehaviour
    {
        protected TrajectoryDrawer2D()
        {
        }

        [SerializeField] private float _sampleTimePeriod;
        [SerializeField] private int _resolution;
        [SerializeField] private float _width;
        [SerializeField] private int _sortingOrder;

        private LineRenderer _lineRenderer;

        private void Start()
        {
            _lineRenderer = gameObject.GetComponent<LineRenderer>();

            _lineRenderer.useWorldSpace = true;

            //_lineRenderer.SetWidth (_width, _width); // This became obsolete. So I implemented these following 2 lines:
            _lineRenderer.startWidth = _width;
            _lineRenderer.endWidth = _width;

            _lineRenderer.sortingOrder = _sortingOrder;
        }

        /// <summary>
        /// Draws the trajectory using a <see cref="LineRenderer"/>.
        /// </summary>
        /// <param name="startPos">Starting position of trajectory.</param>
        /// <param name="initVel">Initial velocity of object.</param>
        /// <param name="gravity">Magnitude of gravity. This method assumes that gravity is directly downwards.</param>
        public void DrawTrajectory(Vector2 startPos, Vector2 initVel, float gravity)
        {
            IList<Vector2> samples =
                TrajectoryUtils2D.SampleTrajectory(_sampleTimePeriod, _resolution, gravity, initVel);
            MoveSamplesToStart(samples, startPos);
            DrawLine(samples);
            _lineRenderer.enabled = true;
        }

        /// <summary>
        /// Disables the <see cref="LineRenderer"/> used to draw trajectory.
        /// </summary>
        public void HideTrajectory()
        {
            _lineRenderer.enabled = false;
        }

        private void MoveSamplesToStart(IList<Vector2> samples, Vector2 startPos)
        {
            for (var i = 0; i < samples.Count; i++)
            {
                samples[i] += startPos;
            }
        }

        private void DrawLine(IList<Vector2> points)
        {
            //_lineRenderer.SetVertexCount (points.Count); // This became obsolete. So I wrote this following line:
            _lineRenderer.positionCount = points.Count;

            for (var i = 0; i < points.Count; i++)
            {
                Vector2 vector2 = points[i];

                _lineRenderer.SetPosition(i, vector2);
            }
        }
    }
}
