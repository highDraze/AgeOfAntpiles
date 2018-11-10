using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttonbehaviours : MonoBehaviour {

   // public GameData gameData;




    // Use this for initialization
    void Awake () {
       // gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void antupgrades(string upgrades)
    {
        InGame_UI_Manager.selected.GetComponent<Ant>().Upgrade(upgrades.ToCharArray()[0]);   
    }

    public void print()
    {
        print("test");
    }

    public void Eggs()
    {
        InGame_UI_Manager.selected.GetComponent<Queen>().LayEggs();
    }

    public void PlaceDirtblock()
    {
       // UISave.dirtBuildClicked = true;
    }
    //building
    public void buildstorage()
    {
        InGame_UI_Manager.selected.GetComponent<Build_Building>().placeStorage();
    }
    public void buildegg()
    {
        InGame_UI_Manager.selected.GetComponent<Build_Building>().placeEgg();
    }
    public void buildupgrade()
    {
        InGame_UI_Manager.selected.GetComponent<Build_Building>().placeUpgrade();
    }
    public void disconnect()
    {
        Network.Disconnect();
        MasterServer.UnregisterHost();
    }

}
