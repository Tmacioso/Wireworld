using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(Mathf.FloorToInt(pos.x)+0.25f, Mathf.FloorToInt(pos.y)+0.25f);
        if (Camera.main.orthographicSize > 6) GetComponent<SpriteRenderer>().enabled = false;
        else GetComponent<SpriteRenderer>().enabled = true;
        //print(Camera.main.orthographicSize);
    }
}
