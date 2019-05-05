using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;


public class GizmoTest : MonoBehaviour
{
    public Vector3 targetPosition { get { return m_TargetPosition; } set { m_TargetPosition = value; } }
    [SerializeField]
    private Vector3 m_TargetPosition = new Vector3(1f, 0f, 2f);    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        transform.LookAt(targetPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,Vector3.one);
        Gizmos.DrawLine(transform.position,transform.position+targetPosition);
    }

}
