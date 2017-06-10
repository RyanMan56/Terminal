using UnityEngine;
using System.Collections;

public class TerminalButton : MonoBehaviour {
    private Vector3 offPosition, onPosition, depressedPosition; // Not that depressed :(
    private bool done = true, off = true;
    private Collider coll, imageColl;
    //private GameObject terminal;
    private TerminalScript terminalScript;
    private bool shouldTurnOff, shouldTurnOn;
    private float speed = 0.0009f, initSpeed, acceleration = 0.00009f;

	// Use this for initialization
	void Start () {
        coll = GetComponent<Collider>();
        imageColl = transform.GetChild(0).GetComponent<Collider>();
        //terminal = GameObject.Find("Terminal");
        terminalScript = GetComponentInParent<TerminalScript>(); //terminal.GetComponent<TerminalScript>();
        offPosition = transform.localPosition;
        offPosition.z = -0.7793f;
        onPosition = transform.localPosition;
        onPosition.z = -0.5742f;
        depressedPosition = transform.localPosition;
        depressedPosition.z = -0.5195f;
        initSpeed = speed;
        if (terminalScript.terminalOn)
        {
            transform.localPosition = onPosition;
            off = false;
        }
        else
        {
            transform.localPosition = offPosition;
            off = true;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!done)
            HandlePresses();
        else
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width - 1) / 2, (Screen.height - 1) / 2));
                RaycastHit hit;
                bool buttonPressed = Physics.Raycast(ray, out hit, 1.5f);
                if (buttonPressed && (hit.collider.Equals(coll) || hit.collider.Equals(imageColl)))
                {
                    if (terminalScript.terminalOn)
                    {
                        shouldTurnOff = true;
                        done = false;
                    }
                    else
                    {
                        shouldTurnOn = true;
                        done = false;
                    }
                }
            }
        }
	}

    void HandlePresses()
    {
        {
            if (shouldTurnOff)
                TurnOff();
            if (shouldTurnOn)
                TurnOn();
        }
    }

    void TurnOn()
    {
        transform.position += new Vector3(0, 0, speed);
        if (off)
        {
            if (transform.localPosition.z >= onPosition.z)
                off = false;
        }
        else
        {
            speed -= acceleration;
            if(transform.localPosition.z <= onPosition.z)
            {
                shouldTurnOn = false;
                done = true;
                speed = initSpeed;
                terminalScript.terminalOn = true;
            }
        }
    }

    void TurnOff() {
        Debug.Log(off);
        transform.position += new Vector3(0, 0, speed);
        if (off)
        {
            if (transform.localPosition.z <= offPosition.z)
            {
                Debug.Log("jnfjsdkfsdf");
                shouldTurnOff = false;
                done = true;
                speed = initSpeed;
                terminalScript.terminalOn = false;
            }
        }
        else
        {
            speed -= acceleration;
            if (transform.localPosition.z <= onPosition.z)
            {
                off = true;
            }
        }
    }
}
