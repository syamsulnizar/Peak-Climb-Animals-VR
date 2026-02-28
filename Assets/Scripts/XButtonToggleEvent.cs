using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class XButtonToggleEvent : MonoBehaviour
{
    public UnityEvent onFirstToggle;
    public UnityEvent onSecondToggle;
    public UnityEvent onFirstInteractor;
    public UnityEvent onSecondInteractor;

    public bool isOpenPanel = true;

    private InputDevice leftHand;
    private bool lastButtonState = false;
    private bool toggleState = false;

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
            // detect button down
            if (buttonPressed && !lastButtonState)
            {
                HandleToggle();
            }

            lastButtonState = buttonPressed;
        }
    }

    void HandleToggle()
    {
        toggleState = !toggleState;

        if (!toggleState)
        {
            onFirstToggle?.Invoke();
            if(!isOpenPanel)
                onFirstInteractor?.Invoke();
        }
        else
        {
            onSecondToggle?.Invoke();
            if(!isOpenPanel)
                onSecondInteractor?.Invoke();
        }
    }

    public void SetPanelState(bool isOpen)
    {
        isOpenPanel = isOpen;
    }
}