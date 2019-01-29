using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeclenchePuff : MonoBehaviour {

    public GameObject prefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("OnCollisionEnter2D");
            Instantiate(prefab, this.transform.position, Quaternion.identity);
        }
    }
}
