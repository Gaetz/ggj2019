using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

	void Start () {
		Button startButton = GameObject.Find("StartButton").GetComponent<Button>();
		startButton.Select();
	}

	public void StartGame() {
		DataAccess.Instance.ResetLives();
		SceneManager.LoadScene("MainLevelDesign");
	}

	public void MainMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	public void Ballad() {
		SceneManager.LoadScene("BalladMenu");
	}

	public void Credits() {
		SceneManager.LoadScene("Credits");
	}

	public void Quit() {
		Application.Quit();
	}
}
