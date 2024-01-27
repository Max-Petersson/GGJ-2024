using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFrameOnImpact : MonoBehaviour
{
    [SerializeField] private float velocityForImpactFrame;
    [SerializeField] private bool freezeTimescale;
    [SerializeField] private float freezeTime;
    [SerializeField] private float cooldown;
    private Rigidbody2D rb;
    private float timer;

    [SerializeField] private AudioClip hitSound;

    Vector2 accumulatedForce;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.velocity.magnitude < velocityForImpactFrame)
            return;

        if (Time.time < timer + cooldown)
            return;

        accumulatedForce += collision.relativeVelocity;
        TriggerFreeze();
        
    }

    private void TriggerFreeze()
    {
        timer = Time.time;
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        StartCoroutine(CoFreezeFrame(freezeTime));
    }


    private IEnumerator CoFreezeFrame(float duration)
    {
        if (freezeTimescale)
            Time.timeScale = 0;

        rb.simulated = false;

        yield return new WaitForSecondsRealtime(duration);

        rb.simulated = true;

        if (freezeTimescale)
            Time.timeScale = 1;

        rb.AddForce(accumulatedForce, ForceMode2D.Impulse);
        accumulatedForce = Vector2.zero;
    }
}
