using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class SpawnerScript : MonoBehaviour
{
    public int count;
    public GameObject obj;

    private List<GameObject> _objects;
    // Start is called before the first frame update
    void Start()
    {
        _objects=new List<GameObject>(count);
        var scale = transform.lossyScale / 2;
        for (int i = 0; i < count; i++)
        {
            var pos = Random.insideUnitSphere;
            pos.Scale(scale);
            var objectInstance = Instantiate(obj);
            objectInstance.transform.position = transform.position + pos;
            _objects.Add(objectInstance);
            
        }
    }


    private void OnDrawGizmos()
    {
        var transform1 = transform;
        Gizmos.DrawWireCube(transform1.position, transform1.lossyScale);
    }

    private void OnApplicationQuit()
    {
        foreach (var objector in _objects)
        {
            Destroy(objector);
        }
    }
}
