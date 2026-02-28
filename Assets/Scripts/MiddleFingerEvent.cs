using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class MiddleFingerEvent : MonoBehaviour
{
    InputDevice hand;

    public bool isRightHand = true;
    [SerializeField] private UnityEvent onMiddleFingerPressed;
    [SerializeField] private UnityEvent onMiddleFingerReleased;

    private bool lastGripState = false;

    // 🔒 Input Lock System
    private bool inputLocked = false;

    void Start()
    {
        InitializeDevice();
    }

    void InitializeDevice()
    {
        if (isRightHand)
            hand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        else
            hand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    }

    void Update()
    {
        if (!hand.isValid)
        {
            InitializeDevice();
            return;
        }

        if (hand.TryGetFeatureValue(CommonUsages.gripButton, out bool gripPressed))
        {
            // 🔒 Jika sedang dikunci
            if (inputLocked)
            {
                // Tunggu sampai user benar-benar melepas grip
                if (!gripPressed)
                {
                    inputLocked = false;
                    lastGripState = false; // reset state
                }

                return;
            }

            // Normal behavior
            if (gripPressed && !lastGripState)
            {
                onMiddleFingerPressed?.Invoke();
            }

            if (!gripPressed && lastGripState)
            {
                onMiddleFingerReleased?.Invoke();
            }

            lastGripState = gripPressed;
        }
    }

    // ✅ Ini dipanggil saat ingin force release
    public void ForceRelease()
    {
        if (lastGripState)
        {
            onMiddleFingerReleased?.Invoke();
        }

        inputLocked = true;
        lastGripState = false;
    }
}