using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameData gameData;

    bool spawnable = true;
    int playerID = 0;
    public Position position = new Position(1, 3);
	// Use this for initialization
    
    Spawner(int y, int x, int iD)
    {
        position.x = x; position.y = y; playerID = iD;
    }
    void Start () {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();

        position.x = (int)gameObject.transform.position.x;
        position.y = -(int)gameObject.transform.position.y;
        
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Spawn") && spawnable == true)
        {
            //Anzahl an Gespawnten Eiern
            for (int c = 0; c < Random.Range(2, 5); c++)
            {
                //Plaziert die Eier auf der Map
                if(this.isFree() == true)
                    gameData.spawnAnt(position.y + Random.Range(-1, 2), position.x + Random.Range(-1, 2), playerID);
            }
        }  	
	}
    bool isFree()
    {
            for(int i = -1; i <= 1 ; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    if ((gameData.blockInfos[position.y + j, position.x + i].material == 'n') &&
                        gameData.blockInfos[position.y + j, position.x + i].antId == 0) {
                        return true;
                    }
                }
            }
           return false;
    }   
}
