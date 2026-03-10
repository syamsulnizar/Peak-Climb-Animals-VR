using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class WandShoot : MonoBehaviour
{
    public Transform shootPoint;

    public float fireRate = 0.35f; // waktu antar tembakan

    float nextFireTime = 0f;

    InputDevice hand;
    bool lastState = false;

    bool isGrabbed = false;

    bool isStarted = false;
    public UnityEvent onStart;

    void Start()
    {
        InitializeDevice();
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
        hand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        if (!hand.isValid)
        {
            InitializeDevice();
            return;
        }

        bool pressed;

        if (hand.TryGetFeatureValue(CommonUsages.primaryButton, out pressed))
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