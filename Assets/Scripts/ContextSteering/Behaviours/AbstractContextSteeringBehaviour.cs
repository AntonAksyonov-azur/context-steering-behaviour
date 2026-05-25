using System.Collections.Generic;
using UnityEngine;

namespace ContextSteering.Behaviours
{
    public abstract class AbstractContextSteeringBehaviour
    {
        public IReadOnlyList<float> Interest => _interest;
        public IReadOnlyList<float> Danger => _danger;

        protected readonly int Resolution;
        
        private readonly float[] _interest;
        private readonly float[] _danger;
     
        private readonly Vector3[] _directions;

        protected AbstractContextSteeringBehaviour(int resolution)
        {
            Resolution = resolution;
            
            _interest = new float[resolution];
            _danger = new float[resolution];
        }

        #region Required

        protected abstract void UpdateInterest(Transform agentTransform, Vector3[] directions, float[] interest);
        protected abstract void UpdateDanger(Transform agentTransform, Vector3[] directions, float[] danger);

        #endregion

        public void UpdateBehaviour(Transform agentPosition, Vector3[] directions)
        {
            UpdateInterest(agentPosition, directions, _interest);
            UpdateDanger(agentPosition, directions, _danger);
        }
    }
}