using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkFollow : MonoBehaviour
{
    private float mSpeed = 10.0f;

    private const float EPSILON = 0.1f;

    private SharkCloseObjects sn;
    private Transform fishTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        sn = GetComponent<SharkCloseObjects>();
        StartCoroutine(DelayedMovement());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fishTarget)
        {
            if ((transform.position - fishTarget.position).magnitude > EPSILON)
                transform.Translate(0.0f, 0.0f, mSpeed * Time.deltaTime);
        }else
            transform.Translate(transform.forward*Time.fixedTime);
    }
    
    Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }

        return tMin;
    }

    IEnumerator DelayedMovement()
    {
        while (true)
        {
            Transform aTarget = GetClosestEnemy(sn.GetObjects());
            if (aTarget)
            {
                fishTarget = aTarget;
                transform.LookAt(fishTarget.position);

            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}
