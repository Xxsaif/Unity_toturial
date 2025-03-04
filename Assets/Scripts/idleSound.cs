using UnityEngine;

public class idleSound : MonoBehaviour
{
    public AudioClip[] idleSounds;
    private AudioSource audioSource;
    private float idleTimer;
    private bool isIdle;

    public AudioClip runningSound; 
    private bool isWalking;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
    



        if (Input.anyKey)
        {
            idleTimer = 0;
            isIdle = false;

            audioSource.Stop();
        }
        else
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= 5f && !isIdle)
            {
                isIdle = true;
                PlayIdleSound();
            }

        }

        void PlayIdleSound()
        {
            if (idleSounds.Length > 0)
            {
                audioSource.PlayOneShot(idleSounds[Random.Range(0, idleSounds.Length)]);
            }
        }
    }
}
