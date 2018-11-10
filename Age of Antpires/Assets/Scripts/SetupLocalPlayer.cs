using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {
    [SyncVar]
    int playerNumber;

    public override void OnStartClient()
    {
        Debug.Log("You are player " + playerNumber++);
    }

    // Use this for initialization
    void Start () {
        /*if (isLocalPlayer)
        {
            GetComponent<Ignore>().enabled = true;
        }
        if (isServer) GetComponent<Renderer>().material = playerColors[0];
        else GetComponent<Renderer>().material = playerColors[1];
        */
    }
    
}
