using UnityEngine;
using UnityEngine.InputSystem;

public class MiddleFingerInputGate : MonoBehaviour
{
    public InputActionReference middleFingerAction;

    private bool inputLocked;
    private bool isPressed;

    void OnEnable()
    {
        middleFingerAction.action.Enable();
    }

    void Update()
    {
        float value = middleFingerAction.action.ReadValue<float>();
        bool currentlyPressed = value > 0.8f;

        if (inputLocked)
        {
            // Tunggu sampai user benar-benar melepas tombol
            if (!currentlyPressed)
            {
                inputLocked = false;
            }

            isPressed = false;
            return;
        }

        isPressed = currentlyPressed;
    }

    public void ForceRelease()
    {
        inputLocked = true;
        isPressed = false;
    }

    public bool IsPressed()
    {
        return isPressed;
    }
}