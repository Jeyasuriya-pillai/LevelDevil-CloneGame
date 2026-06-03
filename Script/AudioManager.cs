using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Static instance allows any script to call AudioManager.instance directly
    public static AudioManager instance;

    [Header("Audio Sources")]
    [Tooltip("Assign an AudioSource dedicated to instant sounds like Jump, Death, and Reg.")]
    [SerializeField] private AudioSource sfxSource;   
    
    [Tooltip("Assign a second AudioSource dedicated ONLY to the walking/running loop.")]
    [SerializeField] private AudioSource loopSource;  

    [Header("Audio Clips")]
    public AudioClip run;   // Changed from run1 to run
    public AudioClip jump;  // Changed from jump2 to jump
    public AudioClip death; // Play when player blasts
    public AudioClip reg;   // Play when new player spawns

    void Awake()
    {
        // Singleton pattern configuration
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keeps audio playing across scene reloads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this function to play any one-shot sound effect instantly
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Call this function to loop a sound (like running) using the dedicated loop source
    public void PlayLoopingSFX(AudioClip clip)
    {
        if (loopSource == null || clip == null) return;
        
        // Guard Clause: If it's already playing this exact clip, do nothing (prevents stuttering/lag)
        if (loopSource.clip == clip && loopSource.isPlaying) return; 

        loopSource.clip = clip;
        loopSource.loop = true;
        loopSource.Play();
    }

    // Call this to stop looping sounds instantly
    public void StopLoopingSFX()
    {
        // FIXED: Removed .clip = null to prevent scene reload reference losing bug
        if (loopSource != null && loopSource.isPlaying)
        {
            loopSource.Stop();
        }
    }
}