using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class Ignore : NetworkBehaviour {
    public float facing;

    private Rigidbody playerRigidbody;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private GameObject minePrefab;

    
	// Use this for initialization
	void Start () {
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        facing = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Horizontal") != 0)
        {
            if (Input.GetAxis("Horizontal") < 0) facing = 1;
            else facing = -1;
            
            transform.Translate(Vector3.right * 5 * Input.GetAxis("Horizontal") * Time.deltaTime);
        }
        if(Input.GetKeyDown("space") && Grounded())
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 7);
        }
        if(transform.position.y <= -5)
        {
            transform.position = new Vector3(0, 5, 0);
        }

        if (Input.GetKeyDown("w"))
        {
            CmdFireBullet();
        }
        if (Input.GetKeyDown("e"))
        {
            Instantiate(minePrefab, transform);
        }
	}

    [Command]
    void CmdFireBullet()
    {
        GameObject instance = Instantiate(projectilePrefab, new Vector3(transform.position.x - facing, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 90)) as GameObject;
        instance.transform.parent = transform;
        NetworkServer.Spawn(instance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Projectile")
        {
            Debug.Log("Poof");
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            if (!isServer) return;
            transform.position = new Vector3(0, -10, 0);
        }
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1f);
    }
    
}
