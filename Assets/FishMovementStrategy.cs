using UnityEngine;

namespace DefaultNamespace
{
    public abstract class FishMovementStrategy : MonoBehaviour
    {
        public abstract Vector3 GetDirection(Vector3 direction,float velocity);
        public float weight;
        
    }
}