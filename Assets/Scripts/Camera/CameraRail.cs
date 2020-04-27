using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRail : MonoBehaviour
{
    private Transform[] nodes;

    void Start()
    {
        nodes = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            nodes[i] = transform.GetChild(i);
        }
    }

    public Vector3 ProjectPosition(Vector3 pos)
    {
        int closestNodeNum = GetClosestNode(pos);

        if(closestNodeNum == 0)
        {
            return ProjectPositionOnSegment(nodes[0].position, nodes[1].position, pos);
        }
        else if (closestNodeNum == nodes.Length - 1)
        {
            return ProjectPositionOnSegment(nodes[nodes.Length - 1].position, nodes[nodes.Length - 2].position, pos);
        }
        else
        {
            Vector3 leftSegment = ProjectPositionOnSegment(nodes[closestNodeNum - 1].position, nodes[closestNodeNum].position, pos);
            Vector3 rightSegment = ProjectPositionOnSegment(nodes[closestNodeNum + 1].position, nodes[closestNodeNum].position, pos);

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
            float distance = (nodes[i].position - pos).sqrMagnitude;

            if(minDistance == 0f || distance < minDistance)
            {
                minDistance = distance;
                closestNodeNum = i;
            }
        }

        return closestNodeNum;
    }

    private Vector3 ProjectPositionOnSegment(Vector3 vector1, Vector3 vector2, Vector3 pos)
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

    public Vector3 ProjectRotation(Vector3 pos, Vector3 camPos)
    {
        int closestNodeNum = GetClosestNode(pos);

        if (closestNodeNum == 0)
        {
            return ProjectRotationOnSegment(nodes[0], nodes[1], camPos);
        }
        else if (closestNodeNum == nodes.Length - 1)
        {
            return ProjectRotationOnSegment(nodes[nodes.Length - 1], nodes[nodes.Length - 2], camPos);
        }
        else
        {
            Vector3 leftSegment = ProjectPositionOnSegment(nodes[closestNodeNum - 1].position, nodes[closestNodeNum].position, pos);
            Vector3 rightSegment = ProjectPositionOnSegment(nodes[closestNodeNum + 1].position, nodes[closestNodeNum].position, pos);

            if ((pos - leftSegment).sqrMagnitude <= (pos - rightSegment).sqrMagnitude)
            {
                return ProjectRotationOnSegment(nodes[closestNodeNum - 1], nodes[closestNodeNum], camPos); ;
            }
            else
            {
                return ProjectRotationOnSegment(nodes[closestNodeNum + 1], nodes[closestNodeNum], camPos); ;
            }
        }
    }

    private Vector3 ProjectRotationOnSegment(Transform node1, Transform node2, Vector3 pos)
    {
        float posDif = Vector3.Distance(node1.position, pos) / Vector3.Distance(node1.position, node2.position);

        float px = node1.eulerAngles.x;
        float py = node1.eulerAngles.y;
        float pz = node1.eulerAngles.z;

        Vector3 pNode1 = node1.eulerAngles;
        Vector3 pNode2 = node2.eulerAngles;

        if (Mathf.Abs(pNode1.x - pNode2.x) > 180)
        {
            if (pNode1.x < pNode2.x)
            {
                pNode1.x += 360;
            }
            else
            {
                pNode2.x += 360;
            }
        }

        if (Mathf.Abs(pNode1.y - pNode2.y) > 180)
        {
            if (pNode1.y < pNode2.y)
            {
                pNode1.y += 360;
            }
            else
            {
                pNode2.y += 360;
            }
        }

        if (Mathf.Abs(pNode1.z - pNode2.z) > 180)
        {
            if (pNode1.z < pNode2.z)
            {
                pNode1.z += 360;
            }
            else
            {
                pNode2.z += 360;
            }
        }

        Vector3 rotDif = pNode2 - pNode1;

        return node1.eulerAngles + rotDif * posDif;
    }
}
