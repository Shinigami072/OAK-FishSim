using UnityEngine;

namespace DefaultNamespace
{
    public class ManualControl: FishMovementStrategy
    {
//        public override Vector3 GetDirection(Vector3 direction, float velocity)
//        {
//            return new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f).normalized;
//        }


        public override Vector3 GetVelocity(Vector3 direction, float velocity)
        {
            return Vector3.forward*0.01f;
        }
        
    }
}