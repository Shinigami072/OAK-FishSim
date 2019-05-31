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
        private float t;
        private Vector3 _velocitySum;
        private Vector3 oldDir;


        public Vector3 direction = Vector3.forward;
        public float velocity = 0.0f;


        private void Start()
        {
            _strategies = GetComponents<FishMovementStrategy>().ToList();
            direction = Random.onUnitSphere;
            oldDir = direction;
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

                if ((oldDir - newDirection).sqrMagnitude > 0.02)
                {
                   oldDir=newDirection;
                   t = 0;
                }
                else
                {
                    t += Time.fixedTime;
                    if (direction != oldDir)
                    {
                        direction = Vector3.Lerp(direction, oldDir, t / 20.0f);
                        transform.LookAt(transform.position + direction);
                    }
                }
            }
            transform.position += meanVelocity * Time.fixedTime;
        }
    }
}