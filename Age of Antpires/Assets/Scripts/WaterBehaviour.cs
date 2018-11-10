using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WaterBehaviour : NetworkBehaviour {
    public GameData gameData;
    int y;
    int x;
    double timer;

	// Use this for initialization
	void Start () {
        y = (int) -transform.position.y;
        x = (int) transform.position.x;
        timer = 0.8;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isServer) return;
        CmdWaterControl();
    }

    [Command]
    void CmdWaterControl()
    {
        int antId = gameData.blockInfos[y, x].antId;
        if (antId != 0)
        {
            //print("Should KILL now");
            if (antId > 0) {
                StartCoroutine(gameData.ants0[antId].GetComponent<Ant>().die());
            }
            if (antId < 0) {
                StartCoroutine(gameData.ants1[-antId].GetComponent<Ant>().die());
            }
            //gameData.killAnt(gameData.blockInfos[y, x].antId);
            //gameData.blockInfos[y, x].antId = 0;
        }

        timer -= Time.deltaTime;

        if (timer <= 0.0)
        {
            // Nach oben
            if (y > 1 && gameData.blockInfos[y - 1, x].material == 'n')
            {
                gameData.spawnObject(y - 1, x, 'w');
            }
            //Nach unten
            if (gameData.blockInfos[y + 1, x].material == 'n')
            {
                gameData.spawnObject(y + 1, x, 'w');
            }
            //Nach rechts
            if (gameData.blockInfos[y, x + 1].material == 'n')
            {
                gameData.spawnObject(y, x + 1, 'w');
            }
            //Nach links
            if (gameData.blockInfos[y, x - 1].material == 'n')
            {
                gameData.spawnObject(y, x - 1, 'w');
            }
        }
    }
}
