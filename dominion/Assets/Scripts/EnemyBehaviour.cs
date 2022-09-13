using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Weapon weapon;
    public Transform firePoint;
    private Vector2 enemyToPlayer;
    private Vector2 enemyToPlayerNormalised;
    [SerializeField]
    private float enemyFireForce;
    [SerializeField] 
    private float enemyCooldown;
    public bool playerInZone = false;
    bool turretSleeping = false;
    Rigidbody2D body;
    public float enemyWalkSpeed;
    //Animations & states 
    Animator animator;
    string currentAnimState;
    const string enemySleepIdle =  "EnemySleepIdle";
    const string enemyAwakeIdle =  "EnemyAwakeIdle";
    const string enemyAwaking =  "EnemyAwakeToSleep";
    const string enemySleeping =  "EnemySleepToAwake";
    

    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        weapon = gameObject.GetComponent<Weapon>();
        animator = gameObject.GetComponent<Animator>();
        gameObject.GetComponent<HealthScript>().invincible = true;
    }

    public void MoveEnemy()
    {
        body.velocity = enemyToPlayerNormalised * enemyWalkSpeed * Time.fixedDeltaTime; //Velocity method
    }
    public void AttackPlayer()
    {
        StartCoroutine(weapon.Shoot(enemyToPlayerNormalised, enemyCooldown, enemyFireForce, gameObject.tag, firePoint, false));
    }
    //Iterates itself once called until the player is out of range, calls move and attack functions with a delay each iteration
    public IEnumerator PlayerDetected(Vector2 playerPosition, GameObject playerObject, float playerDistanceMag)
    {
        if (playerDistanceMag<=5)
        {
            //Debug.Log(playerDistanceMag);
            playerPosition = playerObject.transform.position;
            enemyToPlayer = playerPosition - (Vector2)gameObject.transform.position;
            playerDistanceMag = enemyToPlayer.magnitude;
            enemyToPlayerNormalised = enemyToPlayer.normalized;

            MoveEnemy();
            AttackPlayer();
            //Debug.Log("Got to here!");
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(PlayerDetected(playerPosition, playerObject, playerDistanceMag));
        }
        else
        {
            if(!turretSleeping)
            {
                turretSleeping = true;
                Debug.Log("going to sleep");
                ChangeAnimationState(enemySleeping);
                Debug.Log("sleeping");
                ChangeAnimationState(enemySleepIdle);
                turretSleeping = false;
            }
            playerInZone = false;
            gameObject.GetComponent<HealthScript>().invincible = true;
            yield break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            
            //Debug.Log("enter oog detected");
        
            Vector2 playerPos = collision.transform.position;
            GameObject playerObject = collision.gameObject;
            float playerMag = (playerPos - (Vector2)gameObject.transform.position).magnitude;
            if(!playerInZone && playerMag <=5)
            {
                gameObject.GetComponent<HealthScript>().invincible = false;
                Debug.Log("awaking");
                ChangeAnimationState(enemyAwaking);
                Debug.Log("awake idling");
                ChangeAnimationState(enemyAwakeIdle);
                playerInZone = true;
                StartCoroutine(PlayerDetected(playerPos, playerObject, playerMag));
            }
        }
    }

// Derived from Dani Krossing's method
    void ChangeAnimationState(string newState)
    {
        //Stops interrupting itself
        if (currentAnimState == newState) return;
        //Plays aniamtion
        animator.Play(newState);
        //Updates state

    }
}
