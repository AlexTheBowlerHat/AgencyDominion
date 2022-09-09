using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Weapon weapon;
    private Vector2 enemyToPlayer;
    public bool playerInZone = false;
    Rigidbody2D body;
    public float enemyWalkSpeed = 20f;

    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    public void MoveEnemy()
    {
        body.velocity = enemyToPlayer * enemyWalkSpeed * Time.fixedDeltaTime; //Velocity method
    }
    public void AttackPlayer()
    {
        StartCoroutine(weapon.Shoot(enemyToPlayer, 2f, 5f, gameObject.tag));
    /*}
    public dynamic IEnumerator SetNextScene()
    {
        while (playerInZone)
        {
            yield return new WaitForSeconds(0.1f);
            enemyToPlayer = collision.transform.position - gameObject.transform.position;
            AttackPlayer();
            MoveEnemy();
        }

    }
    */


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("oog detected");
        if (collision.tag == "Player")
        {
            playerInZone = true;
            while (playerInZone)
            {

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("ugg detected");
        playerInZone = false;
    }
}
