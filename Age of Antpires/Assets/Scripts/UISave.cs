using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UISave : NetworkBehaviour {

    // Auswahl Linksklick
    public GameObject selectedObject1st = null;
    // Auswahl Rechtsklick
    public GameObject selectedObject2nd = null;

    //public GameData gameData;
    // Buttons for building stuff
    public bool wallBuildClicked = false;
    public bool dirtBuildClicked = false;
    // Button für Befehlsketten
    public bool shiftClicked = false;


    // public GameData gameData;
    // Use this for initialization
    /*void Start () {
        //gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }*/
    
    void Update() {

        /*if (Input.GetKey(KeyCode.E)) dirtBuildClicked = true;
        else dirtBuildClicked = false;

        if (Input.GetKey(KeyCode.R)) wallBuildClicked = true;
        else wallBuildClicked = false;*/

        if (selectedObject1st == null) {
            return;
        }
        /*if (selectedObject1st != null) {
            //print(selectedAnt);
            //print(selectedAnt.GetComponent<Ant>().ID);
            CmdPrintID(selectedObject1st);
        }*/

        if (selectedObject2nd == null) {
            return;
        }
        /*if(selectedObject1st != null) {
            //print("YUUHHHHHUUUU");
            //print(selectedAnt.GetComponent<Ant>().ID);
            
        }*/
        CmApply((int)selectedObject2nd.transform.position.x, -(int)selectedObject2nd.transform.position.y);
    }

    //[Command]
    void CmApply(int x, int y)
    {
        //Debug.Log("Entered PlayerID: " + player);

        Position destination = new Position(x, y);
        bool applied;
        if (!dirtBuildClicked && !wallBuildClicked)
        {
            if (!shiftClicked)
            {
                Ant testAnt = selectedObject1st.GetComponent<Ant>();
                //if (testAnt == null) print("NOOOOOOOOOOOOOOOOO");
                applied = selectedObject1st.GetComponent<Ant>().applyNewRoute(destination, (player));
                
            }
            else applied = selectedObject1st.GetComponent<Ant>().applyNewRoute(destination, player, false);
        }
        else
        {
            if (!shiftClicked)
            {
                applied = selectedObject1st.GetComponent<Ant>().applyNewRoute(destination, player, true, true, (dirtBuildClicked ? 'e' : 's'));
            }
            else applied = selectedObject1st.GetComponent<Ant>().applyNewRoute(destination, player, false, true, (dirtBuildClicked ? 'e' : 's'));
        }
        if (applied)
        {
            selectedObject2nd = null;
        }
        else
        {
            selectedObject2nd = null;
            print("could not apply move!");
        }
    }

    

    /*[Command]
    void CmdPrintID(GameObject g)
    {
        //print(g.GetComponent<Ant>().ID);
    }*/

    #region PlayerDifferenciation
    [SyncVar]
    int playerNumber;

    
    public int player;

    void Start()
    {
        if (isServer)
        {
            if (isLocalPlayer)
            { 
                print("server, local");
                player = 0;
            }
            else
            {
                print("server, not local");
                player = 1;
            }
        }
        else
        {
            if(isLocalPlayer)
            {
                print("not server, local");
                player = 1;
            }
            else
            {
                print("not server, not local");
                player = 0;
            }
        }
    }

    #endregion
}
