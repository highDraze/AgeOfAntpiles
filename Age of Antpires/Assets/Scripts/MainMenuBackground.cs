using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBackground : MonoBehaviour {
    public GameObject stone, earth, granit, water, food, air;

	// Use this for initialization
	void Start () {
         for (int j = -11; j <= 11; j++)
            {
            for(int i = 2; i >= -5; i--)
            {
                if (j == -11 || j == 11 || i == -5)
                {
                    Instantiate(granit, new Vector3(j, i, 0), Quaternion.identity);
                }
                else if (i == 2 && j > -7 && j < -3)
                {
                    Instantiate(water, new Vector3(j, i, 0), Quaternion.identity);
                }
                else if (i==0||i==-1||i==-2)
                {
                    Instantiate(air, new Vector3(j, i, 0), Quaternion.identity);
                }               
                else
                {
                    Instantiate(earth, new Vector3(j, i, 0), Quaternion.identity);
                }
            }
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
