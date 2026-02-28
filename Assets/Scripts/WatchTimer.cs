using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WatchTimer : MonoBehaviour
{
    public TextMeshProUGUI watchTimer;
    public ClimbingTimer climbingTimer;

    private void Start()
    {
        climbingTimer = FindObjectOfType<ClimbingTimer>();
    }

    private void Update()
    {
        if (climbingTimer != null)
            watchTimer.text = Mathf.FloorToInt(climbingTimer.CurrentTime).ToString();
    }
}
