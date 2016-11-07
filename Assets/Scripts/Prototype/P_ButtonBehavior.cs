using System.Collections;
using UnityEngine;

public class P_ButtonBehavior : MonoBehaviour {

    public GameObject aggregate;
    public GameObject largeAggregate;

    private float interval;
    private Vector3 aggregatePosition;

    void OnMouseEnter() {
        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_Color", Color.red);
    }

    void OnMouseExit() {
        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_Color", Color.white);
    }

    void OnMouseOver() {
        if(Input.GetMouseButtonDown (0)) {
            StartCoroutine("PourConcrete");
            StartCoroutine("PourConcrete");
            StartAudio();
        }
    }

    IEnumerator PourConcrete()
    {
        while (true) {
            PourOneAggregate();
            // suspend execution for 5 seconds
            yield return new WaitForSeconds(0.01f);
        }
    }

    void StartAudio() {
        gameObject.GetComponent<AudioSource>().Play();
    }

    void PourOneAggregate() {
        aggregate.transform.localScale = GenerateScale();
        GameObject newObj = (GameObject) Instantiate(aggregate, GenerateRandPos(), new Quaternion());
        newObj.GetComponent<Rigidbody>().AddForce(0f, 0f, 30f + Random.value * 140f);
    }

    Vector3 GenerateRandPos() {
        float x = 0.556f, y = 5.691f, z = 5.7f;
        x = x + Random.value * 0.592f;
        y = y + Random.value * 0.495f;
        Vector3 newPos = new Vector3(x, y, z);
        return newPos;
    }

    Vector3 GenerateScale() {
        float rad = 0f;
        rad = Random.value * 0.1f + 0.1f;
        Vector3 newScale = new Vector3(rad, rad, rad);
        return newScale;
    }
}
