using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
	public Sprite[] hitSprites;
	private SpriteRenderer m_spriteRenderer;
	private int currentIndex = 10;

	private void Start() {
		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void RandomHitSprite() {
		if (hitSprites.Length <= 1) return;

		int randomIndex;
		do {
			randomIndex = Random.Range(0, hitSprites.Length);
		} while (randomIndex == currentIndex);

		m_spriteRenderer.sprite = hitSprites[randomIndex];
		currentIndex = randomIndex;
	}
}
