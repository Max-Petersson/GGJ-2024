using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneMenu : MonoBehaviour
{
    public GameObject menuButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P)) {
			if (menuButton.activeSelf) {
				menuButton.SetActive(false);
				Cursor.visible = false;
			}
            else {
				menuButton.SetActive(true);
				Cursor.visible = true;
			}

		}
    }
}
