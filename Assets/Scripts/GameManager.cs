using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public BComponent[] components;
    public ProgressManager progressManager;
    public Text switchPhaseButtonText;

    private bool inViewPhase = true;

    public void SwitchPhase() {
        if (inViewPhase) {
            progressManager.SwitchToPlan();
            switchPhaseButtonText.text = "Back to View";
            inViewPhase = false;
        } else {
            progressManager.SwitchToView();
            switchPhaseButtonText.text = "Back to View";
            inViewPhase = true;
        }
    }

//    void Update() {
//        if (Input.GetKeyDown(KeyCode.F)) {
//            foreach (BComponent component in components)
//                component.FlyOut();
//        }
//        if (Input.GetKeyDown(KeyCode.R)) {
//            foreach (BComponent component in components)
//                component.ComeIn();
//        }
//    }
}
