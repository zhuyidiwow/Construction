using UnityEngine;
using System.Collections;

public class P_ToggleDisplay : MonoBehaviour {

    public GameObject[] gameObjects;

    void Start() {
        foreach (GameObject gameObject in gameObjects) {
            gameObject.SetActive(true);
        }
    }

    public void Toggle() {
        bool current = gameObjects[0].activeSelf;
		foreach (GameObject gameObject in gameObjects) {
			gameObject.SetActive(!current);
		}
	}
}
