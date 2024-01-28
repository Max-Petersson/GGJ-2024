using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
	public Sprite[] hitSprites;
	private SpriteRenderer m_spriteRenderer;
	private int currentIndex = 10;

	public Sprite[] electrifySprites;

	public Sprite[] electrifyVFXSprites;

	public SpriteRenderer electrifyVFX;

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

	public void Electrify(float duration)
    {
		StartCoroutine(CoElectrify(duration));
    }

	public IEnumerator CoElectrify(float duration)
    {
		float counter = 0;
		int framecounter = 0;
		int vfxCounter = 0;
		electrifyVFX.enabled = true;

        while (counter < duration)
        {
			framecounter = framecounter < electrifySprites.Length ? framecounter : 0;
			vfxCounter = vfxCounter < electrifyVFXSprites.Length ? vfxCounter : 0;

			m_spriteRenderer.sprite = electrifySprites[framecounter];
			electrifyVFX.sprite = electrifyVFXSprites[vfxCounter];
			yield return new WaitForSeconds(0.1f);

			framecounter++;
			vfxCounter++;
			counter += 0.1f;
		}

		electrifyVFX.enabled = false;
		RandomHitSprite();
    }
}
