using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class XButton : MonoBehaviour
{
    public UnityEvent onTriggerX;
    private InputDevice leftHand;

    void Start()
    {
        InitializeDevice();
    }

    void InitializeDevice()
    {
        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    }

    void Update()
    {
        if (!leftHand.isValid)
        {
            InitializeDevice();
            return;
        }

        if (leftHand.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonPressed))
        {
            if (buttonPressed)
            {
                onTriggerX?.Invoke();
            }
        }
    }
}