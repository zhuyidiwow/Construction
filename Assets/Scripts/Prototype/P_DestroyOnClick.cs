using UnityEngine;
using System.Collections;

public class P_DestroyOnClick : MonoBehaviour {

    void OnMouseOver() {
        if(Input.GetMouseButtonDown (0)) {
            Destroy (this.gameObject);
        }
    }
}
