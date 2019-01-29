using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour {

	void Start () {
		Button restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
		restartButton.Select();
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void RestartButton() {
		DataAccess.Instance.ResetLives();
		SceneManager.LoadScene("MainLevelDesign");
	}

	public void MenuButton() {
		SceneManager.LoadScene("MainMenu");
	}
}
