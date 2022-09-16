using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Unity Forums user NinjalSV - http://answers.unity.com/answers/1810252/view.html
public static class Collider2DExtensions 
{
    // Gets PolygonCollider2D from whatever calls it
    public static void TryUpdateShapeToAttachedSprite (this PolygonCollider2D collider) 
    {
        // Gets collider's sprite
        collider.UpdateShapeToSprite(collider.GetComponent<SpriteRenderer>().sprite);
    }

    //Updates attached physics shape to the sprite's one
    public static void UpdateShapeToSprite (this PolygonCollider2D collider, Sprite sprite) 
    {
        if (collider != null && sprite != null) 
        {
            //Gets number of physics shape points from sprite
            collider.pathCount = sprite.GetPhysicsShapeCount();
            
            // New list to store all the points of the sprite
            List<Vector2> path = new List<Vector2>();

            // Loops through all points
            for (int i = 0; i < collider.pathCount; i++) 
            {
                // Clears path storage
                path.Clear();
                // Gets shape
                sprite.GetPhysicsShape(i, path);
                // Changes collider point
                collider.SetPath(i, path.ToArray());
            }
        }
    }
}
