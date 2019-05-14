using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace DefaultNamespace
{
    public class FishAgentController : MonoBehaviour
    {
        private List<FishMovementStrategy> _strategies;
        private Vector3 _directionSum;
        private float _weightSum;
        private Vector3 _velocitySum;


        public Vector3 direction = Vector3.forward;
        public float veloctity = 0.0f;
        private Rigidbody _rigidbody;


        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            List<Component> strategiesComponents = new List<Component>();
            GetComponents(typeof(FishMovementStrategy), strategiesComponents);
            _strategies = new List<FishMovementStrategy>(strategiesComponents.Count);

            foreach (var strategy in strategiesComponents)
            {
                _strategies.Add(strategy as FishMovementStrategy);
            }

            direction = Random.onUnitSphere;
            veloctity = Random.Range(0.1f, 0.5f);
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        private void OnDrawGizmos()

        {
            var position = transform.position;
//            Debug.DrawLine(position + Vector3.up, position + Vector3.up + direction * veloctity * 1000, Color.yellow);
            Debug.DrawLine(position, position + direction * 500, Color.magenta);
        }

        private void FixedUpdate()
        {
            _directionSum = Vector3.zero;
            _velocitySum = Vector3.zero;
            _weightSum = 0f;

            foreach (var strategy in _strategies)
            {
//                _directionSum += strategy.GetDirection(direction, veloctity) * strategy.weight;
                _velocitySum += strategy.GetVelocity(direction, veloctity) * strategy.weight;
                _weightSum += strategy.weight;
            }
//
            var wantedDirection = _velocitySum/_weightSum;
//
//            var diff = (wantedDirection.normalized - direction) * Time.fixedTime;
//            //lerp - funkcja przekształcająca wektor A do B, zgodnei ze skalą c,
//            //jeśli c = 0 zwraca A
//            //jeśli c = 1 zwraca B
//            //użwyamy delty żeby zmiana była płynniejsza
//
//            direction += diff* Time.fixedTime;
//            direction.Normalize();

//            veloctity = Mathf.Lerp(veloctity, wantedDirection.magnitude, Time.fixedTime / 15.0f);
//            veloctity *= 0.9f;
            //_directionSum.magnitude / _weightSum;
            direction = wantedDirection.normalized;
            veloctity = wantedDirection.magnitude;
//            _rigidbody.position+= direction*veloctity*Time.fixedTime;
            transform.position += wantedDirection*Time.fixedTime;
        }
    }
}