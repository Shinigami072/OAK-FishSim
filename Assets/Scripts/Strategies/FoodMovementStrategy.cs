using UnityEngine;

namespace DefaultNamespace
{
    public class FoodMovementStrategy : FishMovementStrategy
    {
        private FishAgentController _controller;
        public Vector3 _direction;
        private Vector3 _directionC;
        private float _dist, _distC;
        private float _interest;
        public float maxSpeed = 0.01f;


        private void Start()
        {
            _dist = float.NegativeInfinity;
            _controller = GetComponent<FishAgentController>();
        }

        private void FixedUpdate()
        {
            if (_interest <= 0) _interest += 0.1f * Time.fixedDeltaTime;

            _directionC = _direction;
            _distC = _dist;
            _dist = float.NegativeInfinity;
        }

        public override Vector3 GetVelocity(Vector3 direction, float velocity)
        {
            if (_distC > 0f && _interest > 0f)
                return maxSpeed * _directionC;
            return direction * velocity;
        }

        private void OnTriggerStay(Collider other)
        {
            var obs = other.GetComponent<FoodValue>();
            if (_interest < 0) return;

            if (obs == null)
                return;

            var pos = transform.position + _controller.velocity * Time.fixedDeltaTime * _controller.direction;
            var posO = other.ClosestPoint(pos);

            var dist = posO - pos;


            var distM = dist.magnitude;

            if (distM < float.Epsilon)
            {
                _interest -= 10.0f * Time.fixedDeltaTime;
                return;
            }

            var food = obs.GetCurrentAttractiveness();

            var bonus = 1.0 - Vector3.Angle(dist, _controller.direction) / 180;
            if (food / distM * bonus > _dist)
            {
                _direction = dist.normalized;
                _dist = food / distM;
            }
        }
    }
}