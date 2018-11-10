using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour {
    public GameObject Eggs;
    int player;

	// Use this for initialization
	void Start () {
        player = GetComponent<Ant>().playerID;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LayEggs()
    {
        GameObject egg = Instantiate(Eggs, this.transform.position, Quaternion.identity);
        egg.GetComponent<Eggs>().player = this.player;

    }
}
