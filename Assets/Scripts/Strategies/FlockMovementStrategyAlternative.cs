using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class FlockMovementStrategyAlternative : FishMovementStrategy
    {
        public float repulsionRange = 3.5f;
        public float alignmentRange = 7f;
        public float attractionRange = 18f;
        public float maxVelocity = 0.5f;
        public float mainWeight = 1.0f;
        public float refreshRate = 1.5f;

        private Vector3 _sumSpeed = Vector3.zero;
        private float _fishCount = 0.0f;

        private Vector3 _outSpeed = Vector3.zero;
        private FishAgentController _controller;
        private LinkedList<FishAgentController> _otherFish;

        private void Start()
        {
            _controller = GetComponent<FishAgentController>();
            _otherFish = new LinkedList<FishAgentController>();

            //range check
            if (repulsionRange > alignmentRange || alignmentRange > attractionRange || repulsionRange > attractionRange)
                throw new ArgumentOutOfRangeException("", "Ranges misplaced");
            if (maxVelocity <= 0)
                throw new ArgumentOutOfRangeException(paramName: "", message: "maxVlocity should be >0");
            StartCoroutine("FlockCalculations");
        }

        IEnumerator FlockCalculations()
        {
            while (true)
            {
                if (!_controller.enabled)
                    break;
                yield return new WaitForFixedUpdate();
                _sumSpeed = _controller.direction * _controller.velocity;
                _fishCount = mainWeight;
                var position = _controller.transform.position;

                foreach (var other in _otherFish)
                {
                    var otherPosition = other.transform.position;
                    var dist = otherPosition - position;
                    var distMagnitude = dist.magnitude;

                    var weightFish = 1.0f - 0.5F * (Vector3.Angle(_controller.direction, dist) / 180.0f);

                    //not collide
                    if (distMagnitude < repulsionRange)
                    {
                        _fishCount += weightFish;
                        var direction = -(dist).normalized * weightFish;
                        _sumSpeed += maxVelocity * (1 - distMagnitude / repulsionRange) * direction;
                    }

                    //align movement
                    else if (distMagnitude < alignmentRange)
                    {
                        _fishCount += weightFish;
                        var direction = other.direction * weightFish;
                        _sumSpeed += direction * other.velocity;
                    }

                    //move together
                    else if (distMagnitude < attractionRange)
                    {
                        _fishCount += weightFish;
                        var direction = (dist).normalized * weightFish;
                        _sumSpeed += direction * maxVelocity * (distMagnitude - alignmentRange) /
                                     (attractionRange - alignmentRange);
                    }
                }

                _outSpeed = (_sumSpeed) / _fishCount;

            }
        }

        public override Vector3 GetVelocity(Vector3 direction, float velocity)
        {
            return _outSpeed;
        }

        //Debug Drawing
        private void OnDrawGizmos()

        {
            var position = transform.position;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, attractionRange);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(position, alignmentRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, repulsionRange);

            Debug.DrawLine(position, position + _outSpeed * 100, Color.blue);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("FishAgent")) return;

            _otherFish.AddFirst(other.gameObject.GetComponent<FishAgentController>());
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("FishAgent")) return;

            _otherFish.Remove(other.gameObject.GetComponent<FishAgentController>());
        }
    }
}