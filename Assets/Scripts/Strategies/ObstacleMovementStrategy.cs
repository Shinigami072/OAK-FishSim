using UnityEngine;

namespace DefaultNamespace
{
    public class ObstacleMovementStrategy : FishMovementStrategy
    {
        private FishAgentController _controller;
        private Vector3 _direction;
        private Vector3 _directionC;

        private float _dist;
        private float _stress;
        public float decrease = 0.5f;
        public float initialStress;
        public float maxSpeed = 0.01f;

        private void Start()
        {
            _stress = initialStress;

            _controller = GetComponent<FishAgentController>();
        }

        private void FixedUpdate()
        {
            if (_stress > 0.0) _stress -= Time.fixedDeltaTime * decrease;

            if (_stress < 0.0) _stress = 0.0f;

            _directionC = _direction;
            _direction = Vector3.zero;
            _dist = -1;
        }

        public override Vector3 GetVelocity(Vector3 direction, float velocity)
        {
            if (_stress > 0.5f)
                return maxSpeed * _stress * _directionC;
            return direction * velocity;
        }

        private void OnTriggerStay(Collider other)
        {
            var obs = other.GetComponent<Obstacle>();
            if (obs == null)
                return;

            var pos = transform.position + _controller.velocity * Time.fixedDeltaTime * _controller.direction;
            var posO = other.ClosestPoint(pos);
            var swapped = false;
            if (posO == pos) posO = other.bounds.center;

            var dist = pos - posO;

            var distM = dist.magnitude;

            var stress = obs.danger / distM * Time.fixedDeltaTime;
            _stress += stress;
            var bonus = 1.0 - Vector3.Angle(dist, _controller.direction) / 180;
            if (distM * stress * bonus > _dist)
            {
                _direction = dist.normalized;
                _dist = distM * stress;
            }
        }
    }
}