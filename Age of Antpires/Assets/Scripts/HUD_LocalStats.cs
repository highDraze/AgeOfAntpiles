using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD_LocalStats : MonoBehaviour {
    int hp;
    int atk;
    int def;
    int spd;
    int wrk;
    string type;

    Text objStats;

	void Start ()
    {
        objStats = gameObject.GetComponent<Text>();
        objStats.text = "Type: " + type + "\nHP: " + hp + "\nATK: " + atk + "\nSPD: " + spd + "\nWrkSPD: " + wrk;
    }

	void Update ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if(hit.transform.gameObject.tag == "Ant" || hit.transform.gameObject.tag == "Queen")
            {
                hp = hit.transform.gameObject.GetComponent<Ant>().lives;
                atk = hit.transform.gameObject.GetComponent<Ant>().dmg;
                spd = hit.transform.gameObject.GetComponent<Ant>().speed;
                wrk = hit.transform.gameObject.GetComponent<Ant>().workSpeed;
                type = hit.transform.gameObject.tag;
                objStats.text =  "Type: " + type + "\nHP: " + hp + "\nATK: " + atk + "\nSPD: " + spd + "\nWrkSPD: " + wrk;
            }
            else
            {
                type = hit.transform.gameObject.tag;
                objStats.text = "Type: " + type;
            }
        }        
    }
}
