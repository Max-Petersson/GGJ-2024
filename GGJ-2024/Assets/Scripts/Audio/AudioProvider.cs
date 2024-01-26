using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AudioPair
{
    public string Key;
    public AudioClip AudioClip;
    public float Volume = 10f;
    public float Duration = 3f;
    public enum SoundType {SemiLaugh, BigLaugh, SlowClap, WilhemScream, Boing, Punch}
    public SoundType Type;  
}
public interface IAudioService
{
    public void PlayAudioClip(string clipName);
    public void StopAudioClip();
    public void PlayRandomClip(AudioPair.SoundType soundType);
    public void InitializeService();
    public void StopService();
}
public class AudioProvider : MonoBehaviour
{


    [SerializeField] AudioPair[] m_pairs;
    [SerializeField] AudioSource m_source;
    private static IAudioService m_audioService;
    public static IAudioService AudioService 
    {
        get { return m_audioService; }
        set { m_audioService ??= new NullAudioService(); }
    }

    [Header("Debugging")]
    public AudioPair.SoundType SoundType;
    public static void SetService(IAudioService service)
    {
        if(service == null)
        {
            AudioService = new NullAudioService();
            return;
        }

        m_audioService = service;
        service.InitializeService();
    }
    private void OnEnable()
    {
        RealAudioService.startTimerForAudio += StartTimerForAudio;
    }
    public void StartTimerForAudio(float duration)
    {
        StartCoroutine(Timer(duration));
    }
    IEnumerator Timer(float duration)
    {
        yield return new WaitForSeconds(duration);
        RealAudioService.IsPlaying = false;
    }
    void Start()
    {
        SetService(new RealAudioService(m_pairs, m_source));
    }
    private void OnDisable()
    {
        AudioService.StopService();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            AudioService.PlayRandomClip(SoundType);
        }
    }
}

public class RealAudioService : IAudioService
{
    [SerializeField] AudioPair[] m_pairs;
    private AudioSource m_source;
    private static bool m_isPlaying = false;
    public static bool IsPlaying { get { return m_isPlaying; } 
        set 
        {
            m_isPlaying = value;
            if(value == false)
            {
                IsPlayingFalse?.Invoke(null, null);
                Debug.Log("Stopped Audio");
            }
        } 
    }
    public static Action<float> startTimerForAudio;

    public static EventHandler IsPlayingFalse;
    public RealAudioService(AudioPair[] pairs , AudioSource source)
    {
        m_pairs = pairs;
        m_source = source;
    }
    public void PlayAudioClip(string clipName)
    {
       
        if (IsPlaying) return;

        IsPlaying = true;
        AudioPair pair = null;

        foreach (AudioPair audioPair in m_pairs) // find the audio with the specifyed key
        {
            if(audioPair.Key == clipName)
            {
                pair = audioPair;
                break;
            }

        }
        Debug.Log(clipName);
        m_source.GetComponent<AudioSource>().PlayOneShot(pair.AudioClip, pair.Volume); // play it with the volume 
        
        startTimerForAudio?.Invoke(pair.Duration); // wait until another can be played
    }
   
    public void PlayRandomClip(AudioPair.SoundType soundType)
    {
       
        List<AudioPair> tempList = new List<AudioPair>(); // create a temporary list

        foreach (AudioPair audioPair in m_pairs)
        {
            if(audioPair.Type == soundType) // if the soundtype matches, add it to the list
                tempList.Add(audioPair);
        }

        int temp = UnityEngine.Random.Range(0, tempList.Count); //find a random in the list 

        PlayAudioClip(tempList[temp].Key);
    }

    private void StopAudio(object sender, EventArgs args)
    {
        StopAudioClip();
    }
    public void StopAudioClip()
    {
        m_source.Stop();
    }

    public void InitializeService() // setup listener for isplayingchanged
    {
        IsPlayingFalse += StopAudio;
    }

    public void StopService() // unsubscribe
    {
        IsPlayingFalse -= StopAudio;
    }
}
public class NullAudioService : IAudioService
{
    public void InitializeService()
    {
        return;
    }

    public void PlayAudioClip(string clipName)
    {
        Debug.LogError("You are trying to play an audioclip but AudioService is Null");
       
    }

    public void PlayRandomClip(AudioPair.SoundType soundType)
    {
        Debug.LogError("You are trying to play an audioclip but AudioService is Null");
       
    }

    public void StopAudioClip()
    {
        Debug.LogError("You are trying to stop an audioclip but AudioService is Null");
    }

    public void StopService()
    {
        return;
    }
}
