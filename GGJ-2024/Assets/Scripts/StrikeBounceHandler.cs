using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class StrikeBounceHandler : MonoBehaviour {
	public TextMeshProUGUI speedText;
	public float criticalVelocity = 20;

	private float defultGravity;
	private float velMagnitude;

	public static Action<float> OnBounce;
	public UnityEvent OnBounceEvent;

	private Rigidbody2D m_rbody;
	public CircleCollider2D m_circleCollider;
	public PhysicsMaterial2D m_defultMaterial;
	public PhysicsMaterial2D m_slowMaterial;
	public PhysicsMaterial2D m_stopMaterial;

	private float totalSpeed;
	private float numSamples;

	float averageSpeed = 0;

	float idleTimer = 1000;
	private float timer;

	public GameObject hitVisuals;
	public GameObject idelVisuals;

	private void Start() {
		m_rbody = GetComponent<Rigidbody2D>();

		defultGravity = m_rbody.gravityScale;

		totalSpeed = 0;
		numSamples = 0;

		timer = 10000;
	}
	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Vector2 movinDir = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
			movinDir.Normalize();
			AddBounceVelocity(movinDir, 5);
		}

		timer += Time.deltaTime;
		if (timer > 10) {
			m_circleCollider.sharedMaterial = m_stopMaterial;
			m_rbody.angularDrag = 100;
		}
		else if (timer > 5) {
			m_circleCollider.sharedMaterial = m_slowMaterial;
			m_rbody.angularDrag = 10;
		}
		else {
			m_circleCollider.sharedMaterial = m_defultMaterial;
			m_rbody.angularDrag = 1;
		}

		if (averageSpeed > 0.1f) {
			idleTimer = 0;
		}
		else {
			idleTimer += Time.deltaTime;
		}

		if(idleTimer > 1) {
			hitVisuals.SetActive(false);
			idelVisuals.SetActive(true);
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		else {
			hitVisuals.SetActive(true);
			idelVisuals.SetActive(false);
		}
	}


	void FixedUpdate() {
		velMagnitude = m_rbody.velocity.magnitude;		

		totalSpeed += velMagnitude;
		numSamples++;

		if (numSamples >= 20) {
			averageSpeed = totalSpeed / numSamples;


			if (averageSpeed > criticalVelocity) {
				m_rbody.gravityScale = 0;
			}
			else if (averageSpeed > 10) {
				m_rbody.gravityScale = defultGravity;
			}
			else if(averageSpeed > 0) {
				m_rbody.gravityScale = defultGravity * 1.5f;
			}
			

			totalSpeed = 0f;
			numSamples = 0;

			if (speedText)
				speedText.text = Mathf.Floor(averageSpeed).ToString();
		}

		if (speedText)
			speedText.text = Mathf.Floor(velMagnitude).ToString();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		OnBounce?.Invoke(m_rbody.velocity.magnitude);

		if (m_rbody.velocity.x < 0) {
			m_rbody.angularVelocity = UnityEngine.Random.Range(0.5f, 50 * velMagnitude);
		}
		else {
			m_rbody.angularVelocity = UnityEngine.Random.Range(-0.5f, -50 * velMagnitude);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		OnBounceEvent.Invoke();

	}

	public void AddBounceVelocity(Vector2 dir, float ammount) {
		m_rbody.velocity = dir * (velMagnitude + ammount);
		timer = 0;
		idleTimer = 0;
		OnBounceEvent.Invoke();
		OnBounce?.Invoke(m_rbody.velocity.magnitude);
	}
}
