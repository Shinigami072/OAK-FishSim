using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class FishAgentController : MonoBehaviour
    {
        private List<FishMovementStrategy> _strategies;
        private Vector3 _velocitySum;
        private float _weightSum;


        public Vector3 direction = Vector3.forward;
        private Vector3 oldDir;
        private float t;
        public float velocity;


        private void Start()
        {
            _strategies = GetComponents<FishMovementStrategy>().ToList();
            _strategies.RemoveAll(it => !it.enabled);
            direction = transform.TransformDirection(Vector3.forward);
            oldDir = direction;
            velocity = Random.Range(0.1f, 0.5f);
        }

        private void OnDrawGizmos()

        {
            var position = transform.position;
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

            if (_weightSum <= 0)
                return;

            var meanVelocity = _velocitySum / _weightSum;
            var diff = meanVelocity.magnitude - velocity;
            velocity += Mathf.Sign(diff) * Mathf.Min(Mathf.Abs(diff), 0.003f * Time.fixedTime);
            if (float.IsNaN(velocity))
            {
                print("fish" + _weightSum + " " + _velocitySum + "," + direction + "->" + velocity + name);
                velocity = 0.0f;
            }

            if (velocity >= 0)
            {
                var newDirection = meanVelocity.normalized;

                if ((oldDir - newDirection).sqrMagnitude > 0.02)
                {
                    oldDir = newDirection;
                    t = Time.fixedTime;
                }
                else
                {
                    if (direction != oldDir)
                    {
                        direction = Vector3.Lerp(direction, oldDir, (Time.fixedTime - t) / 10.0f);
                        transform.LookAt(transform.position + direction);
                    }
                }

                transform.position += meanVelocity * Time.fixedDeltaTime;
            }
        }
    }
}