using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public float RotationIntensity = 1f;
    float V = 0;
    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Mouse X") * RotationIntensity;
        var v = -Input.GetAxis("Mouse Y") * RotationIntensity;
        V += v;
        this.transform.Rotate(new Vector3(v, h));

        var a = transform.rotation.eulerAngles;
        V = Mathf.Clamp(V, -85, 85);
        var r = transform.rotation;
        r.eulerAngles = new Vector3(V,a.y,0);
        transform.rotation = r;

        {
            var wh = Input.GetAxis("Horizontal");
            var wv = Input.GetAxis("Vertical");
            transform.Translate(new Vector3(wh, 0, wv));
        }
    }
}
