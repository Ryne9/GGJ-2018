using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour {

    public Enemy_Spawner spawn;
    public GameObject ore;

    public string type;

    public int currentState;
    private float timer;
    private float turn;
    private float turnSpeed;

    public int health;

    public int dropType;

    //Used to limit panicking when the enemy has lost sight of the player.
    private int panicCycles;

    public int walkSpeed;
    public int runSpeed;

    public Vector3 player1Position;
    public Vector3 player2Position;

    public bool player1Found;
    public bool player2Found;

    public enum Enemy_Personality
    {
        NORMAL, FRIENDLY, TIMID, SPECIAL
    }

    public enum State_Of_Mind
    {
        IDLE, PLAYER_FOUND, PANIC
    }

    public Enemy_Personality personality;

    public State_Of_Mind stateOfMind;

    public Rigidbody rb;

	// Use this for initialization
	void Start () {

        Animator anim = GetComponentInChildren<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));

        rb = GetComponent<Rigidbody>();
        currentState = 0;
        timer = 2f;
        walkSpeed = 4;
        runSpeed = 13;
        stateOfMind = State_Of_Mind.IDLE;

        int personalityGen = Random.Range(1, 101);
        if (personalityGen < 60)
        {
            personality = Enemy_Personality.NORMAL;
        }
        else if (personalityGen >= 60 && personalityGen < 89)
        {
            personality = Enemy_Personality.TIMID;
        }
        else if (personalityGen >= 89 && personalityGen < 100)
        {
            personality = Enemy_Personality.FRIENDLY;
        }
        else
        {
            personality = Enemy_Personality.SPECIAL;
        }

        player1Found = false;
        player2Found = false;

        panicCycles = 0;

        health = 100;

        dropType = 0;
	}

    public void takeDamage(int amount)
    {
        health -= amount;
        stateOfMind = State_Of_Mind.PANIC;
        
        if (health <= 0)
        {
            GameObject drop = Instantiate(ore);
            drop.transform.position = gameObject.transform.position;
            drop.GetComponent<OreController>().type = this.type;
            spawn.kill(gameObject);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //A quick run-down of what this currently is trying to do:
        //Normal enemies don't care at all until they get hit.
        //Timid enemies will avoid the players and run away from them upon hit.
        //Friendly enemies will attempt to follow the player until they get hit.
        //Special enemies just need to take some time to spin.
        if (personality == Enemy_Personality.SPECIAL)
        {
            doSpecialBehavior();
        }
        else if (stateOfMind == State_Of_Mind.IDLE)
        {
            doNormalIdleBehavior();
        }
        else if (stateOfMind == State_Of_Mind.PLAYER_FOUND)
        {
            if (personality == Enemy_Personality.NORMAL)
            {
                doNormalIdleBehavior();
            }
            else if (personality == Enemy_Personality.FRIENDLY)
            {
                doFriendlyPlayerFound();
            }
            else
            {
                doTimidPlayerFound();
            }
        }
        else if (stateOfMind == State_Of_Mind.PANIC)
        {
            if (personality == Enemy_Personality.NORMAL || personality == Enemy_Personality.FRIENDLY)
            {
                doNormalPanicBehavior();
            }
            else
            {
                doTimidPanicBehavior();
            }
        }
        
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
            
	}

    //Do the normal behavior for an enemy.
    void doNormalIdleBehavior()
    {
        switch (currentState)
        {
            case 0:
                //Move forward.
                if (Physics.Raycast(transform.position, transform.forward, 2f))
                {
                    turn = Random.Range(90f, 140f) * (Random.Range(0, 1) == 0 ? -1 : 1);
                    turnSpeed = Random.Range(3f, 5f);
                    currentState = 2;
                }
                else
                {
                    rb.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
                }
                break;
            case 1:
                //Calculate turn angle.
                turn = Random.Range(-120f, 120f);
                turnSpeed = Random.Range(1f, 4f);
                currentState = 2;
                break;
            case 2:
                //Turn.
                float angleToTurn = turnSpeed * Mathf.Sign(turn);
                turn -= angleToTurn;
                Vector3 rotation = new Vector3(0, angleToTurn, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotation);
                if (Mathf.Abs(turn) - turnSpeed < 0)
                {
                    currentState = 0;
                    timer = Random.Range(1f, 4f);
                }
                break;
        }
        timer -= Time.deltaTime;
        if (timer < 0 && currentState == 0)
        {
            currentState++;
            if (currentState > 2)
                currentState = 0;
            timer = 1f;
        }
    }

    void doSpecialBehavior()
    {
        //Like a record, baby.
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,Random.Range(1f, 11f),0));
    }

    void doFriendlyPlayerFound()
    {
        switch (currentState)
        {
            case 0:
                //Calculate turn angle.
                //Also, break out of panic mode if the enemy has been panicking for a bit and has lost sight of the player.
                if (!player1Found && !player2Found)
                {
                    stateOfMind = State_Of_Mind.IDLE;
                    break;
                }
                currentState = 1;
                break;
            case 1:
                //Turn.
                if (player1Found && player2Found)
                {
                    //For now he moves in between both players if both are there. Maybe he should instead prioritize player 1, or the first one that he found.
                    Vector3 midwayPoint = (player1Position + player2Position) / 2f;
                    transform.LookAt(midwayPoint); 
                }
                else if (player1Found)
                {
                    transform.LookAt(player1Position);
                }
                else if (player2Found)
                {
                    transform.LookAt(player2Position);
                }
                Vector3 eulerAng = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(eulerAng - new Vector3(eulerAng.x, 0, eulerAng.z));
                //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 180, 0f));
                currentState = 2;
                timer = Random.Range(3f, 6f);
                break;
            case 2:
                //Move forward.
                //This enemy type will get stuck on other enemies and walls to make it to a Player, as of right now.
                //if (Physics.Raycast(transform.position, transform.forward, 1f, 1 << 0))
                //{
                //    currentState = 0;
                //}
                //Layer 8 is going to be the Enemy Layer
                //if (Physics.Raycast(transform.position, transform.forward, 1f, 1 << 8))
                //{
                //    currentState = 0;
                //}
                //Layer 9 will be the Player Layer.
                if (!player1Found && !player2Found)
                {
                    stateOfMind = State_Of_Mind.IDLE;
                    break;
                }

                Debug.Log(transform.forward);
                rb.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
                
                break;
        }
        timer -= Time.deltaTime;
        if (timer < 0 && currentState == 2)
        {
            currentState = 0;
        }
    }

    void doTimidPlayerFound()
    {
        switch (currentState)
        {
            case 0:
                //Calculate turn angle.
                //Also, break out of panic mode if the enemy has been panicking for a bit and has lost sight of the player.
                if (!player1Found && !player2Found)
                {
                    stateOfMind = State_Of_Mind.IDLE;
                    break;
                }
                currentState = 1;
                break;
            case 1:
                //Turn.
                if (player1Found && player2Found)
                {
                    //For now he moves in between both players if both are there. Maybe he should instead prioritize player 1, or the first one that he found.
                    Vector3 midwayPoint = (player1Position + player2Position) / 2f;
                    transform.LookAt(midwayPoint);
                }
                else if (player1Found)
                {
                    transform.LookAt(player1Position);
                }
                else if (player2Found)
                {
                    transform.LookAt(player2Position);
                }
                Vector3 eulerAng = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(eulerAng - new Vector3(eulerAng.x, 0, eulerAng.z));
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 180, 0f));
                currentState = 2;
                timer = Random.Range(3f, 6f);
                break;
            case 2:
                //This enemy type is easy to corner - even though he'll run away, if you trap him in between some other enemies or a wall he's screwed.
                //Move forward.
                if (Physics.Raycast(transform.position, transform.forward, 1f, 1 << 0))
                {
                    currentState = 0;
                }
                //Layer 8 is going to be the Enemy Layer
                else if (Physics.Raycast(transform.position, transform.forward, 1f, 1 << 8))
                {
                    currentState = 0;
                }
                //Layer 9 will be the Player Layer.
                else if (Physics.Raycast(transform.position, transform.forward, 3f, 1 << 9))
                {
                    currentState = 0;
                }
                else
                {
                    rb.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
                }
                break;
        }
        timer -= Time.deltaTime;
        if (timer < 0 && currentState == 2)
        {
            currentState++;
            if (currentState > 2)
                currentState = 0;
            timer = 1f;
        }
    }

    void doNormalPanicBehavior()
    {
        if (personality == Enemy_Personality.FRIENDLY)
        {
            //You monster.
            personality = Enemy_Personality.TIMID;
        }
        switch (currentState)
        {
            case 0:
                //Calculate turn angle.
                //Also, break out of panic mode if the enemy has been panicking for a bit and has lost sight of the player.
                if (!player1Found && !player2Found && panicCycles > 10)
                {
                    stateOfMind = State_Of_Mind.IDLE;
                    panicCycles = 0;
                    break;
                }
                turn = Random.Range(-150f, 150f);
                turnSpeed = Random.Range(15f, 20f);
                currentState = 1;
                break;
            case 1:
                //Turn.
                float angleToTurn = turnSpeed * Mathf.Sign(turn);
                turn -= angleToTurn;
                Vector3 rotation = new Vector3(0, angleToTurn, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotation);
                if (Mathf.Abs(turn) - turnSpeed < 0)
                {
                    currentState = 2;
                    timer = Random.Range(3f, 6f);
                }
                break;
            case 2:
                //Move forward.
                if (Physics.Raycast(transform.position, transform.forward, 1f, 1 << 0))
                {
                    turn = Random.Range(90f, 270f) * (Random.Range(0, 1) == 0 ? -1 : 1);
                    turnSpeed = Random.Range(3f, 5f);
                    currentState = 1;
                }
                //Layer 8 is going to be the Enemy Layer
                else if (Physics.Raycast(transform.position, transform.forward, 1f, 1 << 8))
                {
                    turn = Random.Range(30f, 60f) * (Random.Range(0, 1) == 0 ? -1 : 1);
                    turnSpeed = Random.Range(3f, 5f);
                    currentState = 1;
                }
                else
                {
                    rb.MovePosition(transform.position + transform.forward * runSpeed * Time.deltaTime);
                }
                break;
        }
        panicCycles++;
        timer -= Time.deltaTime;
        if (timer < 0 && currentState == 2)
        {
            currentState++;
            if (currentState > 2)
                currentState = 0;
            timer = 1f;
        }
    }

    void doTimidPanicBehavior()
    {
        switch (currentState)
        {
            case 0:
                //Calculate turn angle.
                //Also, break out of panic mode if the enemy has been panicking for a bit and has lost sight of the player.
                if (!player1Found && !player2Found && panicCycles > 16)
                {
                    stateOfMind = State_Of_Mind.IDLE;
                    panicCycles = 0;
                    break;
                }
                else if (!player1Found && !player2Found)
                {
                    turn = Random.Range(-150f, 150f);
                }
                turnSpeed = Random.Range(15f, 20f);
                currentState = 1;
                break;
            case 1:
                //Turn.
                if (!player1Found && !player2Found)
                {
                    float angleToTurn = turnSpeed * Mathf.Sign(turn);
                    turn -= angleToTurn;
                    Vector3 rotation = new Vector3(0, angleToTurn, 0);
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotation);
                    if (Mathf.Abs(turn) - turnSpeed < 0)
                    {
                        currentState = 2;
                        timer = Random.Range(3f, 6f);
                    }
                }
                else if (player1Found)
                {
                    transform.rotation = Quaternion.LookRotation(transform.position - player1Position);
                    currentState = 2;
                    timer = Random.Range(3f, 6f);
                }
                else if (player2Found)
                {
                    transform.rotation = Quaternion.LookRotation(transform.position - player2Position);
                    currentState = 2;
                    timer = Random.Range(3f, 6f);
                }
                else
                {
                    Vector3 midwayPoint = (player1Position + player2Position) / 2f;
                    transform.rotation = Quaternion.LookRotation(transform.position - midwayPoint);
                    currentState = 2;
                    timer = Random.Range(3f, 6f);
                }
                break;
            case 2:
                //Move forward.
                if (Physics.Raycast(transform.position, transform.forward, 1f, 1 << 0))
                {
                    turn = Random.Range(90f, 270f) * (Random.Range(0, 1) == 0 ? -1 : 1);
                    turnSpeed = Random.Range(3f, 5f);
                    currentState = 1;
                }
                //Layer 8 is going to be the Enemy Layer
                else if (Physics.Raycast(transform.position, transform.forward, 1f, 1 << 8))
                {
                    turn = Random.Range(30f, 60f) * (Random.Range(0, 1) == 0 ? -1 : 1);
                    turnSpeed = Random.Range(3f, 5f);
                    currentState = 1;
                }
                //Layer 9 will be the Player Layer.
                else if (Physics.Raycast(transform.position, transform.forward, 1f, 1 << 9))
                {
                    turn = 180f;
                    turnSpeed = Random.Range(3f, 5f);
                    currentState = 1;
                }
                else
                {
                    rb.MovePosition(transform.position + transform.forward * runSpeed * Time.deltaTime);
                }
                break;
        }
        panicCycles++;
        timer -= Time.deltaTime;
        if (timer < 0 && currentState == 2)
        {
            currentState++;
            if (currentState > 2)
                currentState = 0;
            timer = 1f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player1"))
        {
            player1Found = true;
            player1Position = other.gameObject.transform.position;
            stateOfMind = State_Of_Mind.PLAYER_FOUND;
        }
        else if (other.name.Contains("Player2"))
        {
            player2Found = true;
            player2Position = other.gameObject.transform.position;
            stateOfMind = State_Of_Mind.PLAYER_FOUND;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Player1"))
        {
            player1Found = false;
        }
        else if (other.name.Contains("Player2"))
        {
            player2Found = false;
        }
    }
}
