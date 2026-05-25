using UnityEngine;

namespace ContextSteering.Behaviours
{
    public class ContextSteeringBehaviourAvoidance : AbstractContextSteeringBehaviour
    {
        public ContextSteeringBehaviourAvoidance(int resolution) : base(resolution)
        {
        }

        #region Overrides of AbstractContextSteeringBehaviour

        protected override void UpdateInterest(Transform agentTransform, Vector3[] directions, float[] interest)
        {
            throw new System.NotImplementedException();
        }

        protected override void UpdateDanger(Transform agentTransform, Vector3[] directions, float[] danger)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}