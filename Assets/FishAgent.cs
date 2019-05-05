﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Serialization;

//[ExecuteInEditMode]
public class FishAgent : MonoBehaviour
{
    public SphereCollider fishRadious;

    public float maxRadious;
    [FormerlySerializedAs("AttractionRange")] public float attractionRange;
    [FormerlySerializedAs("RepulsionRange")] public float repulsionRange;
    public int contactCounter, contactCounterDisp;

    public Vector3 motion;

    private Vector3 _summator, _sumDisp;

    private Vector3 _attractionsummator, _atrDisp;
    private GameObject _target = null;
    private float goalAttractiveness = 0;

    // Start is called before the first frame update
    void Start()
    {
        fishRadious = GetComponent<SphereCollider>();
        var noise = Random.onUnitSphere;
        while (noise.y.Equals(0.0f))
        {
            noise = Random.onUnitSphere;
        }

        motion = noise * 0.005f;
        _attractionsummator = Vector3.zero;
        _summator = Vector3.zero;
    }


    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(motion, Vector3.Reflect(motion, Vector3.up));
    }

    private void FixedUpdate()
    {
        _summator += motion;
        contactCounter += 1;
        if (_target != null)
        {
            var diff = (_target.transform.position - transform.position);
            if(diff.magnitude >3.0f)
                _attractionsummator += (diff.normalized-motion.normalized).normalized/3e2f;
        }

        motion *= 0.99999999997f;
        motion += ((_attractionsummator / contactCounter)) * (Time.fixedTime / 300.0f);
        motion += (_summator / contactCounter - motion) * Time.fixedTime / 1000.0f;
        var noise = Random.onUnitSphere;
        noise.y = 0;
        motion += Vector3.Slerp(motion.normalized, noise, Time.fixedTime) * Time.fixedTime * 0.0000001f;
       
        transform.position += motion * Time.fixedTime;
        contactCounterDisp = contactCounter;
        contactCounter = 0;
        _atrDisp = _attractionsummator;
        _attractionsummator = Vector3.zero;
        _sumDisp = _summator;
        _summator = Vector3.zero;
    }

    private void OnDrawGizmos()

    {
        Gizmos.color = Color.grey;
        var position = transform.position;
        Gizmos.DrawWireSphere(position, maxRadious);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(position, attractionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, repulsionRange);
        Debug.DrawLine(position + Vector3.up,
            position + Vector3.up + Vector3.forward * contactCounterDisp, Color.black);
        Debug.DrawLine(position + Vector3.up, position + Vector3.up + motion * 1000, Color.blue);
        Debug.DrawLine(position + Vector3.up, position + Vector3.up + (_sumDisp / contactCounterDisp - motion) * 1000,
            Color.cyan);
        Debug.DrawLine(position + Vector3.up, position + Vector3.up + _atrDisp * 1000, Color.yellow);
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "FishAgent":
            {
                var diff = (other.transform.position - transform.position);
                var dist = diff.magnitude;
                if (dist < attractionRange)
                {
                    contactCounter++;
                    _summator += (other.gameObject).GetComponent<FishAgent>().motion;

                    if (dist < repulsionRange)
                        _attractionsummator += ((diff - diff.normalized * repulsionRange) / (repulsionRange) / 1000);
                    else
                        _attractionsummator += ((diff - diff.normalized * repulsionRange) /
                                               (attractionRange - repulsionRange) / 1000);
                }
            }

                break;

            case "Attractor":
            {
                var diff = (other.transform.position - transform.position);
                var dist = diff.magnitude;
                float attract = other.gameObject.GetComponent<AttractorValue>().AttractValue/dist;
                if (goalAttractiveness < attract)
                {
                    goalAttractiveness = attract;
                    _target = other.gameObject;
                }


                break;
            }
        }
    }
}