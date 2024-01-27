using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeBounceHandler : MonoBehaviour {
	private Rigidbody2D m_rbody;
	public PhysicsMaterial2D m_material;
	

	public float maxTime = 1;
	public float maxVelocity = 20;

	private bool stopBounce = false;
	private void Start() {
		m_rbody = GetComponent<Rigidbody2D>();
		//Time.timeScale = 0.2f;



	}
	void FixedUpdate() {
		Debug.Log(m_rbody.velocity.magnitude);
		m_rbody.velocity = Vector2.ClampMagnitude(m_rbody.velocity, maxVelocity);
	}
	private void OnTriggerEnter2D(Collider2D collision) {

		if (m_rbody.velocity.magnitude > 10) {
			m_rbody.gravityScale = 0;
			m_material.bounciness = 2f;
		}
		Debug.Log("Bounce");
	}

	void ReturnGravity() {
		m_rbody.gravityScale = 0;
		m_material.bounciness = 2f;
	}

	private void ResetBounce() {
		m_rbody.gravityScale = 2.5f;
		m_material.bounciness = 0.1f;
	}
}
