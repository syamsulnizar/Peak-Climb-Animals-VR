using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class WandShoot : MonoBehaviour
{
    public Transform shootPoint;

    public float fireRate = 0.35f; // waktu antar tembakan

    float nextFireTime = 0f;

    InputDevice handRight;
    InputDevice handLeft;
    bool lastState = false;

    bool isGrabbed = false;

    bool isRight = false;

    bool isStarted = false;
    public UnityEvent onStart;

    void Start()
    {
        InitializeDevice();
    }

    public void SetRightHand(bool isRightHand)
    {
        isRight = isRightHand;
    }

    public void SetGrabbed(bool grabbed)
    {
        isGrabbed = grabbed;
    }

    public void StartMusic()
    {
        if (isStarted) return;

        isStarted = true;
        onStart?.Invoke();
    }

    void InitializeDevice()
    {
        handRight = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        handLeft = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    }

    void Update()
    {
        if (!handRight.isValid)
        {
            InitializeDevice();
            return;
        }

        bool pressed;

        if (handRight.TryGetFeatureValue(CommonUsages.triggerButton, out pressed) && isRight)
        {
            if (pressed && !lastState && isGrabbed)
            {
                TryShoot();
            }

            lastState = pressed;
        }

        if (handLeft.TryGetFeatureValue(CommonUsages.triggerButton, out pressed) && !isRight)
        {
            if (pressed && !lastState && isGrabbed)
            {
                TryShoot();
            }

            lastState = pressed;
        }
    }

    void TryShoot()
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireRate;

        Shoot();
    }

    void Shoot()
    {
        GameObject projectile = ProjectilePool.instance.GetProjectile();

        projectile.transform.position = shootPoint.position;
        projectile.transform.rotation = shootPoint.rotation;

        projectile.SetActive(true);
    }
}