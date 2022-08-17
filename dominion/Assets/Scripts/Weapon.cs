using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    [SerializeField] private float projectileForce = 20f;
   
   //Creates a projectile and fires it from the weapon, also passes the tag of object that fired it
    public void Fire(Vector2 lookVector, string firedTag)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<ProjectileBehavior>().SetFired(firedTag);
        projectile.GetComponent<Rigidbody2D>().AddForce(lookVector * projectileForce, ForceMode2D.Impulse);
    }
}
