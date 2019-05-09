using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FlockMovementStrategy : FishMovementStrategy
    {
        public float RepulsionRange;
        public float AlignmentRange;
        public float AttractionRange;


        private Vector3 _sumDirection;
        private float _sumSpeed;
        private int _fishCount;
        private void Start()
        {
            if(RepulsionRange>AlignmentRange || AlignmentRange>AttractionRange || RepulsionRange>AttractionRange)
                throw new ArgumentOutOfRangeException("Ranges set up wrong");
            
            
        }

        private void FixedUpdate()
        {
            
            _sumDirection = Vector3.zero;
            _sumSpeed = 0f;
            _fishCount = 0;
        }

        public override Vector3 GetDirection(Vector3 direction, float velocity)
        {
            throw new System.NotImplementedException();
        }

        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("FishAgent"))
            {
                FishAgentController otherFish = other.gameObject.GetComponent<FishAgentController>();
                var position = transform.position;
                Vector3 otherPosition = other.gameObject.GetComponentInChildren<Collider>().ClosestPoint(position);
               
                var dist = Vector3.Distance(position, otherPosition);

                if (dist < RepulsionRange)
                {
                    _sumDirection+= -(otherPosition - position).normalized;
                }
                else if(dist < AlignmentRange)
                {
                    _sumDirection += otherFish.direction;
                    _sumSpeed += otherFish.veloctity;
                }
                else if(dist<AttractionRange)
                {
                    _sumDirection += (otherPosition - position).normalized;
                }
            }
        }
    }
}