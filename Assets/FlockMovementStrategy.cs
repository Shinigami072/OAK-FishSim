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
        private float _sumSpeed = 0;
        private int _fishCount = 0;
        private Vector3 _outDir = Vector3.zero;
        private Vector3 _sumDirectionCalc;
        private float _sumSpeedCalc;
        private int _fishCountCalc;

        private void Start()
        {
            //range check
            if (repulsionRange > alignmentRange || alignmentRange > attractionRange || repulsionRange > attractionRange)
                throw new ArgumentOutOfRangeException();
            if(maxVelocity<=0)
                throw new ArgumentOutOfRangeException("maxVelocity","it should be >0");
            
        }

        private void FixedUpdate()
        {
            _sumDirectionCalc = _sumDirection;
            _sumDirection = Vector3.zero;
            _sumSpeedCalc = _sumSpeed;
            _sumSpeed = 0f;
            _fishCountCalc = _fishCount;
            _fishCount = 0;
        }

        public override Vector3 GetDirection(Vector3 direction, float velocity)
        {
            _outDir =  (_sumDirectionCalc + direction) / (_fishCountCalc + 1);

            return (_sumDirectionCalc + direction) / (_fishCountCalc + 1);
        }

        public override float GetVelocity(Vector3 direction, float velocity)
        {
            return (_sumSpeedCalc + velocity) /
                 (_fishCountCalc + 1);
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
                _fishCount++;
                FishAgentController otherFish = other.gameObject.GetComponent<FishAgentController>();
                var position = transform.position;
                Vector3 otherPosition = other.transform.position;

                var dist = Vector3.Distance(position, otherPosition);

                if (dist < repulsionRange)
                {
                    _sumDirection += -(otherPosition - position).normalized;
                    _sumSpeed += maxVelocity * (1 - dist / repulsionRange);
                }
                else if (dist < alignmentRange)
                {
                    _sumDirection += otherFish.direction;
                    _sumSpeed += otherFish.veloctity;
                }
                else if (dist < attractionRange)
                {
                    _sumDirection += (otherPosition - position).normalized;
                    _sumSpeed += maxVelocity * ((dist-alignmentRange) / (attractionRange-alignmentRange));
                }

            }
        }
    }
}