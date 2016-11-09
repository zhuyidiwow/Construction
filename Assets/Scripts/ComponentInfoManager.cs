using UnityEngine;
using UnityEngine.UI;

public class ComponentInfoManager : MonoBehaviour {
    public GameObject infoPanel;
    public Text infoText;

    public BComponent formerComponent;
    public BComponent currentComponent;

	// Use this for initialization
	void Start () {
	    formerComponent = null;
        currentComponent = null;
        infoPanel.SetActive(false);
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Clear();
        }
    }
    public void UpdateComponent(BComponent newComponent) {
        if (currentComponent == null) {
            currentComponent = newComponent;
            currentComponent.SetHighlight();
            infoPanel.SetActive(true);
        } else {
            infoPanel.SetActive(true);
            formerComponent = currentComponent;
            currentComponent = newComponent;

            formerComponent.SetBack();
            currentComponent.SetHighlight();
        }
        UpdateInfoText();
    }

    void Clear() {
        if (currentComponent != null) {
            currentComponent.SetBack();
            ClearInfoText();
            infoPanel.SetActive(false);
        }
    }

    void UpdateInfoText() {
        ComponentInfo info = currentComponent.componentInfo;
        string newInfoText =
                "<b>Name:</b> " + info.Name + "\n" +
                "<b>Scheduled Day:</b> " + info.ScheduledDay + "\n" +
                "<b>Activity:</b> " + info.Activity;

        infoText.text = newInfoText;
    }

    void ClearInfoText() {
        infoText.text = "";
    }
}
