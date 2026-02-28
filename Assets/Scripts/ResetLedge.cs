using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLedge : MonoBehaviour
{
    public TriggerLedge[] triggerLedges;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var triggerLedge in triggerLedges)
            {
                triggerLedge.isLedgeReached = false;
            }
        }
    }
}
