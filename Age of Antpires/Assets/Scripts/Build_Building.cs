using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Build_Building : NetworkBehaviour {

    public GameObject Build_Storage;
    public GameObject Build_Upgrade;
    public GameObject Build_Egg;
    public Ant ant;
    public GameData gameData;

    private void Start()
    {
        gameData = ant.gameData;
    }

    [Command]
    void CmdSetGameData()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }


    public void placeStorage()
    {
        CmdPlaceBuilding(ant.position.x, ant.position.y, 'x');
        Debug.Log("Storage gebaut");
        //Instantiate(Build_Storage, this.transform.position, Quaternion.identity);

    }
    public void placeUpgrade()
    {
        CmdPlaceBuilding(ant.position.x, ant.position.y, 'y');
        Debug.Log("Upgrade gebaut");
        //Instantiate(Build_Upgrade, this.transform.position, Quaternion.identity);

    }
    public void placeEgg()
    {
        CmdPlaceBuilding(ant.position.x, ant.position.y, 'z');
        Debug.Log("Haus gebaut");
        //Instantiate(Build_Egg, this.transform.position, Quaternion.identity);

    }

    [Command]
    void CmdPlaceBuilding(int x, int y, char type)
    {
        //GameObject building;
        gameData.spawnObject(y, x, type);
       /* switch (type)
        {
            case 'x':

                
               // building = Instantiate(Build_Storage, this.transform.position, Quaternion.identity);
                //NetworkServer.Spawn(building);
                return;
            case 'y':
               // building = Instantiate(Build_Upgrade, this.transform.position, Quaternion.identity);
                //NetworkServer.Spawn(building);
                return;
            case 'z':
                //building = Instantiate(Build_Egg, this.transform.position, Quaternion.identity);
                //NetworkServer.Spawn(building);
                return;
            default:
                print("ERROR: cant place building");
                return;
        }*/
    }
}
