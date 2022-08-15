using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    string tagThatFired;
    public void SetFired(string passedTag)
    {
        tagThatFired = passedTag;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != tagThatFired)
        {
            Destroy(gameObject);
        }
        
    }
}
