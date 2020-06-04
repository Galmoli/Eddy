using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawRailPaths : MonoBehaviour
{
    public bool showCameraPath;
    public bool showRoadPointsPath;

    private NodeConnections connections;

    private void Awake()
    {
        connections = FindObjectOfType<NodeConnections>();
    }

    private void Update()
    {
        if (showCameraPath || showRoadPointsPath)
        {
            foreach (NodeConnections.Connection c in connections.connections)
            {
                if(showCameraPath) Debug.DrawLine(c.node1.transform.position, c.node2.transform.position, Color.red);

                if(showRoadPointsPath) Debug.DrawLine(c.node1.transform.parent.position, c.node2.transform.parent.position, Color.green);
            }
        }
    }
}
