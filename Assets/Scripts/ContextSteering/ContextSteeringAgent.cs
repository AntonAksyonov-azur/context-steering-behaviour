using ContextSteering.Behaviours;
using UnityEngine;

namespace ContextSteering
{
    public class ContextSteeringAgent : MonoBehaviour
    {
        [Header("Contents")]
        [SerializeField] private int _resolution;
        [SerializeField] private float _avoidDistance;

        private Transform _transform;

        private AbstractContextSteeringBehaviour[] _behaviours;
        private Vector2[] _directions;

        #region Game Cycle

        private void Awake()
        {
            Initialize();
            
            UpdateDirections(_directions);
            UpdateBehaviours(_behaviours);
        }

        private void Update()
        {
            // UpdateDirections(_directions);
            // UpdateBehaviours(_behaviours);
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
                new ContextSteeringBehaviourAvoidance(_resolution, _avoidDistance),
            };

            _directions = new Vector2[_resolution];
        }

        private void UpdateDirections(Vector2[] directions)
        {
            var radiansInterval = Mathf.PI * 2 / _resolution;
            for (var i = 0; i < _resolution; i++)
            {
                var directionAngle = radiansInterval * i;
                var directionVector = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
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