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
    public enum SoundType {SmallLaugh, SemiLaugh, BigLaugh, SlowClap, WilhemScream, Boing, Punch, GunShot, BatSwing}
    public SoundType Type;  
}
public interface IAudioService
{
    public void PlayAudioClip(string clipName);
    public void PlayConstant();
    public void StopAudioClip();
    public void PlayRandomClip(AudioPair.SoundType soundType);
    public void InitializeService();
    public void StopService();
}
public class AudioProvider : MonoBehaviour
{

    [SerializeField] AudioSource m_continousLaughter;
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
        SetService(new RealAudioService(m_pairs, m_source, m_continousLaughter));
    }
    private void OnDisable()
    {
        AudioService.StopService();
    }
    // Update is called once per frame
   
}

public class RealAudioService : IAudioService
{
    AudioSource m_continousLaughter;
    [SerializeField] AudioPair[] m_pairs;
    private AudioSource m_audioClipSource;
    private static bool m_isPlaying = false;
    private enum CurrentlyPlaying {SmallLaugh, SemiLaugh, BigLaugh, None}
    CurrentlyPlaying state = CurrentlyPlaying.None;
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
    public RealAudioService(AudioPair[] pairs , AudioSource source, AudioSource continousLaughter)
    {
        m_pairs = pairs;
        m_audioClipSource = source;
        m_continousLaughter = continousLaughter;
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
        m_audioClipSource.GetComponent<AudioSource>().PlayOneShot(pair.AudioClip, pair.Volume); // play it with the volume 
        
        startTimerForAudio?.Invoke(pair.Duration); // wait until another can be played
    }
    public AudioPair GetClip(string key)
    {
        foreach (AudioPair audioPair in m_pairs) // find the audio with the specifyed key
        {
            if (audioPair.Key == key)
            {
                return audioPair;
            }

        }
        return null;
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
        m_audioClipSource.Stop();
    }

    public void InitializeService() // setup listener for isplayingchanged
    {
        IsPlayingFalse += StopAudio;
        EventManager.Instance.OnMultiplyerChanged += MultiplyerChanged;
        EventManager.Instance.OnShoot += Shoot;
        EventManager.Instance.OnSwingBat += SwingBat;
    }
    public void Shoot()
    {
        PlayAudioClip("GunShot");
    }
    public void SwingBat()
    {
        PlayAudioClip("SwingBat");
    }
    public void MultiplyerChanged(float multiplyer)
    {
        float percentage = (multiplyer - 1) / UILaughOMeter.c_maxMultiplyer;
        percentage = Mathf.Clamp(percentage, 0, 1);
        Debug.Log(percentage);
        if (percentage > 0.66f )
        {
            if (state == CurrentlyPlaying.BigLaugh)
                return;
            state = CurrentlyPlaying.BigLaugh;
            PlayContinous("BigLaugh");
            //play huge laughter
            return;
        }
        else if(percentage > 0.33f)
        {
            if (state == CurrentlyPlaying.SemiLaugh)
                return;
            state = CurrentlyPlaying.SemiLaugh;
            PlayContinous("SemiLaugh");
            
            return;
        }
        else
        {
            if (state == CurrentlyPlaying.SmallLaugh)
                return;

            state = CurrentlyPlaying.SmallLaugh;
            PlayContinous("SmallLaugh");
        }


        //play small laugh
    }

    private void PlayContinous(string key)
    {
        AudioPair audioPair = GetClip(key);
        m_continousLaughter.clip = audioPair.AudioClip;
        m_continousLaughter.volume = audioPair.Volume;
        m_continousLaughter.Play();
    }

    public void StopService() // unsubscribe
    {
        IsPlayingFalse -= StopAudio;
        EventManager.Instance.OnMultiplyerChanged -= MultiplyerChanged;
        EventManager.Instance.OnShoot -= Shoot;
        EventManager.Instance.OnSwingBat -= SwingBat;
    }

    public void PlayConstant()
    {
        m_continousLaughter.Play();
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

    public void PlayConstant()
    {
        Debug.LogError("You are trying to stop an audioclip but AudioService is Null");

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
