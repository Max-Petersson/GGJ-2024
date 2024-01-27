using System.Collections;
using System.Collections.Generic;
using UnityUtils;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq.Expressions;

public class LaughOMeter : MonoBehaviour
{
    // Start is called before the first frame update

    private float m_multiplyer = 1;
    private int m_currentScore = 0;
    private bool m_isHoldingPlayer = false;

    private const float c_multiplyerAddon = .1f;
    WaitForSeconds m_waitTime = new WaitForSeconds(2f);

    public float Multiplyer 
    { 
        get { return m_multiplyer; }
        set 
        {
            if(m_multiplyer != value)
            {
                m_multiplyer = value;
                EventManager.Instance.OnMultiplyerChanged(m_multiplyer);
            }
        }
    }
    public int CurrentScore 
    {  
        get { return m_currentScore; }
        set 
        { 
            if(m_currentScore != value)
            {
                m_currentScore = value;
                EventManager.Instance.OnScoreChanged?.Invoke(m_currentScore);
            }
        }
    }
    private void OnEnable()
    {
        EventManager.Instance.OnGrab += Grabbed;
        StrikeBounceHandler.OnBounce += Bounced;
        EventManager.Instance.OnRelease += Released;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGrab -= Grabbed;
        StrikeBounceHandler.OnBounce -= Bounced;
        EventManager.Instance.OnRelease -= Released;

    }
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Smacked();
        }
    }
    public void Grabbed()
    {
        Multiplyer += c_multiplyerAddon;
        m_isHoldingPlayer = true;
        StartCoroutine(TimerForMultiPlyerReset());
    }
    public void Released()
    {
        m_isHoldingPlayer = false;
    }
    IEnumerator TimerForMultiPlyerReset()
    {
        yield return m_waitTime;
        if(m_isHoldingPlayer)
        {
            m_multiplyer = 1;
        }
    }
    public void Shot()
    {
        AudioProvider.AudioService.PlayAudioClip("Gun");
        Multiplyer += c_multiplyerAddon;
    }
    public void Smacked()
    {
        //AudioProvider.AudioService.PlayRandomClip((AudioPair.SoundType.Punch)); // make a true audioClip thingy
        Multiplyer += c_multiplyerAddon;
    }
    public void Bounced(float velocity)
    {
        float currentScore = velocity * Multiplyer;
        CurrentScore += (int)currentScore;
    }
}
