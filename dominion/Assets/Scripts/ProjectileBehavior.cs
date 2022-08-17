using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    string tagThatFired;
    //Sets tag to stop projectile hitting itself
    public void SetFired(string passedTag)
    {
        tagThatFired = passedTag;
    }

    //Cleans up projectile once it hits an object, checks it isn't hitting its owner
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != tagThatFired)
        {
            Destroy(gameObject);
        }
        
    }
}
