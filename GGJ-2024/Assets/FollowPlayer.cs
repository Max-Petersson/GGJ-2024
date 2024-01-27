using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform ragdoll;

    private RagdollPhysicsController ragdollPhysicsController;
    private Vector2 normal;


    // Start is called before the first frame update
    void Start()
    {
        ragdollPhysicsController = ragdoll.GetComponentInParent<RagdollPhysicsController>();

        ragdollPhysicsController.AddForce(new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized * 20f, ForceMode2D.Impulse);
    }


    private void FixedUpdate()
    {
        transform.position = ragdoll.position;
        transform.rotation = ragdoll.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        normal = collision.GetContact(0).normal;

        ragdollPhysicsController.ReflectVelocity(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ragdollPhysicsController.AddForce(normal, ForceMode2D.Impulse);
    }
}
