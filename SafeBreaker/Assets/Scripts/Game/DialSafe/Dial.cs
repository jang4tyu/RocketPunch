using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dial : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        UIEventListener.Get(gameObject).onDrag = OnDragDial;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDragDial(GameObject GO, Vector2 delta)
    {
        if (-0.6f < UICamera.lastWorldPosition.y)
            transform.Rotate(new Vector3(0, 0, -delta.x / 2));
        else
            transform.Rotate(new Vector3(0, 0, delta.x / 2));

        if (0 < UICamera.lastWorldPosition.x)
            transform.Rotate(new Vector3(0, 0, delta.y / 2));
        else
            transform.Rotate(new Vector3(0, 0, -delta.y / 2));
    }       
}
