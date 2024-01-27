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
	

	public static Action<float> OnBounce;

	private void Start() {
		m_rbody = GetComponent<Rigidbody2D>();
		defultGravity = m_rbody.gravityScale;
	}
	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Vector2 movinDir = m_rbody.velocity;
			movinDir.Normalize();
			AddBounceVelocity(movinDir, 3);

		}
	}

	void FixedUpdate() {
		float velMagnitude = m_rbody.velocity.magnitude;

		speedText.text = Mathf.Floor(velMagnitude).ToString();

		if (velMagnitude > criticalVelocity) {
			m_rbody.gravityScale = 0;
		}
		else if(velMagnitude < 10) {
			m_rbody.gravityScale = defultGravity * 1.5f;
		}
		else {
			m_rbody.gravityScale = defultGravity;
		}

	}
	private void OnTriggerEnter2D(Collider2D collision) {
		OnBounce?.Invoke(m_rbody.velocity.magnitude);
	}

	public void AddBounceVelocity(Vector2 dir, float ammount) {
		float currenMagnitude = m_rbody.velocity.magnitude;
		m_rbody.velocity = dir * (currenMagnitude + ammount);
	}
}
