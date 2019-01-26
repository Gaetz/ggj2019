using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesCounter : MonoBehaviour {

	[SerializeField] Image liveImage;

	ShootTarget player;
	int maxLives;
	int displayedLives;
	List<Image> images;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<ShootTarget>();
		maxLives = player.MaxHp;
		displayedLives = player.MaxHp;
		images = new List<Image>();
		for(int i = 0; i < maxLives; i++) {
			Image newImage = Instantiate(liveImage);
			newImage.transform.position = transform.position;
			newImage.transform.position += new Vector3(50 + 50 * i, -50, 0);
			newImage.gameObject.GetComponent<RectTransform>().SetParent(transform);
        	newImage.gameObject.SetActive(true);
			images.Add(newImage);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(displayedLives != player.CurrentHp) {
			ChangeLives(player.CurrentHp);
			displayedLives = player.CurrentHp;
		}
	}

	void ChangeLives(int lives) {
		for(int i = maxLives - 1; i >= 0; i--) {
			if(i > lives) {
				images.RemoveAt(i);
			}
		}
	}
}
