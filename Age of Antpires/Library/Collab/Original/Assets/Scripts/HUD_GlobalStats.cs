using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class HUD_GlobalStats : NetworkBehaviour {
  /*  [SyncVar]
    int ants;
    [SyncVar]
    int food;
    [SyncVar]
    int dirt;
    [SyncVar]
    int stone;*/

    Text mystats;

    public GameData gameData;
    [SerializeField]
    int player;
    PlayerStats stats;

    void Start()
    {

        //gameData = this.GetComponentInParent<Canvas>().GetComponentInChildren<InGame_UI_Manager>().gameData;
        if (isServer) player = 0;
        else player = 1;
        print("Player " + player);
        //if (!isServer) return;
        mystats = gameObject.GetComponent<Text>();
        CmdGetStats(player);
        mystats.text = "Ants: " + stats.ants + "\nFood: " + stats.food + "\nDirt: " + stats.dirt + "\nStone: " + stats.stone;
        //mystats.text = "Ants: " + ants + "\nFood: " + food + "\nDirt: " + dirt + "\nStone: " + stone;
    }

    [Command]
    void CmdGetStats(int playerid)
    {
        stats =  gameData.getStats(player);
       // print(player);
        /*
        if (player == 0)
        {
            ants = gameData.GetComponent<GameData>().player1Stats.getCount('a');
            food = gameData.GetComponent<GameData>().player1Stats.getCount('f');
            dirt = gameData.GetComponent<GameData>().player1Stats.getCount('e');
            stone = gameData.GetComponent<GameData>().player1Stats.getCount('s');            
        }
        else
        {          
            ants = gameData.GetComponent<GameData>().player2Stats.getCount('a');
            food = gameData.GetComponent<GameData>().player2Stats.getCount('f');
            dirt = gameData.GetComponent<GameData>().player2Stats.getCount('e');
            stone = gameData.GetComponent<GameData>().player2Stats.getCount('s');
        }   */
    }

 
	void Update () {
        // ants = GameData.[myants];
        // food = GameData.[myfood];
        // dirt = GameData.[mydirt];
        // stone = GameData.[mystone];
        //if (!isServer) return;
        CmdGetStats(player);


        mystats.text = "Ants: " + stats.ants + "\nFood: " + stats.food + "\nDirt: " + stats.dirt + "\nStone: " + stats.stone;
        //mystats.text = "Ants: " + ants + "\nFood: " + food + "\nDirt: " + dirt + "\nStone: " + stone;
    }
}
