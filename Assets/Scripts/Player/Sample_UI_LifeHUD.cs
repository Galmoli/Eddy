using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_UI_LifeHUD : MonoBehaviour                      //THIS SCRIPT IS A SAMPLE. PUT IT ON THE CAMERA AND DRAG THE VARIABLES TO TEST IT.
{
    [SerializeField] private RectTransform _lifePointsPanel;        //_lifePointsPanel is the LifeHUD_Panel in the GameCanvasPrefab
    [SerializeField] private Transform playerPos;                   //playerPos is the player's position + 1.25 meters more in the Y axis. This transform could be a children to the player gameobject.
    

    // Update is called once per frame
    void Update()
    {
        _lifePointsPanel.transform.position = Camera.main.WorldToScreenPoint(playerPos.position);
    }
}
