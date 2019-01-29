using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScript : MonoBehaviour {

	[SerializeField] Transform player;
	
	// Update is called once per frame
	void Update () {
		if (player.position.x > 376f) {
			SceneManager.LoadScene("Victory1");
		}
	}
}
