using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkCloseObjects : MonoBehaviour
{
    private List<Transform> objects = new List<Transform>();

    public List<Transform> GetObjects()
    {
        return objects;}

    private void OnTriggerEnter(Collider other)
    {
        if (!objects.Contains(other.gameObject.transform) && other.gameObject.CompareTag("Fish"))
        {
            objects.Add(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objects.Remove(other.gameObject.transform);
    }
}