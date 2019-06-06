using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    void Update()
    {
        var s = Camera.main.orthographicSize * 1.5f * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) gameObject.transform.position += new Vector3(s, 0, 0);
        else if (Input.GetKey(KeyCode.A)) gameObject.transform.position += new Vector3(-s, 0, 0);
        else if (Input.GetKey(KeyCode.W)) gameObject.transform.position += new Vector3(0, s, 0);
        else if (Input.GetKey(KeyCode.S)) gameObject.transform.position += new Vector3(0, -s, 0);

        Camera.main.orthographicSize += Input.mouseScrollDelta.y;

        if (Camera.main.orthographicSize < 3) Camera.main.orthographicSize = 3;
    }
}
