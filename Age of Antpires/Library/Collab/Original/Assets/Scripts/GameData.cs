using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameData : NetworkBehaviour {
    public static GameData Instance = null;

    public const int HEIGHT = 22;
    public const int WIDTH = 49;
    public const int MAX_ANTS = 100;

    public GameObject[] ants0 = new GameObject[MAX_ANTS];
    public GameObject[] ants1 = new GameObject[MAX_ANTS];
    public int ants0Count = 1;
    public int ants1Count = 1;


    public BlockInfo[,] blockInfos = new BlockInfo[HEIGHT, WIDTH];
    public GameObject[,] blocks = new GameObject[HEIGHT, WIDTH];

    public GameObject selectedObjectUI;

    public PlayerStats player1Stats;
    public PlayerStats player2Stats;

    // Blocktypes
    public GameObject air;      // 'n'
    public GameObject earth;    // 'e'
    public GameObject stone;    // 's'
    public GameObject granite;  // 'g'
    public GameObject food;     // 'f'
    public GameObject water;    // 'w'

    // Anttypes
    public GameObject ant0Prefab;
    public GameObject ant1Prefab;
    public GameObject queen;

    /*private void Start() {
        initField();
        test();
        fillWithAir();
        testAnt();
        //testAlgo();
    }*/

    // DEBUG
    /*
    [ClientRpc]
    void RpcSetCommandElementPlayer(GameObject myGameData) {
        if (isServer) return;
        blockInfos = myGameData.GetComponent<GameData>().blockInfos;
        Debug.Log("I got an update!");
    }*/

    void Update() {
        //if (isServer)
        // Debug.Log("started this function");
        // RpcSetCommandElementPlayer(gameObject);
        printPositionOfAnts();
    }

    /*void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }*/

    public override void OnStartServer()
    {
        
        initField();     
        // Player 0  
        createNest(5, 5, 0);
        // Player 1  
        createNest(5, 35, 1);
        createRiver(false);
        player1Stats = new PlayerStats();
        player2Stats = new PlayerStats();
  

        //littleLabyrinth();
        //spawnAnt(2, 1, 0);

    }


    public void initField() {
        // First layer is air
        for (int j = 0; j < WIDTH; j++) {
            spawnObject(0, j);
        }
        // Earth
        for (int i = 1; i < HEIGHT; i++) {          
            for (int j = 0; j < WIDTH; j++)
            {
                spawnObject(i, j, 'e');
            }                       
        }

        // Granite on border
        for (int j = 0; j < WIDTH; j++) {
            spawnObject(HEIGHT - 1, j, 'g');
        }
        for (int i = 1; i < HEIGHT; i++) {
            spawnObject(i, 0, 'g');
        }
        for (int i = 1; i < HEIGHT; i++) {
            spawnObject(i, WIDTH - 1, 'g');
        }
    }

    public void littleLabyrinth() {
        
        spawnObject(1, 1);
        spawnObject(1, 2);
        spawnObject(1, 3);
        spawnObject(2, 3);
        spawnObject(3, 3);
        spawnObject(2, 1);
        spawnObject(3, 1);
        spawnObject(4, 1);
        spawnObject(5, 1);
        spawnObject(5, 2);
        spawnObject(5, 3);
        spawnObject(5, 4);
        spawnObject(5, 5);
        spawnObject(5, 6);
        spawnObject(5, 7);
        spawnObject(3, 4);
        spawnObject(3, 5);
        spawnObject(2, 5);
        spawnObject(1, 5);
        spawnObject(1, 6);
        spawnObject(4, 7);
        spawnObject(3, 7);
        spawnObject(2, 7);
        spawnObject(1, 7);
        spawnObject(4, 3);
        spawnObject(4, 4);
    }

    public void createNest(int y, int x, int player) {
        int OFFSET_Y = 5;
        int OFFSET_X = 8;
        // Fill with air
        for (int i = y; i < y + OFFSET_Y; i++) {
            for (int j = x; j < x + OFFSET_X; j++) {
                spawnObject(i, j);
            }
        }
        // Queen
        if (player == 0) {
            ants0[1] = Instantiate(queen, new Vector3(x + OFFSET_X / 2, -y - OFFSET_Y / 2, 0), new Quaternion());
            ants0[1].GetComponent<Ant>().gameData = this;
            ants0[1].GetComponent<Ant>().ID = 1;
            blockInfos[y + OFFSET_Y / 2, x + OFFSET_X / 2].antId = 1;
            NetworkServer.Spawn(ants0[1]);
            // Two more ants
            spawnAnt(y + OFFSET_Y/2 + 1, x + OFFSET_X/2 + 1, 0);
            spawnAnt(y + OFFSET_Y/2 + 1, x + OFFSET_X/2 - 1, 0);
        }
        else {
            ants1[1] = Instantiate(queen, new Vector3(x + OFFSET_X / 2, -y - OFFSET_Y / 2, 0), new Quaternion());
            ants1[1].GetComponent<Ant>().gameData = this;
            ants1[1].GetComponent<Ant>().ID = -1;
            blockInfos[y + OFFSET_Y / 2, x + OFFSET_X / 2].antId = -1;
            NetworkServer.Spawn(ants1[1]);
            // Two more ants
            spawnAnt(y + OFFSET_Y / 2 + 1, x + OFFSET_X / 2 + 1, 1);
            spawnAnt(y + OFFSET_Y / 2 + 1, x + OFFSET_X / 2 - 1, 1);
        }


    }

    public void createRiver(bool large = true) {
        int MID = WIDTH / 2;
        if (!large) {            
            // Water
            for (int i = 1; i < 4; i++) {
                for (int j = MID - 4; j < MID + 5; j++) {
                    spawnObject(i, j, 'w');
                }
            }
            // Back to earth
            spawnObject(3, MID - 4, 'e');
            spawnObject(3, MID - 3, 'e');
            spawnObject(3, MID + 3, 'e');
            spawnObject(3, MID + 4, 'e');
            // Island
            spawnObject(1, MID - 1, 'e');
            spawnObject(1, MID    , 'e');
            spawnObject(1, MID + 1, 'e');
        }
        else {
            // Water
            for (int i = 1; i < 6; i++) {
                for (int j = MID - 6; j < MID + 7; j++) {
                    spawnObject(i, j, 'w');
                }
            }
            // Back to earth
            spawnObject(4, MID - 6, 'e');
            spawnObject(4, MID + 6, 'e');
            spawnObject(5, MID - 6, 'e');
            spawnObject(5, MID + 6, 'e');
            spawnObject(5, MID - 5, 'e');
            spawnObject(5, MID + 5, 'e');
            // Island
            spawnObject(1, MID - 1, 'e');
            spawnObject(1, MID, 'e');
            spawnObject(1, MID + 1, 'e');
        }
    }

    // For testing
    public void printPositionOfAnts() {
        for(int i = 0; i < HEIGHT; i++) {
            for(int j = 0; j < WIDTH; j++) {
                if(blockInfos[i,j].antId != 0) {
                   // print("Ant "+ blockInfos[i, j].antId + "is on position: X: " + j + ", Y: " + i);
                }
            }
        }
    }


    #region Networking Tools
    public void spawnObject(int y, int x, char newType)
    {
        GameObject type = getPrefabType(newType);

        // No Object yet created
        if(blocks[y, x] == null)
        {
            //Creation and Spawn
            blockInfos[y, x] = new BlockInfo(newType);
            GameObject blockX = Instantiate(type, new Vector3(x, -y, 0), Quaternion.identity);
            // Only for water
            if (newType == 'w') {
                blockX.GetComponent<WaterBehaviour>().gameData = this;
            }
            blocks[y, x] = blockX;
            //************
            NetworkServer.Spawn(blockX);
            return;

        }
        // No change needed
        if (blockInfos[y, x].material == newType) return;
        
        // Change object
        // Destroy current object
        NetworkServer.UnSpawn(blocks[y, x]);
        Destroy(blocks[y, x]);

        //Creation and Spawn
        blockInfos[y, x].material = newType;
        GameObject block = Instantiate(type, new Vector3(x, -y, 0), Quaternion.identity);
        // Only for water
        if (newType == 'w') {
            block.GetComponent<WaterBehaviour>().gameData = this;
        }
        blocks[y, x] = block;
        //************
        NetworkServer.Spawn(block);

    }

    //To fill the blocks with air
    public void spawnObject(int y, int x)
    {
        spawnObject(y, x, 'n');

    }

    // returns true, if ant was instantiated
    public bool spawnAnt(int y, int x, int player) {
        if (blockInfos[y, x].material != 'n' || blockInfos[y, x].antId != 0) return false;

        for(int i = 1; i < MAX_ANTS; i++) {
            if(player == 0 && ants0[i] == null) {
                GameObject newAnt = Instantiate(ant0Prefab, new Vector3(x, -y, 0), new Quaternion());
                newAnt.GetComponent<Ant>().gameData = this;
                newAnt.GetComponent<Ant>().ID = i;
                blockInfos[y, x].antId = i;
                // For testing fast ants
                newAnt.GetComponent<Ant>().speed = 5;
                newAnt.GetComponent<Ant>().workSpeed = 5;
                NetworkServer.Spawn(newAnt);
                ants0[i] = newAnt;

                // Increase Counter
                ants0Count++;
                return true;
            }
            if (player == 1 && ants1[i] == null) {
                GameObject newAnt = Instantiate(ant1Prefab, new Vector3(x, -y, 0), new Quaternion());
                newAnt.GetComponent<Ant>().gameData = this;
                newAnt.GetComponent<Ant>().ID = -i;
                blockInfos[y, x].antId = -i;
                // For testing fast ants
                newAnt.GetComponent<Ant>().speed = 5;
                newAnt.GetComponent<Ant>().workSpeed = 5;
                NetworkServer.Spawn(newAnt);
                ants1[i] = newAnt;

                // Increase Counter
                ants1Count++;
                return true;
            }
        }

        return false;
    }

    public void killAnt(int antId) {
        print("KILL PLEASE");
        if (antId >= MAX_ANTS || antId <= -MAX_ANTS || antId == 0) return;
        
        if (antId > 0) {
            if (ants0[antId] == null) return;


            NetworkServer.UnSpawn(ants0[antId]);
            Destroy(ants0[antId]);
            ants0[antId] = null;
            ants0Count--;
        }
        if (antId < 0) {
            if (ants1[-antId] == null) return;
            NetworkServer.UnSpawn(ants1[-antId]);
            Destroy(ants1[-antId]);
            ants1[-antId] = null;
            ants1Count--;
        }
    }

    GameObject getPrefabType(char type)
    {
        switch(type)
        {
            case 'n': return air;
            case 's': return stone;
            case 'g': return granite;
            case 'e': return earth;
            case 'f': return food;
            case 'w': return water;
            // Not good
            default: return air;

        }
    }
    #endregion

    public class PlayerStats
    {
        public PlayerStats()
        {

        }

        int ants;
        int food;
        int dirt;
        int stone;

        [ClientRpc]
        public void RpcIncStats(char c)
        {
            switch (c)
            {
                case 'e':
                    dirt++;
                    return;
                case 's':
                    stone++;
                    return;
                case 'a':
                    ants++;
                    return;
                case 'f':
                    food++;
                    return;
                default:
                    print("ERROR: cant add element");
                    return;
            }
        }

        [ClientRpc]
        public void RpcDecStats(char c, int amt)
        {
            switch (c)
            {
                case 'e':
                    dirt-= amt;
                    return;
                case 's':
                    stone-= amt;
                    return;
                case 'a':
                    ants-= amt;
                    return;
                case 'f':
                    food-= amt;
                    return;
                default:
                    print("ERROR: cant add element");
                    return;
            }
        }

        public int getCount(char c)
        {
            switch (c)
            {
                case 'e':
                    return dirt;
                case 's':
                    return stone;
                case 'f':
                    return food;
                case 'a':
                    return ants;
                default:
                    print("ERROR: Invalide type");
                    return 0;
            }
        }
    } 
}

public struct BlockInfo {
    public char material;
    public int antId;

    public bool reserved; // Only needed for init map generation
    public BlockInfo(char material) {
        this.material = material;
        antId = 0;
        reserved = false;
    }
}
