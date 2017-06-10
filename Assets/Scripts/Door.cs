using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
    private Vector3 closedPos, openPos;

	// Use this for initialization
	void Start () {
        closedPos = transform.localPosition;
        openPos = new Vector3(-0.8415947f, 3.487f, -5.598108f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
