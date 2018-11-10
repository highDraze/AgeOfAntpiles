using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Ant : NetworkBehaviour {
    #region Declaration
    public GameData gameData;


    public bool isQueen;
    public Queue<Position> moveList = new Queue<Position>();
    //  public Queue<Position> orderListTest = new Queue<Position>();
    public Queue<Order> orderList = new Queue<Order>();

    [SyncVar]
    public int lives;
    public int dmg;
    public int speed;
    public int workSpeed;
    public int ID;
    public int playerID;
    private bool goingToBuild = false;
    private char objToBuild;
    public Position position;
    private Position oldPosition;
    public AntState state;
    private Pathfinder path;

    Animator[] anim;
    private Animator antanim;
    // Work Upgrade
    private Animator antanimW;
    // DMG Upgrade
    private Animator antanimD;
    // Lives Upgrade
    private Animator antanimL;
    // Speed Upgrade
    private Animator antanimS;

    private Animator antanimTOT;

    private Animator animQ;
    [HideInInspector]
    public bool speedUpgraded = false, dmgUpgraded = false, workUpgraded = false, liveUpgraded = false; 

    [SerializeField]
    private float workTimer;
    private float autoHitCooldown;
    private float dyingTimer;
    private Position actualDestination;

    #endregion
    public enum AntState
    {
        waiting,
        moving,
        working,
        fighting,
        blocked,
        dying
    }

    // Use this for initialization
    void Start()
    {
        // gameData = GameObject.Find("GameData").GetComponent<GameData>();
        speedUpgraded = false; dmgUpgraded = false; workUpgraded = false; liveUpgraded = false;
        state = AntState.waiting;
        if (!isQueen)
        {
            Animator[] anim = this.GetComponentsInChildren<Animator>();
            antanim = anim[0];
            antanimW = anim[1];
            antanimD = anim[2];
            antanimL = anim[3];
            antanimS = anim[4];
            antanimTOT = anim[5];
            antanim.SetBool("Running", false);
        }
        else if (isQueen)
        {
            animQ = this.GetComponent<Animator>();
            animQ.SetBool("Running", false);
        }
        // Assigning correct player id
        playerID = (ID > 0 ? 0 : 1);
        //speed = 1;
        //workSpeed = 1;
        if (!isQueen) { 
        lives = 10;
        dmg = 5;
        workTimer = 2f;
        autoHitCooldown = 0;
        dyingTimer = 1f;
        position = new Position((int)transform.position.x, -(int)transform.position.y);
        oldPosition = new Position((int)transform.position.x, -(int)transform.position.y);
        }
        if (isQueen)
        {
            lives = 200;
            dmg = 3;
            workTimer = 2f;
            autoHitCooldown = 0;
            dyingTimer = 1f;
            position = new Position((int)transform.position.x, -(int)transform.position.y);
            oldPosition = new Position((int)transform.position.x, -(int)transform.position.y);
        }

       
        // DEBUGGING
        /*double timer = 0.3;
        while (timer >= 0.0) timer -= Time.deltaTime;
        print("My ID is: " + ID);*/
        // DEBUGGING ENDE
        

    }

    // Update is called once per frame
    void Update() {
        if (state != AntState.dying)
        {

            if (lives <= 0)
            {
                StartCoroutine(die());
                return;
            }

            if (!isQueen)
            {
                if (state != AntState.waiting)
                {
                    antanim.SetBool("Running", true);
                }
                else
                {
                    antanim.SetBool("Running", false);
                }
                antanimD.SetBool("Running", antanim.GetBool("Running"));
                antanimL.SetBool("Running", antanim.GetBool("Running"));
                antanimS.SetBool("Running", antanim.GetBool("Running"));
                antanimW.SetBool("Running", antanim.GetBool("Running"));
                antanimW.SetBool("Mining", state == AntState.working);
            }
            else if (isQueen)
            {
                if (state != AntState.waiting)
                {
                    animQ.SetBool("Running", true);
                }
                else
                {
                    animQ.SetBool("Running", false);
                }
            }

            if (moveList.Count > 0) CmdMove();
            else if (orderList.Count > 0)
            {
               
                if (!orderList.Peek().build)
                    applyNewRoute(orderList.Dequeue().target, playerID);
                else applyNewRoute(orderList.Peek().target, playerID, true, true, orderList.Dequeue().type);
            }
        }
	}

    [Command]
    private void CmdMove()
    {
       // antanim.SetBool("Running", true);
        if (moveList.Peek().ignore)
        {
            moveList.Dequeue();
        }
        Position dest = moveList.Peek();
        // DEBUG FOR STRUCT
        // GameData.BlockInfo destBlock = GameData.Instance.blockInfos[dest.y, dest.x];
        BlockInfo destBlock = gameData.blockInfos[dest.y, dest.x];
        bool free = true;

        //check material, wenn stein(e)/erde bau ab 
        switch (destBlock.material)
        {
            case 's':
            case 'f':
            case 'e': if (moveList.Count == 1) work(dest);
                free = false;
                break;
        }

        if (free && (destBlock.antId != 0 && destBlock.antId != this.ID))
        {
            if ((destBlock.antId > 0) != (this.ID > 0)) fight(destBlock.antId);
            else autoHitCooldown = 2f;
            free = false;
        }
        if (free)
        {
            if (moveList.Count == 1 && goingToBuild)
            {
                build(objToBuild, moveList.Peek());
            }
            else
            {
                if (state != AntState.moving) state = AntState.moving;
                gameData.blockInfos[position.y, position.x].antId = 0;
                gameData.blockInfos[dest.y, dest.x].antId = this.ID;
                position.x = dest.x;
                position.y = dest.y;
                Vector3 dir = new Vector3(position.x - oldPosition.x, -(position.y - oldPosition.y), 0);
                int zrot = 0;
                if (dir.x == 0)
                {
                    if (dir.y == 1) zrot = 0;
                    else zrot = 180;
                }
                else if (dir.x == 1)
                {
                    if (dir.y == 1) zrot = -45;
                    else if (dir.y == -1) zrot = -135;
                    else zrot = -90;
                }
                else
                {
                    if (dir.y == 1) zrot = 45;
                    else if (dir.y == -1) zrot = 135;
                    else zrot = 90;
                }

                transform.rotation = Quaternion.Euler(new Vector3(0, 0, zrot));
                float dist = (transform.position - new Vector3(dest.x, -dest.y, 0)).magnitude;
                float move = speed * Time.deltaTime;
                if (move > dist) move = dist;
                transform.Translate(Vector3.up * move);
                if (dist <= 0.01)
                {
                    oldPosition.x = dest.x;
                    oldPosition.y = dest.y;
                    moveList.Dequeue();
                    if (moveList.Count == 0)
                    {
                        state = AntState.waiting;
                    }
                }
            }
        }
    }

    private void work(Position dest)
    {
        //antanim.SetBool("Running", true);
        if (this.state != AntState.working) { this.state = AntState.working; }

        if (gameData.blockInfos[dest.y, dest.x].material == 'e') {
            workTimer -= workSpeed * Time.deltaTime;
        }
        else if (gameData.blockInfos[dest.y, dest.x].material == 's') {
            workTimer -= workSpeed * Time.deltaTime * 0.5f;
        }
        else if (gameData.blockInfos[dest.y, dest.x].material == 'f') {
            workTimer -= workSpeed * Time.deltaTime * 1.5f;
        }

        if (workTimer <= 0)
        {
            //print("1.: " + playerID);
            CmdSetStats(playerID, gameData.blockInfos[dest.y, dest.x].material, 1);
            if (ID > 0)
            {
                
                //gameData.player1Stats.RpcIncStats(gameData.blockInfos[dest.y, dest.x].material);
            }
            else
            {
               // gameData.player2Stats.RpcIncStats(gameData.blockInfos[dest.y, dest.x].material);
            }
            gameData.spawnObject(dest.y, dest.x);
            workTimer = 2f;
           // Upgrade('s');
            state = AntState.moving;
        }
    }

    public bool applyNewRoute(Position destination, int player, bool newDestination = true, bool build = false, char type = 'e') {
        // Wrong player tries to access enemy ant
        if (player == 0 && ID < 0 || player == 1 && ID > 0) return false;

        path = new Pathfinder(gameData);
        Queue<Position> queueToCheck;
        /*
        GameData test = GameData.Instance;
        print(destination.x + " " + destination.y);
        //DEBUG FOR STRUCT GameData. removed
        BlockInfo[,] test2 = GameData.Instance.blockInfos;
        BlockInfo test3 = GameData.Instance.blockInfos[destination.y, destination.x];
        char test4 = GameData.Instance.blockInfos[destination.y, destination.x].material;
        */
        if (!newDestination)
        {
            orderList.Enqueue(new Order(destination, build, type));
            return true;
        }
        if (!build)
        {
            if (gameData.blockInfos[destination.y, destination.x].material == 'n')
            {
                //  if (newDestination)
                queueToCheck = path.pathfindingAlgo(position, destination);
                // else queueToCheck = path.pathfindingAlgo(actualDestination, destination);
            }
            else
            {
                //  if (newDestination)
                queueToCheck = path.pathfindingAlgo(position, destination, true);
                // else queueToCheck = path.pathfindingAlgo(actualDestination, destination, true);
            }
           
        }
        else
        {
            if (gameData.blockInfos[destination.y, destination.x].material == 'n')
            {
                //  if (newDestination)
                queueToCheck = path.pathfindingAlgo(position, destination);
                objToBuild = type;
                goingToBuild = true;
               
                // else queueToCheck = path.pathfindingAlgo(actualDestination, destination);
            }
            else return false;
        }
        if (queueToCheck.Count == 0) return false;
        // Apply new queue
        //actualDestination = destination;
        // if (newDestination)
        //  {
        moveList = queueToCheck;
        workTimer = 2f;
        //  }
        /*  else
          {
              while (queueToCheck.Count > 0)
              {
                  moveList.Enqueue(queueToCheck.Dequeue());
              }
          }*/
        return true;
    }

    public bool Upgrade(char type)
    {
        switch (type)
        {
            case 'w':
                Debug.Log("WorkUpgrade Archived");
                if (!workUpgraded && (!dmgUpgraded || !liveUpgraded))
                {
                    workSpeed *= 2;
                    workUpgraded = true;
                    antanimW.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    return true;
                }
                else return false;
            case 'd':
                Debug.Log("DmgUpgrade Archived");
                if (!dmgUpgraded && !workUpgraded)
                {
                    dmg *= 2;
                    dmgUpgraded = true;
                    antanimD.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    return true;
                }
                else return false;
            case 's':
                Debug.Log("SpeedUpgrade Archived");
                if (!speedUpgraded)
                {
                    speed *= 2;
                    speedUpgraded = true;
                    antanimS.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    return true;
                }
                else return false;
            case 'l':
                Debug.Log("LifeUpgrade Archived");
                if (!liveUpgraded && !workUpgraded)
                {
                    lives *= 2;
                    liveUpgraded = true;
                    antanimL.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    return true;
                }
                else return false;
        }
        return false;
    }

    void build(char type, Position dest)
    {
        //antanim.SetBool("Running", true);
        if (ID > 0) {
            if (gameData.player1Stats.getCount(type) <= 2)
            {
                moveList.Dequeue();
                return;
            }
        }
        else
        {
            if (gameData.player2Stats.getCount(type) <= 2)
            {
                moveList.Dequeue();
                return;
            }
        }
        if (state != AntState.working) state = AntState.working;

        if (gameData.blockInfos[dest.y, dest.x].material == 'n')
            workTimer -= workSpeed * Time.deltaTime;
        else
        {
            state = AntState.waiting;
            return;
        }

        if (workTimer <= 0)
        {
           // CmdSetStats(playerID, gameData.blockInfos[dest.y, dest.x].material, -3);
            /*if(ID > 0)
            {
               // gameData.player1Stats.RpcDecStats(type, 3);
            }
            else
            {
              //  gameData.player2Stats.RpcDecStats(type, 3);
            }*/
            gameData.spawnObject(dest.y, dest.x, type);
            workTimer = 2f;
            goingToBuild = false;
            moveList.Dequeue();
            state = AntState.waiting;
        }
    }

    void fight(int id)
    {
        if (state != AntState.fighting) state = AntState.fighting;
        autoHitCooldown -= 1 * Time.deltaTime;

        if (autoHitCooldown <= 0)
        {
            CmdDecLives(id);
            autoHitCooldown = 2f;
        }
        
    }

    [Command]
    void CmdDecLives(int id)
    {
        gameData.decAntLive(id, this.dmg);
    }

    [Command]
    void CmdSetStats(int playerid, char type, int amt)
    {
        //print("2.: " + playerid);
        gameData.SetStats(playerid, type, amt);
    }

    public IEnumerator die ()
    {
        if (!isQueen)
        {
            state = AntState.dying;
            antanimW.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            antanimS.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            antanimD.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            antanimL.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            antanim.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            antanimTOT.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            //print("ich sterbe");
            yield return new WaitForSeconds(dyingTimer);
            gameData.killAnt(ID);
        }
        else SceneManager.LoadScene(2);
        // hier animation aktivieren
        
    }
}
