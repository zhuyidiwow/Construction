using UnityEngine;

public class GetterUtility : MonoBehaviour{

    public Material yellowMat;
    private static ComponentInfoManager componentInfoManager;

    public Material GetYellowMat() {
        return yellowMat;
    }

    public static GameManager GetGameManager() {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager != null) {
            return gameManager;
        }
        else {
            Debug.Log("GameManager not found");
            return null;
        }
    }

    public static ComponentInfoManager GetComponentInfoManager() {
        componentInfoManager = GameObject.Find("ComponentInfoManager").GetComponent<ComponentInfoManager>();
        if (componentInfoManager != null) {
            return componentInfoManager;
        }
        else {
            Debug.Log("ComponentInfoManager not found");
            return null;
        }
    }

    public static ProgressManager GetProgressManager() {
        ProgressManager progressManager = GameObject.Find("ProgressManager").GetComponent<ProgressManager>();
        if (progressManager != null) {
            return progressManager;
        }
        else {
            Debug.Log("ProgressManager not found");
            return null;
        }
    }
}
