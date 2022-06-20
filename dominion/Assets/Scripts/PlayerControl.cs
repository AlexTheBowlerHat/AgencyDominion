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
       // Debug.Log("received at PlayerControl, OOG UGG VERY HAPPY");

        direction = context.ReadValue<Vector2>();
        Debug.Log(direction);
        FlipSprite();

    }

    public void MovePlayer()
    {
        //body.AddForce(direction * walkSpeed * Time.fixedDeltaTime, ForceMode2D.Force); //Force method to be used w/ dynamic rb in case
        body.velocity = direction * walkSpeed * Time.fixedDeltaTime; //Velocity method
        //body.MovePosition(new Vector2(body.transform.position.x + direction.x , body.transform.position.y + direction.y));
    }
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        SetSprite();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " is touch " + collision.gameObject.name);
    }
    void FlipSprite()
    {
        if (!spriteRenderer.flipX && direction.x < 0 ) //Sprite not flipped and input to left
        {
            spriteRenderer.flipX = true;
        } 
        else if (spriteRenderer.flipX && direction.x > 0) // Sprite flipped and input heads right
        {
            spriteRenderer.flipX = false;
        }
    }

    void SetSprite()
    {
        List<Sprite> directionSpritesChosen = SelectSpriteList();

        if (directionSpritesChosen != null) //Chooses sprite to show if there are no sprites
        {
            spriteRenderer.sprite = directionSpritesChosen[0];
        }
        else
        {
            return;
        }

    }

    List<Sprite> SelectSpriteList()
    {
        List<Sprite> selectedSprites = null; //No sprites currently chosen, resets

        /////North Handling
        if (direction.y > 0) //Going north
        {
            if (Mathf.Abs(direction.x) > 0) //Player moving left or right
            {
                selectedSprites = northEastSprites; //Only north east as FlipSprite() handles west 
            }
            else //Player going solely north
            {
                selectedSprites = northSprites;
            }
        }
        
        ////South Handling
        else if (direction.y < 0) //Going South
        {
                if (Mathf.Abs(direction.x) > 0) //Player moving left or right
                {
                    selectedSprites = southEastSprites; //Only south east as FlipSprite handles west
                }
                else //Player going solely south
                {
                    selectedSprites = southSprites;
                }
        }

        else
        {
            if (Mathf.Abs(direction.x) > 0) //Player moving left or right
            {
                selectedSprites = eastSprites; //Only east as FlipSprite() handles west 
            }
        }

        return selectedSprites;

    }
}
