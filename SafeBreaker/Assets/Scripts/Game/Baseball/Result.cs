using UnityEngine;
using System.Collections;

public class Result : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Next()
    {
        SceneManager.instance.ActionEvent(eAction.NEXT);
        Destroy(gameObject);
    }
}
