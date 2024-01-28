using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFrameOnImpact : MonoBehaviour
{
    [SerializeField] private bool freezeOtherRigidbody;
    [SerializeField] private float velocityForImpactFrame = 25f;
    [SerializeField] private bool freezeTimescale;
    [SerializeField] private float freezeTime = 0.2f;
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private float speedForce = 2f;
    [SerializeField] private bool disableColliderOnCooldown = false;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private GameObject particleEffect;

    private Collider2D col;
    private Rigidbody2D rb;
    private float timer;

    [SerializeField] private AudioClip[] hitSounds;

    private SpriteSwitcher spriteSwitcher;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (audioSource == null)
        {
            audioSource = FindAnyObjectByType<AudioSource>();
        }

        spriteSwitcher = FindAnyObjectByType<SpriteSwitcher>();
    }

    //private void Update()
    //{
    //    if (!disableColliderOnCooldown)
    //    {
    //        return;
    //    }

    //    if (Time.time < timer + cooldown)
    //        col.enabled = false;
    //    else
    //        col.enabled = true;

    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (freezeOtherRigidbody && collision.rigidbody == null)
            return;

        Rigidbody2D rigidbody = freezeOtherRigidbody ? collision.rigidbody : rb;

        //collided with self
        if (collision.otherRigidbody == rigidbody)
            return;

        if (rigidbody.velocity.magnitude < velocityForImpactFrame)
            return;

        if (Time.time < timer + cooldown)
            return;

        if (!rigidbody.gameObject.CompareTag("Player"))
        {
            return;
        }

        var contact = collision.GetContact(0);

        if (!freezeOtherRigidbody)
        {
            Vector2 moveToPoint = contact.point + contact.normal * 0.5f;
            transform.position = moveToPoint;
        }

        //Vector2 relativeVelocity = freezeOtherRigidbody ? -collision.relativeVelocity : collision.relativeVelocity;

        //accumulatedForce += rb.velocity * 0.5f;
        TriggerFreeze(rigidbody);
        Debug.Log("Collided with " + collision.transform.name, collision.transform);
    }

    private void TriggerFreeze(Rigidbody2D rb)
    {
        timer = Time.time;

        if (audioSource != null)
        {
            foreach (AudioClip hitSound in hitSounds)
            {
                audioSource.PlayOneShot(hitSound);
            }
        }
        
        GameObject effect = Instantiate(particleEffect, rb.transform.position, rb.transform.rotation);
        Destroy(effect, freezeTime);

        if (freezeOtherRigidbody)
        {
            spriteSwitcher.Electrify(freezeTime);
        }

        StartCoroutine(CoFreezeFrame(rb, freezeTime));
    }


    private IEnumerator CoFreezeFrame(Rigidbody2D rb, float duration)
    {
        if (freezeTimescale)
            Time.timeScale = 0;

        Collider2D col = this.rb.GetComponent<Collider2D>();

        rb.simulated = false;
        if (freezeOtherRigidbody && this.rb != null)
        {
            this.rb.simulated = false;
        }

        yield return new WaitForSecondsRealtime(duration);

        rb.simulated = true;

        if (freezeOtherRigidbody && this.rb != null)
        {
            this.rb.simulated = true;
        }

        if (freezeTimescale)
            Time.timeScale = 1;

        Vector2 random = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
        rb.GetComponent<StrikeBounceHandler>().AddBounceVelocity(random, speedForce);

        //rb.velocity *= speedForce;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (Time.time < timer + cooldown)
            return;

        if (!freezeOtherRigidbody)
        {
            return;
        }

        TriggerFreeze(collision.attachedRigidbody);
    }
}
