using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaughWhenHit : MonoBehaviour
{
    [SerializeField] private AudioClip[] laughs;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();

        EventManager.Instance.OnShoot += Laugh;
        EventManager.Instance.OnSwingBat += Laugh;
        EventManager.Instance.OnRelease += Laugh;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnShoot -= Laugh;
        EventManager.Instance.OnSwingBat -= Laugh;
        EventManager.Instance.OnRelease -= Laugh;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Laugh()
    {
        audioSource.volume = 1;
        int random = UnityEngine.Random.Range(0, laughs.Length);
        audioSource.clip = laughs[random];
        audioSource.Play();
        StopAllCoroutines();
        StartCoroutine(FadeVolume());
    }

    private IEnumerator FadeVolume()
    {
        float vol = 1;
        while (vol > 0)
        {
            audioSource.volume = vol;
            vol -= Time.deltaTime * 0.5f;
            yield return null;
        }
        audioSource.Stop();
    }
}
