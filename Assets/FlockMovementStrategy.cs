using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class FlockMovementStrategy : FishMovementStrategy
    {
        public float repulsionRange = 3.5f;
        public float alignmentRange = 7f;
        public float attractionRange = 18f;
        public float maxVelocity = 0.5f;
        public float mainWeight = 1.0f;

        private Vector3 _sumSpeed = Vector3.zero;
        private float _fishCount = 0;

        private Vector3 _outSpeed = Vector3.zero;
        private Vector3 _sumSpeedCalc;
        private float _fishCountCalc;
        private FishAgentController _controller;

        private void Start()
        {
            _controller = GetComponent<FishAgentController>();
            //range check
            if (repulsionRange > alignmentRange || alignmentRange > attractionRange || repulsionRange > attractionRange)
                throw new ArgumentOutOfRangeException("", "Ranges misplaced");
            if (maxVelocity <= 0)
                throw new ArgumentOutOfRangeException(paramName: "", message: "maxVlocity should be >0");
        }

        private void FixedUpdate()
        {
            //transfer calculated values
            _sumSpeedCalc = _sumSpeed;
            _fishCountCalc = _fishCount + mainWeight;
            _outSpeed = (_sumSpeedCalc + _controller.direction * _controller.velocity) / _fishCountCalc;

            //resetAccumulators
            _sumSpeed = Vector3.zero;
            _fishCount = 0;
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


        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("FishAgent")) return;

            var otherFish = other.gameObject.GetComponent<FishAgentController>();

            var position = transform.position;
            var otherPosition = other.transform.position;

            var dist = otherPosition - position;
            var distMagnitude = dist.magnitude;


            var weightFish = 1.0f;//+ 0.1f * Vector3.Dot(_controller.direction, (dist).normalized);

            if (weightFish < 1.05f)
                weightFish = 1.0f;
            
//            var weightFish = 1.0f;
            //not collide
            if (distMagnitude < repulsionRange)
            {
                _fishCount += weightFish;
                var direction = -(dist).normalized * weightFish;
                _sumSpeed += direction * maxVelocity * (1 - distMagnitude / repulsionRange);
            }

            //align movement
            else if (distMagnitude < alignmentRange)
            {
                _fishCount += weightFish;
                var direction = otherFish.direction * weightFish;
                _sumSpeed += direction * otherFish.velocity;
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
    }
}