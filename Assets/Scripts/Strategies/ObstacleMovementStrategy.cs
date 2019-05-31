using System;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace DefaultNamespace
{
    public class ObstacleMovementStrategy : FishMovementStrategy
    {
        public float initialStress = 0.0f;
        public float decrease = 0.5f;
        private float _stress;
        public float maxSpeed = 0.01f;
        private Vector3 _direction;
        private Vector3 _directionC;
        private FishAgentController _controller;

        private float _dist;

        private void Start()
        {
            _stress = initialStress;

            _controller = GetComponent<FishAgentController>();
        }

        private void FixedUpdate()
        {
            if (_stress > 0.0)
            {
                _stress -= Time.fixedTime * decrease;
            }

            if (_stress < 0.0)
            {
                _stress = 0.0f;
            }
            
            _directionC = _direction;
            _direction = Vector3.zero;
            _dist = -1;
        }

        public override Vector3 GetVelocity(Vector3 direction, float velocity)
        {
            if (_stress > 0.3f)
                return maxSpeed * Mathf.Pow(_stress,2.0f) * _directionC;
            else
                return direction*velocity;
        }

        private void OnTriggerStay(Collider other)
        {
            var obs = other.GetComponent<Obstacle>();
            if (obs == null)
                return;

            var pos = transform.position+_controller.velocity * Time.fixedTime * _controller.direction;
            var posO = other.ClosestPoint(pos);
            var swapped = false;
            if (posO == pos)
            {
                posO = other.bounds.center;
                swapped = true;
            }

            var dist = pos - posO ;

            var distM = dist.magnitude;
            if (!swapped)
                distM += 20.0f;
            
            if(distM<22)
                print("nowisnan"+distM);
            distM /= 10.0f;
            var stress = (obs.danger / distM) * Time.fixedTime;
            _stress += stress;
            
            var bonus = 1.0- Vector3.Angle(dist, _controller.direction) / 180;
            if (distM * stress *bonus> _dist)
            {
                _direction = dist.normalized;
                _dist = distM * stress;
            }
        }
    }
}