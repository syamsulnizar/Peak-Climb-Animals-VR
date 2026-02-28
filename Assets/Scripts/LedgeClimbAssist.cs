using UnityEngine;

public class LedgeClimbAssist : MonoBehaviour
{
    public Transform playerRig;        // XR Origin
    public Transform head;             // Camera
    public Transform ledgeTopPoint;    // Empty object di atas tebing

    public float heightThreshold = 0.2f;
    public float snapForwardOffset = 0.3f;
    public float snapUpOffset = 0.1f;

    public CharacterController controller; // atau Rigidbody

    public bool isGrabbing;

    void Update()
    {
        if (!isGrabbing) return;

        float headHeight = head.position.y;
        float ledgeHeight = ledgeTopPoint.position.y;

        if (headHeight > ledgeHeight - heightThreshold)
        {
            SnapToTop();
        }
    }

    void SnapToTop()
    {
        Vector3 targetPosition =
            ledgeTopPoint.position +
            ledgeTopPoint.forward * snapForwardOffset +
            Vector3.up * snapUpOffset;

        controller.enabled = false;
        playerRig.position = targetPosition;
        controller.enabled = true;

        isGrabbing = false;
    }

    public void OnGrabLedge()
    {
        isGrabbing = true;
    }

    public void OnReleaseLedge()
    {
        isGrabbing = false;
    }
}