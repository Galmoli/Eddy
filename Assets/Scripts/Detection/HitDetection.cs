using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [TagSelector] public string[] TagsToCollision = new string[] { };
    [HideInInspector] public GameObject hitObject;

    private BoxCollider _trigger;
    public Action OnHit = delegate { };

    private PlayerCombatController _controller;

    private void Awake()
    {
        _trigger = GetComponent<BoxCollider>();

        _controller = transform.root.GetComponent<PlayerCombatController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TagsToCollision.Contains(other.tag))
        {
            hitObject = other.gameObject;
            OnHit();
        }
    }

    public void EnableTrigger()
    {
        _trigger.enabled = true;

        if (AudioManager.Instance.ValidEvent(_controller.playerSounds.attackSoundPath))
        {
            AudioManager.Instance.PlayOneShotSound(_controller.playerSounds.attackSoundPath, _controller.transform);
        }
    }

    public void DisableTrigger()
    {
        _trigger.enabled = false;
    }
}
