using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileBehaviour : NetworkBehaviour {
    [SerializeField]
    private float speed;

    private float direction;

    //[SerializeField]
    private GameObject player;

    //private Ignore playerScript;

	// Use this for initialization
	void Start () {
        player = transform.parent.gameObject;
        Ignore playerScript = player.GetComponent<Ignore>();
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            direction = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            direction = -playerScript.facing;
        }
	}
	
	// Update is called once per frame
    [ServerCallback]
	void Update () {
        if (!isLocalPlayer) return;
        Debug.Log(direction);
        transform.Translate(Vector3.down * direction * speed * Time.deltaTime);
        if(transform.position.x < -20 || transform.position.x > 20)
        {
            NetworkServer.Destroy(gameObject);
        }
	}
}
