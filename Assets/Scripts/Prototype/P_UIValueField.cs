using UnityEngine;
using UnityEngine.UI;

public class P_UIValueField : MonoBehaviour {

	public Text text;
    public Slider slider;

    public void UpdateValue() {
        text.text = slider.value.ToString();
    }
}
