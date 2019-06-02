using UnityEngine;

public class FoodValue : MonoBehaviour
{
    private float AttractValue = 1.0f;
    public float MaxAttractValue = 1.0f;

    private void Start()
    {
        AttractValue = MaxAttractValue;
    }

    public float GetCurrentAttractiveness()
    {
        return AttractValue;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Fish")) return;

        AttractValue -= 0.05f * Time.fixedDeltaTime;
        if (AttractValue < 0)
            AttractValue = 0;
        else
            transform.localScale = new Vector3(1, AttractValue / MaxAttractValue, 1);
    }
}