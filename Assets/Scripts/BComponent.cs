using System.Collections;
using UnityEngine;

public class BComponent : MonoBehaviour {

    #region movement variables
    public float yOffset = 40f;
    public float animationDuration = 5f;

    private Vector3 originalPos;
    private Vector3 startPos;
    #endregion

    private Renderer thisRenderer;
    private Material yellowMat;
    private Material originalMat;

    [HideInInspector] public ComponentInfo componentInfo;
    private ComponentInfoManager componentInfoManager;

    private GetterUtility getterUtility;

    // components are hidden in the beginning
	void Start () {
        
        // get positions ready
        originalPos = this.transform.localPosition;
        startPos = originalPos + new Vector3(0f, yOffset, 0f);

        // get materials ready
        getterUtility = GameObject.Find("GetterUtility").GetComponent<GetterUtility>();
        thisRenderer = GetComponent<Renderer>();
        originalMat = thisRenderer.material;
        yellowMat = getterUtility.GetYellowMat();

        // get information ready
        componentInfo = GetComponent<ComponentInfo>();
        componentInfoManager = GetterUtility.GetComponentInfoManager();

        this.gameObject.SetActive(false); // remember this does not call Update or something when in-active
	}

    public void ComeIn() {
        this.gameObject.SetActive(true);
        transform.localPosition = startPos;
        StartCoroutine(ComeInCoroutine(startPos));
    }

    IEnumerator ComeInCoroutine(Vector3 startPosition) {
        float startTime = Time.time;
        float timePast = Time.time - startTime;
        while (timePast <= animationDuration) {
            timePast = Time.time - startTime;
            transform.localPosition = Vector3.Lerp(startPosition, originalPos, timePast / animationDuration);
            yield return new WaitForSeconds(0.02f);
        }
        StopCoroutine("ComeInCoroutine");
    }

    public void FlyOut() {
        StartCoroutine(FlyOutCoroutine(startPos));
    }

    IEnumerator FlyOutCoroutine(Vector3 targetPos) {
        float startTime = Time.time;
        float timePast = Time.time - startTime;
        while (timePast <= animationDuration) {
            timePast = Time.time - startTime;
            transform.localPosition = Vector3.Lerp(originalPos, targetPos, timePast / animationDuration);
            yield return new WaitForSeconds(0.02f);
        }
        this.gameObject.SetActive(false);
        StopCoroutine("FlyOutCoroutine");
    }

    void OnMouseDown() {
        componentInfoManager.UpdateComponent(this);
    }

    public void SetHighlight() {
        thisRenderer.material = yellowMat;
    }

    public void SetBack() {
        thisRenderer.material = originalMat;
    }

}
