using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : NetworkBehaviour {
    [SerializeField]
    private GameObject earthPrefab;
    [SerializeField]
    private GameObject foodPrefab;
    [SerializeField]
    private GameObject stonePrefab;
    [SerializeField]
    private GameObject granitePrefab;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public override void OnStartServer()
    {
        //GameObject block = Instantiate(earthPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        var e = (GameObject)Instantiate(earthPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        NetworkServer.Spawn(e);
    }
}
