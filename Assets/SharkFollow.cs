using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkFollow : MonoBehaviour
{
    public Transform aTarget;

    private float mSpeed = 10.0f;

    private const float EPSILON = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        transform.LookAt(aTarget.position);
            if((transform.position - aTarget.position).magnitude > EPSILON)
                transform.Translate(0.0f,0.0f, mSpeed * Time.deltaTime);

    }
}
