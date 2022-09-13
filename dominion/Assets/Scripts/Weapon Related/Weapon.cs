using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    bool hasFired = false;
    [SerializeField] float projectileDamage;

    //Coroutine that fires bullet with a cooldown
    public IEnumerator Shoot(Vector2 lookVector, float cooldown, float projectileForce, string firedTag, Transform firePoint, bool holdAccessibility)
    {
        if (!hasFired)
        {
            hasFired = true;
            Fire(lookVector, firedTag, projectileForce, firePoint);
            yield return new WaitForSeconds(cooldown);
            hasFired = false;
        }
        yield break;
    }
   
   //Creates a projectile and fires it from the weapon, also passes the tag of object that fired it
    public void Fire(Vector2 lookVector, string firedTag, float projectileForce, Transform firePoint)
    {
        Vector2 projectileDirection = (projectileForce * lookVector);
        /*
        Debug.Log(firedTag + " CALLED FIRE()");
        Debug.Log("FORCE FIRED IS: " + projectileForce * lookVector);
        Debug.Log("MAGNITUDE IS: " + (projectileForce * lookVector).magnitude);
        */
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.tag = "NotToBeCollided";
        projectile.GetComponent<ProjectileBehavior>().SetFired(firedTag, projectileDamage);
        projectile.GetComponent<Rigidbody2D>().velocity = projectileDirection;
    }
}
