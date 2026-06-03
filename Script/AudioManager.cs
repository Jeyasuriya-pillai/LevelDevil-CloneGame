using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [Tooltip("Assign an AudioSource dedicated to instant sounds like Jump, Death, and Reg.")]
    [SerializeField] private AudioSource sfxSource;   
    
    [Tooltip("Assign a second AudioSource dedicated ONLY to the walking/running loop.")]
    [SerializeField] private AudioSource loopSource;  

    [Header("Audio Clips")]
    public AudioClip run;   
    public AudioClip jump;  
    public AudioClip death; 
    public AudioClip reg;   

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayLoopingSFX(AudioClip clip)
    {
        if (loopSource == null || clip == null) return;
        
        if (loopSource.clip == clip && loopSource.isPlaying) return; 

        loopSource.clip = clip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void StopLoopingSFX()
    {
        if (loopSource != null && loopSource.isPlaying)
        {
            loopSource.Stop();
        }
    }
}