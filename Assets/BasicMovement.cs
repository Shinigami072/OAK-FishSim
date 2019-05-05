using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 _movemnt =new Vector3();
    public FishAgent agent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _movemnt.x = Input.GetAxisRaw("Horizontal");
        _movemnt.z = Input.GetAxisRaw("Vertical");
        _movemnt.y = Input.GetAxisRaw("UPDown");
        if(_movemnt.sqrMagnitude>0.01)
            if(agent != null)
                agent.motion+= Time.deltaTime * (_movemnt.normalized*
                                             Mathf.Clamp(agent.motion.magnitude,0.01f,0.5f)- agent.motion);
    }
}
