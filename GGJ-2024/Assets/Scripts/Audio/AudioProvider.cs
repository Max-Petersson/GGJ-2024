using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioPair
{
    public string Key;
    public AudioClip[] AudioClip;

    public void GetRandomAudio()
    {
        
    }
}
public interface IAudioService
{
    public void PlayAudioClip(string clipName);
    public void StopAudioClip(string clipName);
}
public class AudioProvider : MonoBehaviour
{
    public AudioPair[]
    private static IAudioService m_audioService;
    public static IAudioService AudioService 
    {
        get { return m_audioService; }
        set { m_audioService ??= new NullAudioService(); }
    }
    
    public static void SetService(IAudioService service)
    {
        if(service == null)
        {
            AudioService = new NullAudioService();
            return;
        }

        m_audioService = service;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class AudioService : IAudioService
{
    public void PlayAudioClip(string clipName)
    {
        throw new System.NotImplementedException();
    }

    public void StopAudioClip(string clipName)
    {
        throw new System.NotImplementedException();
    }
}
public class NullAudioService : IAudioService
{
    public void PlayAudioClip(string clipName)
    {
        throw new System.NotImplementedException();
    }

    public void StopAudioClip(string clipName)
    {
        throw new System.NotImplementedException();
    }
}
