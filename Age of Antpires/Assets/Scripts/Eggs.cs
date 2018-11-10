using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Eggs : NetworkBehaviour {

    public GameData gameData;
    public int player;

    public float timer;

	// Use this for initialization
	void Start () {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }
	
	// Update is called once per frame
	void Update () {
        timer -= 1*Time.deltaTime;

        if (timer <= 0)
        { 
            if (player != 0)
                CmdSpawn();
            else gameData.spawnAnt(-(int)transform.position.y, (int)transform.position.x, 0);
            Destroy(gameObject);
        }
	}

    [Command]
    void CmdSpawn()
    {
        print("halo");
        gameData.spawnAnt(-(int)transform.position.y, (int)transform.position.x, 1);
    }
}
