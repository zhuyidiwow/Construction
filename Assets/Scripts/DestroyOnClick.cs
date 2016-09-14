using UnityEngine;
using System.Collections;

public class DestroyOnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver() {
        if(Input.GetMouseButtonDown (0)) {
            Destroy (this.gameObject);
        }
    }
}
