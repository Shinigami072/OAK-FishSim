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

        
        public Vector3 direction;
        public float veloctity;


       
        private void Start()
        {
            List<Component> strategiesComponents = new List<Component>();
            GetComponents(typeof(FishMovementStrategy),strategiesComponents);
            _strategies = new List<FishMovementStrategy>(strategiesComponents.Count);
            
            foreach (var strategy in strategiesComponents)
            {
                _strategies.Add(strategy as FishMovementStrategy);
            }

            direction = Random.onUnitSphere;
            veloctity = 0.0f;

        }

        private void FixedUpdate()
        {
            _directionSum = Vector3.zero;
            _weightSum = 0f;
           
            foreach (var strategy in _strategies)
            {
                _directionSum += strategy.GetDirection(direction, veloctity) * strategy.weight;
                _weightSum += strategy.weight;
            }
            var wantedDirection = _directionSum.normalized;
            
            //lerp - funkcja przekształcająca wektor A do B, zgodnei ze skalą c,
            //jeśli c = 0 zwraca A
            //jeśli c = 1 zwraca B
            //użwyamy delty żeby zmiana była płynniejsza
            direction = Vector3.Lerp(direction,wantedDirection,Time.fixedTime);

            veloctity *= 0.9f;
            veloctity = Mathf.Lerp(veloctity, _directionSum.magnitude / _weightSum, Time.fixedTime);
            //_directionSum.magnitude / _weightSum;

        }
    }
}