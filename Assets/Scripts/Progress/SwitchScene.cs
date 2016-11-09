using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour {

	public void GoToSite() {
        SceneManager.LoadScene("DemoHouse");
    }
}
