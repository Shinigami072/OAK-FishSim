using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor;
using UnityEngine;

public class FishScript2 : MonoBehaviour
{
    public float attractionRange, alignmentRange, repulsionRange;
    public LinkedList<GameObject> FishList;
    public LinkedList<GameObject> Obstacles;
    public LinkedList<GameObject> DangerousObstacles;
    public float maxVelocity = 100;
    public float minVelocity = 0;
    private Vector3 targetPos;


    enum State
    {
        Attract,
        Align,
        Repulse,
        None
    };

    private Vector3 heading;
    private Vector3 sumHeading;
    private float velocity;
    private float sumVelocity;
    private int fishCount;
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        FishList = new LinkedList<GameObject>();
        heading = Vector3.forward;
        velocity = 0;
        sumHeading = Vector3.zero;
        fishCount = 0;
        state = State.None;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        sumHeading = Vector3.zero;
        if (FishList.Count > 0)
        {
            targetPos = Vector3.zero;
            foreach (var Fish in FishList)
            {
                var position = Fish.transform.position;
                var d = Vector3.Distance(transform.position, position);
                targetPos += position;

                //if (d < attractionRange && d > repulsionRange)
                //    sumHeading += (Fish.transform.position - transform.position).normalized;
                if (d < repulsionRange)
                    sumHeading -= (Fish.transform.position - transform.position).normalized;
            }

        
        }
        targetPos += transform.position;
        targetPos /= FishList.Count+1;

        state = State.None;
        if(sumHeading.sqrMagnitude>0)
            heading = (sumHeading.normalized);
        else
        {
            heading=(targetPos-transform.position).normalized;
        }
        sumHeading = heading;
        fishCount = 0;
        FishList.Clear();
        var dist = Vector3.Distance(targetPos, transform.position);
        if (dist < attractionRange && dist > alignmentRange)
            velocity = 0.001f * dist / attractionRange;
        else
            velocity = 0;
        if (dist < repulsionRange)
            velocity = -1;


        transform.position += heading * velocity * Time.fixedTime;
    }

    private void OnDrawGizmos()

    {
        var position = transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(position, attractionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(position, alignmentRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, repulsionRange);

        Debug.DrawLine(position, position + heading * 1000, Color.cyan);
        Debug.DrawLine(position, position + heading * velocity, Color.yellow);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetPos, 0.5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("FishAgent"))
            FishList.AddLast(other.gameObject);
    }
}