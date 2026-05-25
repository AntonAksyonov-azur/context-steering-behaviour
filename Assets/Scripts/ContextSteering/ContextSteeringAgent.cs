using ContextSteering.Behaviours;
using UnityEngine;

namespace ContextSteering
{
    public class ContextSteeringAgent : MonoBehaviour
    {
        [Header("Contents")]
        [SerializeField] private int _resolution;

        [SerializeField] private float _avoidDistance;
        [SerializeField] private float _movementSpeed = 20.0f;
        [SerializeField] private float _rotationSpeed = 20.0f;

        [Header("Debug")]
        [SerializeField] private Color _debugDirectionsColor = Color.green;
        [SerializeField] private float _debugDirectionLenght = 2.0f;
        [SerializeField] private Color _debugHitPointsColor = Color.red;
        [SerializeField] private float _debugHitPointsRadius = 2.0f;
        [SerializeField] private Color _debugResultVector = Color.blue;

        private Transform _transform;

        private AbstractContextSteeringBehaviour[] _behaviours;
        private Vector3[] _directions;
        private float[] _interest;
        private Vector3 _resultVector;

        //
        private ContextSteeringBehaviourAvoidance _contextSteeringBehaviourAvoidance;

        #region Game Cycle

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            SetupDirections();
            UpdateBehaviours();
            CombineInterests();
            ModifyDirections();
            SumResultMovementVector();
            
            UpdateMovement();

            // Debug
            DebugDraw();
        }


        private void OnDrawGizmos()
        {
            if (_contextSteeringBehaviourAvoidance == null)
            {
                return;
            }

            // Hit points
            Gizmos.color = _debugHitPointsColor;
            foreach (var hitPoints in _contextSteeringBehaviourAvoidance.DebugHitPoints)
            {
                Gizmos.DrawSphere(hitPoints, _debugHitPointsRadius);
            }
        }

        #endregion

        private void Initialize()
        {
            //
            _transform = GetComponent<Transform>();

            //
            _contextSteeringBehaviourAvoidance = new ContextSteeringBehaviourAvoidance(_resolution, _avoidDistance);

            _behaviours = new AbstractContextSteeringBehaviour[]
            {
                new ContextSteeringBehaviourDirectionForward(_resolution),
                _contextSteeringBehaviourAvoidance
            };

            // Initial
            _directions = new Vector3[_resolution];
            _interest = new float[_resolution];
        }

        private void SetupDirections()
        {
            var radiansInterval = Mathf.PI * 2 / _resolution;
            for (var i = 0; i < _resolution; i++)
            {
                var directionAngle = radiansInterval * i;
                var directionVector = new Vector3(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                _directions[i] = directionVector;
            }
        }

        private void UpdateBehaviours()
        {
            foreach (var behaviour in _behaviours)
            {
                behaviour.UpdateBehaviour(_transform, _directions);
            }
        }

        private void CombineInterests()
        {
            for (var index = 0; index < _resolution; index++)
            {
                // Clear
                _interest[index] = 0;

                // Combine
                foreach (var behaviour in _behaviours)
                {
                    _interest[index] = Mathf.Max(0, _interest[index] + behaviour.Interest[index] - behaviour.Danger[index]);
                }
            }
        }

        private void ModifyDirections()
        {
            for (var index = 0; index < _resolution; index++)
            {
                _directions[index] = _interest[index] * _directions[index];
            }
        }

        private void SumResultMovementVector()
        {
            _resultVector = Vector3.zero;
            for (var index = 0; index < _resolution; index++)
            {
                var direction = _directions[index];
                _resultVector += direction;
            }

            _resultVector = _resultVector.normalized;
        }

        private void UpdateMovement()
        {
            // Movement
            _transform.position += _resultVector * (_movementSpeed * Time.deltaTime);
            
            // Rotate by movement vector
            var lookDirection = _transform.position + _resultVector - _transform.position;
            var angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
            var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }

        #region Debug

        private void DebugDraw()
        {
            for (var index = 0; index < _directions.Length; index++)
            {
                var direction = _directions[index];
                var d = direction.normalized * (_interest[index] * _debugDirectionLenght);
                Debug.DrawLine(_transform.position, _transform.position + d, _debugDirectionsColor);
            }

            Debug.DrawLine(_transform.position, _transform.position + _resultVector * _debugDirectionLenght, _debugResultVector);
        }

        #endregion
    }
}