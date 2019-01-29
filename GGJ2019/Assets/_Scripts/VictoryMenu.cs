using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryMenu : MonoBehaviour {

	void Start () {
		Button menuButton = GameObject.Find("BalladButton").GetComponent<Button>();
		menuButton.Select();
	}
	
	public void MenuButton() {
		SceneManager.LoadScene("MainMenu");
	}

	public void BalladButton() {
		SceneManager.LoadScene("BalladMenu");
	}
}
