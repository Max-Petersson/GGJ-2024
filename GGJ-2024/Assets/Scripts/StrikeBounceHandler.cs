using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StrikeBounceHandler : MonoBehaviour {
	private Rigidbody2D m_rbody;
	public TextMeshProUGUI speedText;

	public float criticalVelocity = 20;
	private float defultGravity;
	private float velMagnitude;

	public static Action<float> OnBounce;

	private void Start() {
		m_rbody = GetComponent<Rigidbody2D>();
		defultGravity = m_rbody.gravityScale;
	}
	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Vector2 movinDir = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
			movinDir.Normalize();
			AddBounceVelocity(movinDir, 3);
		}
	}

	void FixedUpdate() {
		velMagnitude = m_rbody.velocity.magnitude;

		if (velMagnitude > criticalVelocity) {
			m_rbody.gravityScale = 0;
		}
		else if (velMagnitude < 10) {
			m_rbody.gravityScale = defultGravity * 1.5f;
		}
		else {
			m_rbody.gravityScale = defultGravity;
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

	public void AddBounceVelocity(Vector2 dir, float ammount) {
		m_rbody.velocity = dir * (velMagnitude + ammount);
	}
}
