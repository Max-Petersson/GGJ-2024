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
    [SerializeField] private float speedMultipier = 2f;

    [SerializeField] private GameObject particleEffect;
    private Rigidbody2D rb;
    private float timer;

    [SerializeField] private AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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
        AudioSource.PlayClipAtPoint(hitSound, rb.transform.position);
        GameObject effect = Instantiate(particleEffect, rb.transform.position, rb.transform.rotation);
        Destroy(effect, freezeTime);
        StartCoroutine(CoFreezeFrame(rb, freezeTime));
    }


    private IEnumerator CoFreezeFrame(Rigidbody2D rb, float duration)
    {
        if (freezeTimescale)
            Time.timeScale = 0;

        rb.simulated = false;

        yield return new WaitForSecondsRealtime(duration);

        rb.simulated = true;

        if (freezeTimescale)
            Time.timeScale = 1;

        rb.velocity *= speedMultipier;
    }
}
