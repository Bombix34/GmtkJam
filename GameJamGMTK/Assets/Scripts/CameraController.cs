using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    public float magnitude = 2f;
    public float frequency = 10f;

    private Vector2 vectorLook;
    private Vector2 smoothV;
    private GameObject Character;

    private bool cameraShake = false;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
        Character = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 inputValues = new Vector2(Input.GetAxisRaw("JoystickX"), Input.GetAxisRaw("JoystickY"));

        if(inputValues == Vector2.zero)
        {
            inputValues = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }

        inputValues = Vector2.Scale(inputValues, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, inputValues.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, inputValues.y, 1f / smoothing);

        vectorLook += smoothV;
        vectorLook.y = Mathf.Clamp(vectorLook.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(-vectorLook.y, Vector3.right);
        Character.transform.localRotation = Quaternion.AngleAxis(vectorLook.x, Vector3.up);

        if(cameraShake)
        {
            Vector2 perlinVector = PerlinShake();
            transform.localPosition = new Vector3(perlinVector.x, perlinVector.y, 0f);
        }
	}

    public Vector2 PerlinShake()
    {
        Vector2 result;
        float seed = Time.time * frequency;
        result.x = Mathf.Clamp01(Mathf.PerlinNoise(seed, 0f)) - 0.5f;
        result.y = Mathf.Clamp01(Mathf.PerlinNoise(0f, seed)) - 0.5f;
        result = result * magnitude;
        return result;
    }

    public void startCameraShake()
    {
        if(!cameraShake)
        {
            StartCoroutine("CameraShake");
        }
    }

    IEnumerator CameraShake()
    {
        cameraShake = true;
        yield return new WaitForSeconds(0.5f);
        cameraShake = false;
        transform.localPosition = Vector3.zero;
    }
}
