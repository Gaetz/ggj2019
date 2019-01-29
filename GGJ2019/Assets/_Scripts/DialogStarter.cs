using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogStarter : MonoBehaviour {

	[Tooltip("Space between NPC and dialog window")]
	[SerializeField] float dialogAbovePosition;

	[Tooltip("Link toward the dialog background panel")]
	[SerializeField] Transform ChatBackground;

	[Tooltip("Text content, one text per dialog box")]
	[SerializeField][TextArea(5, 10)] string[] sentences;
	DialogSystem dialogSystem;
	
	bool isDialogDisplayed;

	void Start () {
		dialogSystem = FindObjectOfType<DialogSystem>();
		isDialogDisplayed = false;
	}

	public void OnTriggerStay2D(Collider2D other) {
		//string period = sentences
		if(DataAccess.Instance.Lives > 0 && DataAccess.Instance.Lives < 9) {
			string[] oneSentence = new string[1] { sentences[8 - DataAccess.Instance.Lives] };
			if (!isDialogDisplayed && other.gameObject.tag == "Player") {
					isDialogDisplayed = true;
					Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
					pos.y += dialogAbovePosition;
					ChatBackground.position = pos;
					dialogSystem.EnterRangeOfNPC();
					dialogSystem.dialogLines = oneSentence;
					dialogSystem.TriggerDialog();
			}
		}
	}

	public void OnTriggerExit2D(Collider2D other) {
		dialogSystem.OutOfRange();
		isDialogDisplayed = false;
	}
}
