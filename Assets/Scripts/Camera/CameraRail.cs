using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRail : MonoBehaviour
{
    private Vector3[] nodes;

    void Start()
    {
        nodes = new Vector3[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            nodes[i] = transform.GetChild(i).position;
        }
    }

    public Vector3 ProjectPosition(Vector3 pos)
    {
        int closestNodeNum = GetClosestNode(pos);

        if(closestNodeNum == 0)
        {
            return ProjectOnSegment(nodes[0], nodes[1], pos);
        }
        else if (closestNodeNum == nodes.Length - 1)
        {
            return ProjectOnSegment(nodes[nodes.Length - 1], nodes[nodes.Length - 2], pos);
        }
        else
        {
            Vector3 leftSegment = ProjectOnSegment(nodes[closestNodeNum - 1], nodes[closestNodeNum], pos);
            Vector3 rightSegment = ProjectOnSegment(nodes[closestNodeNum + 1], nodes[closestNodeNum], pos);

            if((pos - leftSegment).sqrMagnitude <= (pos - rightSegment).sqrMagnitude)
            {
                return leftSegment;
            }
            else
            {
                return rightSegment;
            }
        }
    }
    
    private int GetClosestNode(Vector3 pos)
    {
        int closestNodeNum = -1;
        float minDistance = 0f;

        for(int i = 0; i < nodes.Length; i++)
        {
            float distance = (nodes[i] - pos).sqrMagnitude;

            if(minDistance == 0f || distance < minDistance)
            {
                minDistance = distance;
                closestNodeNum = i;
            }
        }

        return closestNodeNum;
    }

    private Vector3 ProjectOnSegment(Vector3 vector1, Vector3 vector2, Vector3 pos)
    {
        Vector3 vector1ToPos = pos - vector1;
        Vector3 direction = (vector2 - vector1).normalized;

        float distanceFromVector1 = Vector3.Dot(direction, vector1ToPos);

        if (distanceFromVector1 < 0f)
        {
            return vector1;
        }
        else if (distanceFromVector1 * distanceFromVector1 > (vector2 - vector1).sqrMagnitude)
        {
            return vector2;
        }
        else
        {
            return direction * distanceFromVector1 + vector1;
        }
    }
}
