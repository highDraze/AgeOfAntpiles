using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour {

    public GameData gameData;

    public BlockState state;
    public Position position;
    public float destroyTime;
    public float timer;
    public GameObject airblock;

    public enum BlockState
    {
        idle,
        gettingDestroyed
    }


	// Use this for initialization
	void Start () {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();

        state = BlockState.idle;
        switch (gameObject.tag)
        {
            case "Dirt": destroyTime = 2f;
                break;
            case "Stone": destroyTime = 5f;
                break;
        }
        destroyTime = 2f;
        timer = destroyTime;
        position = new Position(5, 4);
    }
	
	// Update is called once per frame
	void Update () {
        if (state == BlockState.gettingDestroyed)
        {
            print(timer);
            timer -= 1 * Time.deltaTime;
        }

        if (timer <= 0)
        {
            gameData.blocks[position.y, position.x] = Instantiate(airblock, this.transform);
            gameData.blockInfos[position.y, position.x].material = 'n';
            gameData.blockInfos[position.y, position.x].antId = 0;
            Destroy(this.gameObject);
        }
	}
}
