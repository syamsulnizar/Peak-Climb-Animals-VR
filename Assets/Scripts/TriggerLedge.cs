using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerLedge : MonoBehaviour
{
    public UnityEvent onLedgeReach;
    public bool isLedgeReached = false;
    public Transform teleportTarget;
    public Transform playerTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (isLedgeReached)
            return;

        if (other.CompareTag("Player"))
        {
            isLedgeReached = true;
            onLedgeReach.Invoke();
        }
    }

    public void TeleportLocalTransform()
    {
        playerTransform.position = teleportTarget.position;
    }
}
