using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    [SerializeField] private float projectileForce = 20f;
   
    // Start is called before the first frame update
   
    public void Fire(Vector2 lookVector, string firedTag)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<ProjectileBehavior>().SetFired(firedTag);
        projectile.GetComponent<Rigidbody2D>().AddForce(lookVector * projectileForce, ForceMode2D.Impulse);
    }
}
