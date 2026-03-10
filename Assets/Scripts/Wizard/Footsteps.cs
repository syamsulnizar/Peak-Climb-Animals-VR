using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource footstepAudio;
    public AudioClip[] footstepClips;

    public float moveThreshold = 0.01f;
    public float stepInterval = 0.4f;

    Vector3 lastPosition;
    float stepTimer;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        stepTimer -= Time.deltaTime;

        if (distanceMoved > moveThreshold && stepTimer <= 0f)
        {
            PlayFootstep();
            stepTimer = stepInterval;
        }

        lastPosition = transform.position;
    }

    void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        int index = Random.Range(0, footstepClips.Length);

        footstepAudio.pitch = Random.Range(0.9f, 1.1f);

        footstepAudio.PlayOneShot(footstepClips[index]);
    }
}