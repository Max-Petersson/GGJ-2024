using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFrameOnImpact : MonoBehaviour
{
    [SerializeField] private float velocityForImpactFrame = 25f;
    [SerializeField] private bool freezeTimescale;
    [SerializeField] private float freezeTime = 0.2f;
    [SerializeField] private float cooldown = 5f;

    [SerializeField] private GameObject particleEffect;
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

        if (collision.rigidbody == rb)
            return;

        if (rb.velocity.magnitude < velocityForImpactFrame)
            return;

        if (Time.time < timer + cooldown)
            return;
        var contact = collision.GetContact(0);
        Vector2 moveToPoint = contact.point + contact.normal * 0.5f;

        transform.position = moveToPoint;
        accumulatedForce += collision.relativeVelocity;
        TriggerFreeze();
        Debug.Log("Collided with " + collision.transform.name, collision.transform);
    }

    private void TriggerFreeze()
    {
        timer = Time.time;
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        GameObject effect = Instantiate(particleEffect, transform.position, transform.rotation);
        Destroy(effect, freezeTime);
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
