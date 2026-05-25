using ContextSteering.Behaviours;
using UnityEngine;

namespace ContextSteering
{
    public class ContextSteeringAgent : MonoBehaviour
    {
        [Header("Contents")]
        [SerializeField] private int _resolution;

        private Transform _transform;
        
        private AbstractContextSteeringBehaviour[] _behaviours;
        private Vector3[] _directions;

        #region Game Cycle

        private void Awake()
        {
            Initialize();
        }
        
        private void Update()
        {
            UpdateDirections(_directions);
            UpdateBehaviours(_behaviours);
        }

        #endregion

        private void Initialize()
        {
            //
            _transform = GetComponent<Transform>();
            
            //
            _behaviours = new AbstractContextSteeringBehaviour[]
            {
                new ContextSteeringBehaviourDirectionForward(_resolution),
            };

            _directions = new Vector3[_resolution];
        }

        private void UpdateDirections(Vector3[] directions)
        {
            var radiansInterval = Mathf.PI * 2 / _resolution;
            for (var i = 0; i < _resolution; i++)
            {
                var directionAngle = radiansInterval * i;
                var directionVector = new Vector3(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle), 0.0f);
                directions[i] = directionVector;
            }
        }

        private void UpdateBehaviours(AbstractContextSteeringBehaviour[] behaviours)
        {
            foreach (var behaviour in behaviours)
            {
                behaviour.UpdateBehaviour(_transform, _directions);
            }
        }
    }
}