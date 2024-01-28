using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTransitioneer : MonoBehaviour
{
	private Animator m_animator;
	string stageToLoad;

	private void Awake() {
		m_animator = GetComponentInChildren<Animator>();
	}


	public void ActivateTransition(string levelToLoad) {
		stageToLoad = levelToLoad;

		m_animator.SetTrigger("ZoomNow");
		Invoke(nameof(TransitionNOW), 1.5f);
	}

	private void TransitionNOW() {
		SceneManager.LoadScene(stageToLoad);
	}

}
