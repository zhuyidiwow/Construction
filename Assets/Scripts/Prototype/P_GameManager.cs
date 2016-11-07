using UnityEngine;
using System.Collections;

public class P_GameManager : MonoBehaviour {

	public GameObject staticCam;
    public GameObject fpsController;

    #region UI
    public GameObject hudCanvas;
    public GameObject welcomeCanvas;
    public GameObject planningCanvas;
    public GameObject resultCanvas;
    #endregion

    void Start() {
        Initialize();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            ToggleCamera();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Initialize();
        }
    }

    void Initialize() {
    	staticCam.SetActive(true);
        fpsController.SetActive(false);
        hudCanvas.SetActive(false);
        welcomeCanvas.SetActive(true);
        planningCanvas.SetActive(false);
        resultCanvas.SetActive(false);
    }

    public void StartPlanning() {
        hudCanvas.SetActive(false);
        planningCanvas.SetActive(true);
    }

    public void SubmitPlan() {
        planningCanvas.SetActive(false);
        resultCanvas.SetActive(true);
    }


    public void StartWalking() {
        staticCam.SetActive(false);
        fpsController.SetActive(true);
        hudCanvas.SetActive(false);
        welcomeCanvas.SetActive(false);
        planningCanvas.SetActive(false);
    }

    void ToggleCamera() {
        staticCam.SetActive(!staticCam.activeSelf);
        fpsController.SetActive(!fpsController.activeSelf);
        hudCanvas.SetActive(!hudCanvas.activeSelf);
    }
}
