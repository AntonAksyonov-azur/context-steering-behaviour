using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ContextSteering.Behaviours
{
    public class ContextSteeringBehaviourAvoidance : AbstractContextSteeringBehaviour
    {
        // Debug
        public readonly List<Vector3> DebugHitPoints = new List<Vector3>();
        
        private readonly float _avoidDistance; 
        private readonly ContactFilter2D _contactFilter2D;
        private readonly List<RaycastHit2D> _raycastHits = new  List<RaycastHit2D>();

        public ContextSteeringBehaviourAvoidance(int resolution, float avoidDistance) : base(resolution)
        {
            _avoidDistance = avoidDistance;
            
            _contactFilter2D = new ContactFilter2D
            {
                // useLayerMask = true,
                // layerMask = LayerMask.GetMask("Obstacle")
            };
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
                
                var hits = Physics2D.Raycast(agentTransform.position, direction, _contactFilter2D, _raycastHits, _avoidDistance);
                if (hits == 0)
                {
                    continue;
                } 

                var firstObstacleHit = _raycastHits.FirstOrDefault(a => a.transform != agentTransform);
                if (firstObstacleHit.transform == null)
                {
                    continue;
                }
                
                var vectorHitPointDirection = (Vector3)firstObstacleHit.point - agentTransform.position;
                var vectorHitPointDirectionNormalized = vectorHitPointDirection.normalized;
                danger[i] = Vector3.Dot(direction, vectorHitPointDirectionNormalized);
                
                // Debug
                DebugHitPoints.Add(firstObstacleHit.point);
            }
        }

        #endregion
    }
}