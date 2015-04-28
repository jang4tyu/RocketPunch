using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {

    public float SetTime = 0.0f;
    float CheckTime = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (CheckTime < SetTime)
            CheckTime += Time.deltaTime;

        else
            Destroy(gameObject);
	}
}
