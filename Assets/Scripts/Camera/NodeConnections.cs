using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnections : MonoBehaviour
{
    [System.Serializable]
    public struct Connection
    {
        public CameraNode node1;
        public CameraNode node2;
    }

    public Connection[] connections;

    void Start()
    {
        foreach (Connection c in connections)
        {
            if (c.node1 != null && c.node2 != null && c.node1 != c.node2)
            {
                if (!c.node1.connectedNodes.Contains(c.node2))
                {
                    c.node1.connectedNodes.Add(c.node2);
                }

                if (!c.node2.connectedNodes.Contains(c.node1))
                {
                    c.node2.connectedNodes.Add(c.node1);
                }
            }
        }
    }
}
