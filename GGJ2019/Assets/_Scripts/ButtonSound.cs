using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour, ISelectHandler {    
 
	[SerializeField] AudioSource source;
	[SerializeField] AudioClip hoverSfx;
	[SerializeField] AudioClip clickSfx;

	bool selected;

	void Start() {
		hoverSfx.LoadAudioData();
		clickSfx.LoadAudioData();
	}

	public void OnSelect(BaseEventData eventData) {
			source.PlayOneShot(hoverSfx);
	}

	public void OnClick() {
		source.PlayOneShot(clickSfx);
	}

}