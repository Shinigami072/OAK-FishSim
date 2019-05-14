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

        private Vector3 _sumDirection = Vector3.zero;
        private Vector3 _sumSpeed = Vector3.zero;
        private float _fishCount = 0;
        private Vector3 _outDir = Vector3.zero;
        private Vector3 _sumDirectionCalc;
        private Vector3 _sumSpeedCalc;
        private float _fishCountCalc;
        private FishAgentController controller;

        private void Start()
        {
            controller = GetComponent<FishAgentController>();
            //range check
            if (repulsionRange > alignmentRange || alignmentRange > attractionRange || repulsionRange > attractionRange)
                throw new ArgumentOutOfRangeException();
            if (maxVelocity <= 0)
                throw new ArgumentOutOfRangeException("maxVelocity", "it should be >0");
        }

        private void FixedUpdate()
        {
            //transfer calculated values
//            _sumDirectionCalc = _sumDirection;
            _sumSpeedCalc = _sumSpeed;
            _fishCountCalc = _fishCount;

            //resetAccumulators
            _sumSpeed = Vector3.zero;
//            _sumDirection = Vector3.zero;
            _fishCount = 0;
        }

//        public override Vector3 GetDirection(Vector3 direction, float velocity)
//        {
//            _outDir = ((_sumSpeedCalc + direction * velocity) / (_fishCountCalc + 1));
//            return ((_sumSpeedCalc + direction * velocity) / (_fishCountCalc + 1)).normalized;
//        }

        public override Vector3 GetVelocity(Vector3 direction, float velocity)
        {
            return ((_sumSpeedCalc + direction * velocity) / (_fishCountCalc + 1));
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

            Debug.DrawLine(position + Vector3.up, position + Vector3.up + _outDir * 1000, Color.blue);
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("FishAgent"))
            {
                FishAgentController otherFish = other.gameObject.GetComponent<FishAgentController>();

                var position = transform.position;
                var otherPosition = other.transform.position;

                var dist = Vector3.Distance(position, otherPosition);

//                var dir = 0.1f * Vector3.Dot(controller.direction, (otherPosition - position).normalized);
//                if (dir < 0.05f)
//                    dir = 0;
//                dir += 1.0f;
                var dir = 1.0f;
                //not collide
                if (dist < repulsionRange)
                {
                    _fishCount += dir;
                    var d = -(otherPosition - position).normalized * dir;
//                    _sumDirection += d;
                    _sumSpeed += d * maxVelocity * (1 - dist / repulsionRange);
                }

                //align movement
                else if (dist < alignmentRange)
                {
                    _fishCount += dir;
                    var d = otherFish.direction * dir;
//                    _sumDirection += d;
                    _sumSpeed += d * otherFish.veloctity;
                }

                //move together
                else if (dist < attractionRange)
                {
                    _fishCount += dir;
                    var d = (otherPosition - position).normalized * dir;
//                    _sumDirection += d;
                    _sumSpeed += d * maxVelocity * (dist - alignmentRange) / (attractionRange - alignmentRange);
                }
            }
        }
    }
}