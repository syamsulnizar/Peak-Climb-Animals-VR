using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class ClimbingProvider : MonoBehaviour
{
    public Transform xrRig;
    public CharacterController cc;
    public XRDirectInteractor leftHand;
    public XRDirectInteractor rightHand;

    public LayerMask climbLayer;

    [Header("Manual Gravity")]
    public float gravity = -9.81f;
    public float maxFallSpeed = -20f;

    private float verticalVelocity = 0f;
    private bool wasClimbingLastFrame = false;

    Vector3 lastLeft;
    Vector3 lastRight;

    private MiddleFingerEvent leftHandEvent;
    private MiddleFingerEvent rightHandEvent;

    void OnEnable()
    {
        StartCoroutine(InitializeAfterDelay());
    }

    public void HardResetClimbState()
    {
        ForceReleaseBoth();
        ForceXRRelease(leftHand);
        ForceXRRelease(rightHand);

        verticalVelocity = 0f;
        lastLeft = leftHand.transform.position;
        lastRight = rightHand.transform.position;
    }

    void ForceXRRelease(XRDirectInteractor interactor)
    {
        if (interactor.selectTarget != null)
        {
            interactor.interactionManager.SelectExit(
                interactor,
                interactor.selectTarget
            );
        }
    }

    IEnumerator InitializeAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        MiddleFingerEvent[] all = FindObjectsOfType<MiddleFingerEvent>();

        foreach (var hand in all)
        {
            if (!hand.isRightHand)
                leftHandEvent = hand;
            else if (hand.isRightHand)
                rightHandEvent = hand;
        }

        Debug.Log("MiddleFingerManager initialized.");
    }

    public void ForceReleaseBoth()
    {
        leftHandEvent?.ForceRelease();
        rightHandEvent?.ForceRelease();
    }

    void Update()
    {
        bool leftGrab = IsGrabbingClimbable(leftHand);
        bool rightGrab = IsGrabbingClimbable(rightHand);
        bool isClimbingNow = leftGrab || rightGrab;

        if (isClimbingNow)
        {
            // Matikan gravity DynamicMoveProvider saat climbing

            // Reset vertical velocity saat climbing
            verticalVelocity = 0f;

            Vector3 delta = Vector3.zero;

            if (leftGrab)
                delta += lastLeft - leftHand.transform.position;

            if (rightGrab)
                delta += lastRight - rightHand.transform.position;

            if (leftGrab && rightGrab)
                delta *= 0.5f;

            cc.Move(delta);
        }
        else
        {
            // Saat baru lepas dari climbing, mulai dari 0
            if (wasClimbingLastFrame)
                verticalVelocity = 0f;

            // Apply gravity manual
            if (cc.isGrounded)
            {
                verticalVelocity = -2f; // tetap menempel di ground
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
                verticalVelocity = Mathf.Max(verticalVelocity, maxFallSpeed);
            }

            cc.Move(Vector3.up * verticalVelocity * Time.deltaTime);
        }

        wasClimbingLastFrame = isClimbingNow;
        lastLeft = leftHand.transform.position;
        lastRight = rightHand.transform.position;
    }

    bool IsGrabbingClimbable(XRDirectInteractor hand)
    {
        if (hand.selectTarget == null)
            return false;

        GameObject target = hand.selectTarget.gameObject;
        return ((1 << target.layer) & climbLayer) != 0;
    }
}
