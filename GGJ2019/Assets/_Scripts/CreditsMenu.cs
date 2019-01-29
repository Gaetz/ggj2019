using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsMenu : MonoBehaviour {

	void Start () {
		Button menuButton = GameObject.Find("MenuButton").GetComponent<Button>();
		menuButton.Select();
	}

	public void MenuButton() {
		SceneManager.LoadScene("MainMenu");
	}
}
