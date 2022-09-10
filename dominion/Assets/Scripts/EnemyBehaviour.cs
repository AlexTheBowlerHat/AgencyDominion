using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Weapon weapon;
    public Transform firePoint;
    private Vector2 enemyToPlayer;
    public bool playerInZone = false;
    Rigidbody2D body;
    public float enemyWalkSpeed;
    

    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        weapon = gameObject.GetComponent<Weapon>();
    }

    public void MoveEnemy()
    {
        body.velocity = enemyToPlayer * enemyWalkSpeed * Time.fixedDeltaTime; //Velocity method
    }
    public void AttackPlayer()
    {
        StartCoroutine(weapon.Shoot(enemyToPlayer, 1f, 1f, gameObject.tag, firePoint));
    }

    public IEnumerator PlayerDetected(Vector2 playerPosition, GameObject playerObject, float playerDistanceMag)
    {
        if (playerDistanceMag<=5)
        {
            //Debug.Log(playerDistanceMag);
            playerPosition = playerObject.transform.position;
            enemyToPlayer = playerPosition - (Vector2)gameObject.transform.position;
            playerDistanceMag = enemyToPlayer.magnitude;
            MoveEnemy();
            AttackPlayer();
            //Debug.Log("Got to here!");
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(PlayerDetected(playerPosition, playerObject, playerDistanceMag));
        }
        else
        {
            yield break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Debug.Log("enter oog detected");
            //playerInZone = true;
            Vector2 playerPos = collision.transform.position;
            GameObject playerObject = collision.gameObject;
            float playerMag = (playerPos - (Vector2)gameObject.transform.position).magnitude;
            StartCoroutine(PlayerDetected(playerPos, playerObject, playerMag));
        }
    }
    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is CircleCollider2D)
        {
            //Debug.Log("exit ugg detected");
            playerInZone = false;
        }
    }
    */
}
