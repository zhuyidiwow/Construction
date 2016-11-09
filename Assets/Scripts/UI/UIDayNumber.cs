using UnityEngine;
using UnityEngine.UI;

public class UIDayNumber : MonoBehaviour {

	public Slider slider;

    public void UpdateText() {
        GetComponent<Text>().text = slider.value.ToString();
    }

}
