using UnityEngine;

namespace ContextSteering.Behaviours
{
    public class ContextSteeringBehaviourAvoidance : AbstractContextSteeringBehaviour
    {
        private readonly float _avoidDistance;

        public ContextSteeringBehaviourAvoidance(int resolution, float avoidDistance) : base(resolution)
        {
            _avoidDistance = avoidDistance;
        }

        #region Overrides of AbstractContextSteeringBehaviour

        protected override void UpdateInterest(Transform agentTransform, Vector2[] directions, float[] interest)
        {
            for (var i = 0; i < interest.Length; i++)
            {
                interest[i] = 0;
            }
        }

        protected override void UpdateDanger(Transform agentTransform, Vector2[] directions, float[] danger)
        {
            for (var i = 0; i < Resolution; i++)
            {
                var direction = directions[i];
                
                var hit = Physics2D.Raycast(agentTransform.position, direction, _avoidDistance);
                if (hit.collider == null)
                {
                    continue;
                }

                var vectorHitPointDirection = hit.point - (Vector2)agentTransform.position;
                var vectorHitPointDirectionNormalized = vectorHitPointDirection.normalized;
                danger[i] = Vector2.Dot(direction, vectorHitPointDirectionNormalized);
            }
        }

        #endregion
    }
}