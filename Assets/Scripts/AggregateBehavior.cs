using System.Security.Cryptography;
using UnityEngine;

public class AggregateBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.z < 5.5f || gameObject.transform.position.z > 7.5f
        || gameObject.transform.position.x < -0.2f || gameObject.transform.position.x > 1.9f) {
            Destroy(this.gameObject);
        }
	}
}
