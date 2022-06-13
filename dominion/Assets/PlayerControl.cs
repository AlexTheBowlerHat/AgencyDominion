using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControl : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D body;
    //Sprite lists don't include west due to use of spriterender.flipx
    public List<Sprite> northSprites;
    public List<Sprite> northEastSprites;
    public List<Sprite> eastSprites;
    public List<Sprite> southSprites;
    public List<Sprite> southEastSprites;

   // public InputActionMap PlayerActions;
    //public Dictionary<string, string> ActionToMovement;

    public float walkSpeed;
    [SerializeField] Vector2 direction; 

    public void MoveInvoked(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        Debug.Log("received at PlayerControl, OOG UGG VERY HAPPY");

        direction = context.ReadValue<Vector2>();
        Debug.Log(direction);

    }

    public void MovePlayer()
    {
        body.AddForce(direction * walkSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
        //body.velocity = direction * walkSpeed;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }
}
