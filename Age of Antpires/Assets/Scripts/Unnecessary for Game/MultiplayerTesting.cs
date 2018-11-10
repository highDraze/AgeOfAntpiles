using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerTesting : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && isLocalPlayer)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                CmdTranslate(hit.transform.gameObject);
            }
        }
	}

    [Command]
    void CmdTranslate(GameObject g)
    {
        g.transform.Translate(1, 1, 1);
    }
}
