using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPhysicsController : MonoBehaviour
{
    private Rigidbody2D[] rigidbody2Ds;

    [SerializeField]private float forceModifier = 20f;

    [SerializeField] private float bouncyness = 0.5f;

    [SerializeField] private float spinFactor = 15f;

    private Vector2 normal;

    private void Awake()
    {
        //RootRigidbody = GetComponentInChildren<Rigidbody2D>();
        rigidbody2Ds = GetComponentsInChildren<Rigidbody2D>();
    }

    public void AddForce(Vector2 force, ForceMode2D forceMode2D)
    {
        foreach(Rigidbody2D rb in rigidbody2Ds)
        {
            rb.AddForce(force, forceMode2D);
        }
    }

    public void ReflectVelocity(Collision2D collision)
    {
        normal = collision.GetContact(0).normal;

        foreach (Rigidbody2D rb in rigidbody2Ds)
        {
            rb.velocity = Vector2.Reflect(rb.velocity, normal) * Random.RandomRange(0.95f, 1.05f) * bouncyness;
            rb.AddForceAtPosition(rb.velocity * spinFactor, collision.GetContact(0).point);
        }
    }
}
