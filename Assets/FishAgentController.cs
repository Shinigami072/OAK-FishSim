using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class FishAgentController : MonoBehaviour
    {
        private List<FishMovementStrategy> _strategies;
        private float _weightSum;
        private Vector3 _velocitySum;


        public Vector3 direction = Vector3.forward;
        public float velocity = 0.0f;


        private void Start()
        {
            _strategies = GetComponents<FishMovementStrategy>().ToList();
            direction = Random.onUnitSphere;
            velocity = Random.Range(0.1f, 0.5f);
        }

        private void Update()
        {
        }
        

        private void OnDrawGizmos()

        {
            var position = transform.position;
//            Debug.DrawLine(position + Vector3.up, position + Vector3.up + direction * veloctity * 1000, Color.yellow);
            Debug.DrawLine(position, position + direction * 500, Color.magenta);
        }

        private void FixedUpdate()
        {
            _velocitySum = Vector3.zero;
            _weightSum = 0f;

            foreach (var strategy in _strategies)
            {
                _velocitySum += strategy.GetVelocity(direction, velocity) * strategy.weight;
                _weightSum += strategy.weight;
            }

            var meanVelocity = _velocitySum / _weightSum;

            velocity = meanVelocity.magnitude;

            if (velocity >= 0)
            {
                var newDirection = meanVelocity.normalized;

                if ((direction - newDirection).sqrMagnitude > 0.025)
                {
                    direction = Vector3.Lerp(direction,newDirection,Time.fixedTime);
                    transform.LookAt(transform.position+direction);

                }
            }

            transform.position += meanVelocity * Time.fixedTime;
        }
    }
}