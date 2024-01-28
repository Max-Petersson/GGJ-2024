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

	private float totalSpeed;
	private float numSamples;

	private float timer;

	private void Start() {
		m_rbody = GetComponent<Rigidbody2D>();

		defultGravity = m_rbody.gravityScale;

		totalSpeed = 0;
		numSamples = 0;

		timer = 5;
	}
	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Vector2 movinDir = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
			movinDir.Normalize();
			AddBounceVelocity(movinDir, 5);
		}

		timer -= Time.deltaTime;

		if (timer <= 0) {
			m_circleCollider.sharedMaterial = m_slowMaterial;
		}
		else {
			m_circleCollider.sharedMaterial = m_defultMaterial;
		}

	}


	void FixedUpdate() {
		velMagnitude = m_rbody.velocity.magnitude;

		if (velMagnitude > criticalVelocity) {
			m_rbody.gravityScale = 0;
		}
		else if (velMagnitude > 10) {
			m_rbody.gravityScale = defultGravity;
		}
		else {
			m_rbody.gravityScale = defultGravity * 1.5f;
		}

		totalSpeed += velMagnitude;
		numSamples++;

		if (numSamples >= 20) {
			float averageSpeed = totalSpeed / numSamples;

			float minSpeedThreshold = 0.1f;



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
		OnBounceEvent.Invoke();
		if (m_rbody.velocity.x < 0) {
			m_rbody.angularVelocity = UnityEngine.Random.Range(0.5f, 50 * velMagnitude);
		}
		else {
			m_rbody.angularVelocity = UnityEngine.Random.Range(-0.5f, -50 * velMagnitude);
		}
	}

	public void AddBounceVelocity(Vector2 dir, float ammount) {
		m_rbody.velocity = dir * (velMagnitude + ammount);
		timer = 5;
	}
}
