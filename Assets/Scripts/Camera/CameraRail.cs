﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraRail : MonoBehaviour
{
    private Transform[] roadPoints;
    private CameraNode[] nodes;

    private int closestRoadPointNum;
    private int closestConnectedRoadPointNum;

    private void Start()
    {
        nodes = FindObjectsOfType<CameraNode>();
        roadPoints = new Transform[nodes.Length];

        for (int i = 0; i < nodes.Length; i++)
        {
            roadPoints[i] = nodes[i].transform.parent;
        }
    }

    private void Update()
    {
        /*foreach(CameraNode node in nodes)
        {
            foreach (CameraNode connection in node.connectedNodes)
            {
                Debug.DrawLine(node.transform.position, connection.transform.position, Color.red);
            }
        }*/
        
        /*if(nodes.Length > 1)
        {
            for (int i = 0; i < nodes.Length - 1; i++)
            {
                Debug.DrawLine(nodes[i].position, nodes[i + 1].position, Color.red);
            }
        }*/
    }

    public Vector3 ProjectPosition(Vector3 pos)
    {
        closestRoadPointNum = GetClosestRoadPoint(pos);
        closestConnectedRoadPointNum = GetSecondClosestPoint(pos);

        Vector3 posOnSegment = ProjectPositionOnRoadSegment(roadPoints[closestRoadPointNum].position, roadPoints[closestConnectedRoadPointNum].position, pos);

        return ProjectPositionOnRailSegment(closestRoadPointNum, closestConnectedRoadPointNum, posOnSegment);

        /*if(closestRoadPointNum == 0)
        {
            Vector3 posOnSegment = ProjectPositionOnRoadSegment(roadPoints[0].position, roadPoints[1].position, pos);
            return ProjectPositionOnRailSegment(0, 1, posOnSegment);
        }
        else if (closestRoadPointNum == roadPoints.Length - 1)
        {
            Vector3 posOnSegment = ProjectPositionOnRoadSegment(roadPoints[roadPoints.Length - 1].position, roadPoints[roadPoints.Length - 2].position, pos);
            return ProjectPositionOnRailSegment(roadPoints.Length - 1, roadPoints.Length - 2, posOnSegment);
        }
        else
        {
            Vector3 leftSegment = ProjectPositionOnRoadSegment(roadPoints[closestRoadPointNum - 1].position, roadPoints[closestRoadPointNum].position, pos);
            Vector3 rightSegment = ProjectPositionOnRoadSegment(roadPoints[closestRoadPointNum + 1].position, roadPoints[closestRoadPointNum].position, pos);

            if((pos - leftSegment).sqrMagnitude <= (pos - rightSegment).sqrMagnitude)
            {
                return ProjectPositionOnRailSegment(closestRoadPointNum - 1, closestRoadPointNum, leftSegment);
            }
            else
            {
                return ProjectPositionOnRailSegment(closestRoadPointNum + 1, closestRoadPointNum, rightSegment);
            }
        }*/
    }

    private int GetClosestRoadPoint(Vector3 pos)
    {
        int closestRoadPointNum = 0;
        float minDistance = 0f;

        for(int i = 0; i < roadPoints.Length; i++)
        {
            float distance = (roadPoints[i].position - pos).sqrMagnitude;

            if(minDistance == 0f || distance < minDistance)
            {
                minDistance = distance;
                closestRoadPointNum = i;
            }
        }

        return closestRoadPointNum;
    }

    private int GetSecondClosestPoint(Vector3 pos)
    {
        int closestConnectedRoadPointNum = 0;
        float minDistance = 0f;

        for (int i = 0; i < roadPoints.Length; i++)
        {
            float distance = (roadPoints[i].position - pos).sqrMagnitude;

            if (i != closestRoadPointNum && (minDistance == 0f || distance < minDistance))
            {
                if (nodes[closestRoadPointNum].connectedNodes.Contains(nodes[i]))
                {
                    minDistance = distance;
                    closestConnectedRoadPointNum = i;
                }

            }
        }

        return closestConnectedRoadPointNum;
    }

    private Vector3 ProjectPositionOnRoadSegment(Vector3 vector1, Vector3 vector2, Vector3 pos)
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

    private Vector3 ProjectPositionOnRailSegment(int idx1, int idx2, Vector3 pos)
    {
        float posPercent = Vector3.Distance(roadPoints[idx1].position, pos) / Vector3.Distance(roadPoints[idx1].position, roadPoints[idx2].position);

        Vector3 posDif = nodes[idx2].transform.position - nodes[idx1].transform.position;

        return nodes[idx1].transform.position + posDif * posPercent;

    }

    public Vector3 ProjectRotation(Vector3 pos, Vector3 camPos)
    {
        int closestRoadPointNum = GetClosestRoadPoint(pos);

        return ProjectRotationOnSegment(nodes[closestRoadPointNum].transform, nodes[closestConnectedRoadPointNum].transform, camPos);

        /*if (closestRoadPointNum == 0)
        {
            return ProjectRotationOnSegment(nodes[0].transform, nodes[1].transform, camPos);
        }
        else if (closestRoadPointNum == roadPoints.Length - 1)
        {
            return ProjectRotationOnSegment(nodes[nodes.Length - 1].transform, nodes[nodes.Length - 2].transform, camPos);
        }
        else
        {
            Vector3 leftSegment = ProjectPositionOnRoadSegment(roadPoints[closestRoadPointNum - 1].position, roadPoints[closestRoadPointNum].position, pos);
            Vector3 rightSegment = ProjectPositionOnRoadSegment(roadPoints[closestRoadPointNum + 1].position, roadPoints[closestRoadPointNum].position, pos);

            if ((pos - leftSegment).sqrMagnitude <= (pos - rightSegment).sqrMagnitude)
            {
                return ProjectRotationOnSegment(nodes[closestRoadPointNum - 1].transform, nodes[closestRoadPointNum].transform, camPos); ;
            }
            else
            {
                return ProjectRotationOnSegment(nodes[closestRoadPointNum + 1].transform, nodes[closestRoadPointNum].transform, camPos); ;
            }
        }*/
    }

    private Vector3 ProjectRotationOnSegment(Transform node1, Transform node2, Vector3 pos)
    {
        float posPercent = Vector3.Distance(node1.position, pos) / Vector3.Distance(node1.position, node2.position);

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

        return node1.eulerAngles + rotDif * posPercent;
    }
}
