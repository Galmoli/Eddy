using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawRailPaths : MonoBehaviour
{
    private NodeConnections connections;

    private void Awake()
    {
        connections = FindObjectOfType<NodeConnections>();
    }

    private void Update()
    {
        foreach (NodeConnections.Connection c in connections.connections)
        {
            Debug.DrawLine(c.node1.transform.position, c.node2.transform.position, Color.red);
        }
    }
}
