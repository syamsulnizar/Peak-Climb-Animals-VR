using UnityEngine;

public class FinishController : MonoBehaviour
{
    public enum TriggerType
    {
        Start,
        Stop,
        Finish
    }

    public TriggerType triggerType;
    public ClimbingTimer timer;

    public void Finish()
    {
        switch (triggerType)
        {
            case TriggerType.Start:
                timer.StartTimer();
                break;
            case TriggerType.Stop:
                timer.StopTimer();
                break;
            case TriggerType.Finish:
                timer.FinishTimer();
                break;
        }
    }
}