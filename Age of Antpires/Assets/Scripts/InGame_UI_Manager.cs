using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_UI_Manager : MonoBehaviour {

    //public GameData gameData;
    public GameObject mainselected;
    public static GameObject selected=null;    
    
    public GameObject[] UIOptions;

	// Use this for initialization
	/*void Awake () {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }*/
	
	// Update is called once per frame
	void Update () {
        //print(mainselected);
       // print(selected);

        if (mainselected == null)
        {
            UICloser();
            selected = mainselected;
        }
            
        if (mainselected !=null)
        {
            if (mainselected.tag == "Ant" && mainselected != selected)
            {
                selected = mainselected;
                UICloser();
                UIOptions[0].gameObject.SetActive(true);
                UIOptions[1].gameObject.SetActive(true);
               // UIOptions[6].gameObject.SetActive(true);
                if (selected.GetComponent<Ant>().workUpgraded == true)
                {                   
                    UIOptions[4].gameObject.SetActive(true);
                }
                if (selected.GetComponent<Ant>().dmgUpgraded == true || selected.GetComponent<Ant>().liveUpgraded == true)
                {
                    UIOptions[5].gameObject.SetActive(false);
                }
                else { UIOptions[5].gameObject.SetActive(true); }

            }
            if (mainselected.tag == "Queen" && mainselected != selected)
            {
                selected = mainselected;
                UICloser();
                UIOptions[0].gameObject.SetActive(true);
                UIOptions[2].gameObject.SetActive(true);
              //  UIOptions[6].gameObject.SetActive(true);
                UIOptions[5].gameObject.SetActive(false);

            }
            if (mainselected.tag == "Upgrade" && mainselected != selected)
            {
                selected = mainselected;
                UICloser();
                UIOptions[0].gameObject.SetActive(true);
                UIOptions[3].gameObject.SetActive(true);
              //  UIOptions[6].gameObject.SetActive(true);

            }
            if (mainselected == selected)
            {
                if (selected.tag=="Ant"&&selected.GetComponent<Ant>().workUpgraded == true)
                {
                    UIOptions[4].gameObject.SetActive(true);
                }
                if (selected.tag == "Ant" && selected.GetComponent<Ant>().dmgUpgraded == true || selected.tag == "Ant" && selected.GetComponent<Ant>().liveUpgraded == true||selected.tag=="Queen")
                {
                    UIOptions[5].gameObject.SetActive(false);
                }
                else { UIOptions[5].gameObject.SetActive(true); }
            }
            else
            {
                UICloser();
            }      
        }
        /*
        //if selected--> show UI options
        if (mainselected == null||Input.GetKeyDown("Escape"))
        {
            selected = null;            

        }
        if (selected == null)
        {
           // UICloser();
        }
        if (mainselected.tag == "Ant" || mainselected.tag == "Queen" || mainselected.tag == "Upgraded")
        {


            //turn on ant options
            if (mainselected != null && mainselected.tag == "Ant" && mainselected != selected)
            {
                selected = mainselected;
                UICloser();
                UIOptions[0].gameObject.SetActive(true);
                UIOptions[1].gameObject.SetActive(true);
                UIOptions[6].gameObject.SetActive(true);

            }

            //turn on queen options
            if (mainselected != null && mainselected.tag == "Queen" && mainselected != selected)
            {
                selected = mainselected;
                UICloser();
                UIOptions[0].gameObject.SetActive(true);
                UIOptions[2].gameObject.SetActive(true);
                UIOptions[6].gameObject.SetActive(true);

            }
            //turn on evolution upgrades
            if (mainselected != null && mainselected.tag == "Upgrade" && mainselected != selected)
            {
                selected = mainselected;
                UICloser();
                UIOptions[0].gameObject.SetActive(true);
                UIOptions[3].gameObject.SetActive(true);
                UIOptions[6].gameObject.SetActive(true);

            }
            if (selected != null)
            {
                //turn on tileing options
                if (selected.tag == "Ant" && selected.GetComponent<Ant>().workUpgraded == true)
                {
                    UIOptions[4].gameObject.SetActive(true);
                }

                //turn on building options
                if (selected.tag == "Ant" && selected.GetComponent<Ant>().dmgUpgraded != true || selected.GetComponent<Ant>().liveUpgraded != true)
                {
                    UIOptions[5].gameObject.SetActive(true);
                }
            }
        }

    */
    }

    void UICloser()
    {
        //selected = GameData.Instance.selectedObjectUI.gameObject;
        for (int i = 0; i < UIOptions.Length; i++)
        {
            UIOptions[i].gameObject.SetActive(false);
        }
    }
}
