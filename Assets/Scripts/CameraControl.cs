using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	private float moveSpeed = 3f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Vertical") != 0) {
			//this.GetComponent <Rigidbody>
			Transform thisT = this.transform;
			transform.position = new Vector3 (thisT.position.x + moveSpeed * Time.deltaTime, thisT.position.y, thisT.position.z);
		}
	}
}
