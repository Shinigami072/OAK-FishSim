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
    void Awake()
    {
       StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        _objects=new List<GameObject>(count);
        var transformCache = transform;
        var baseRot = transformCache.rotation;
        var basePos = transformCache.position;
        var baseScale = transformCache.lossyScale / 2;
        for (int i = 0; i < count; i++)
        {
            var pos = Random.insideUnitSphere;
            pos.Scale(baseScale);
            
            var objectInstance = Instantiate(obj,basePos + pos,baseRot);
            _objects.Add(objectInstance);
            yield return new WaitForSeconds(1.0f/count);
            
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
