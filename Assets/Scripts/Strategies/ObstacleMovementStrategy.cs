using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ObstacleMovementStrategy : FishMovementStrategy
    {
        public float initialStress;
        public float decrease = 0.5f;
        public float _stress;
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
            var pos_o = other.ClosestPoint(pos);

            var dist = pos - pos_o;

            var dist_m = Mathf.Max(1e-20f,Mathf.Pow(dist.magnitude,3.0f));
            var stress = (obs.danger / dist_m) * Time.fixedTime;
            _stress += stress;
            var bonus = 1.0- Vector3.Angle(dist, _controller.direction) / 180;
            if (dist_m * stress *bonus> _dist)
            {
                _direction = dist.normalized;
                _dist = dist_m * stress;
            }
        }
    }
}