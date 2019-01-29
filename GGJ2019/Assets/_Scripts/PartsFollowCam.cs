using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsFollowCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void FixedUpdate()
    {
        transform.position = (new Vector3(Camera.main.gameObject.transform.position.x,transform.position.y,transform.position.z));
    }
}
