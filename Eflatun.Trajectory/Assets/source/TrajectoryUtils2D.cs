using System.Collections.Generic;
using UnityEngine;

namespace Eflatun.Trajectory
{
    /// <summary>
    /// Methods for 2D trajectory calculations and sampling on XY plane.
    /// </summary>
    /// 
    /// <remarks>
    /// All results will be relative to origin, in other words: all methods in this class assumes movement starts from origin (0, 0).
    /// </remarks>
    public static class TrajectoryUtils2D
    {
        /// <summary>
        /// Samples trajectory positions for a given flight time. <para/>
        /// Relative to origin - in other words: assumes movement starts from origin.
        /// </summary>
        /// <param name="sampleTimePeriod">The flight time in seconds to sample positions from. The last sample will be the position at this time.</param>
        /// <param name="resolution">The amount of samples.</param>
        /// <param name="gravity">Magnitude of gravity. This method assumes that gravity is directly downwards.</param>
        /// <param name="initVel">The initial velocity.</param>
        /// <returns>Sample positions from the trajectory in flight time order.</returns>
        public static IList<Vector2> SampleTrajectory(float sampleTimePeriod, int resolution, float gravity,
            Vector2 initVel)
        {
            var resultSamples = new List<Vector2>();

            if (sampleTimePeriod <= 0 || resolution <= 0)
            {
                return resultSamples;
            }

            var sampleInterval = sampleTimePeriod / resolution;

            for (var t = 0f; t <= sampleTimePeriod; t += sampleInterval)
            {
                var sample = PositionAtTime(gravity, initVel, t);
                resultSamples.Add(sample);
            }

            return resultSamples;
        }

        /// <summary>
        /// Returns the position of object at the given time. <para/>
        /// Relative to origin - in other words: assumes movement starts from origin.
        /// </summary>
        /// <param name="gravity">Magnitude of gravity vector. Gravity vector is assumed to be downwards only.</param>
        /// <param name="initVel">Initial velocity of the object.</param>
        /// <param name="time">The time.</param>
        public static Vector2 PositionAtTime(float gravity, Vector2 initVel, float time)
        {
            float finalX = XAtTime(initVel.x, time);
            float finalY = YAtTime(gravity, initVel.y, time);
            return new Vector2(finalX, finalY);
        }

        /// <summary>
        /// Returns the position of object when it is at maximum height. <para/>
        /// Relative to origin - in other words: assumes movement starts from origin.
        /// </summary>
        /// <param name="gravity">Magnitude of gravity vector. Gravity vector is assumed to be downwards only.</param>
        /// <param name="initVel">Initial velocity of the object.</param>
        public static Vector2 PositionAtMaxHeight(float gravity, Vector2 initVel)
        {
            float maxHeight = MaxHeight(gravity, initVel.y);
            float timeToReachMaxHeight = TimeAtMaxHeight(gravity, initVel.y);
            float xAtMaxHeight = XAtTime(initVel.x, timeToReachMaxHeight);

            return new Vector2(maxHeight, xAtMaxHeight);
        }

        /// <summary>
        /// Returns the X position of object at the given time. <para/>
        /// Relative to origin - in other words: assumes movement starts from origin.
        /// </summary>
        /// <param name="initVelX">X component of initial velocity of object.</param>
        /// <param name="time">The time.</param>
        public static float XAtTime(float initVelX, float time)
        {
            return initVelX * time;
        }

        /// <summary>
        /// Returns the Y position of object at the given time. <para/>
        /// Relative to origin - in other words: assumes movement starts from origin.
        /// </summary>
        /// <param name="gravity">Magnitude of gravity vector. Gravity vector is assumed to be downwards only.</param>
        /// <param name="initVelY">Y component of initial velocity of object.</param>
        /// <param name="time">The time.</param>
        public static float YAtTime(float gravity, float initVelY, float time)
        {
            return initVelY * time - gravity * time * time / 2;
        }

        /// <summary>
        /// Returns the maximum height object can reach. <para/>
        /// Relative to origin - in other words: assumes movement starts from origin.
        /// </summary>
        /// <param name="gravity">Magnitude of gravity vector. Gravity vector is assumed to be downwards only.</param>
        /// <param name="initVelY">Y component of initial velocity of object.</param>
        public static float MaxHeight(float gravity, float initVelY)
        {
            return initVelY * initVelY / 2 * gravity;
        }

        /// <summary>
        /// Returns the time required for object to reach maximum height.
        /// </summary>
        /// <param name="gravity">Magnitude of gravity vector. Gravity vector is assumed to be downwards only.</param>
        /// <param name="initVelY">Y component of initial velocity of object.</param>
        public static float TimeAtMaxHeight(float gravity, float initVelY)
        {
            return initVelY / gravity;
        }
    }
}
