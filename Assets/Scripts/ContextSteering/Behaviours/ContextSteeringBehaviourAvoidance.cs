using System.Collections.Generic;
using UnityEngine;

namespace ContextSteering.Behaviours
{
    public class ContextSteeringBehaviourAvoidance : AbstractContextSteeringBehaviour
    {
        // Debug
        public readonly List<Vector3> DebugHitPoints = new List<Vector3>();
        
        private readonly float _avoidDistance;

        public ContextSteeringBehaviourAvoidance(int resolution, float avoidDistance) : base(resolution)
        {
            _avoidDistance = avoidDistance;
        }

        #region Overrides of AbstractContextSteeringBehaviour

        protected override void UpdateInterest(Transform agentTransform, Vector3[] directions, float[] interest)
        {
            for (var i = 0; i < interest.Length; i++)
            {
                interest[i] = 0;
            }
        }

        protected override void UpdateDanger(Transform agentTransform, Vector3[] directions, float[] danger)
        {
            // Debug
            DebugHitPoints.Clear();
            
            // Update
            for (var i = 0; i < Resolution; i++)
            {
                // Clear
                danger[i] = 0;
                
                // Detect obstacle
                var direction = directions[i];
                
                var hit = Physics2D.Raycast(agentTransform.position, direction, _avoidDistance);
                if (hit.collider == null)
                {
                    continue;
                }

                var vectorHitPointDirection = (Vector3)hit.point - agentTransform.position;
                var vectorHitPointDirectionNormalized = vectorHitPointDirection.normalized;
                danger[i] = Vector3.Dot(direction, vectorHitPointDirectionNormalized);
                
                // Debug
                DebugHitPoints.Add(hit.point);
            }
        }

        #endregion
    }
}