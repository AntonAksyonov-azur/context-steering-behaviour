using UnityEngine;

namespace ContextSteering.Behaviours
{
    public class ContextSteeringBehaviourDirectionForward : AbstractContextSteeringBehaviour
    {
        public ContextSteeringBehaviourDirectionForward(int resolution) : base(resolution)
        {
        }

        #region Overrides of AbstractContextSteeringBehaviour

        protected override void UpdateInterest(Transform agentTransform, Vector3[] directions, float[] interest)
        {
            var forwardDirection = agentTransform.up;
            for (var i = 0; i < Resolution; i++)
            {
                var direction = directions[i]; 
                interest[i] = Vector3.Dot(forwardDirection, direction);
            }
        }

        protected override void UpdateDanger(Transform agentTransform, Vector3[] directions, float[] danger)
        {
            for (var i = 0; i < danger.Length; i++)
            {
                danger[i] = 0;
            }
        }

        #endregion
    }
}