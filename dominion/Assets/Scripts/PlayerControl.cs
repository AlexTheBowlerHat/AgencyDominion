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

    const float screenDivideConst = 3;
    //public Animator animator;
    /*
    string currentState;
    const string playerIdle = "Player_Idle";
    const string playerWalkLeft = "Player_Walk_Left";
    const string playerWalkRight = "Player_Walk_Right";
    const string playerWalkUp = "Player_Walk_Up";
    const string playerWalkDown = "Player_Walk_Down";
    */

    public float walkSpeed;
    [SerializeField] Vector2 direction;
    [SerializeField] Vector2 lookDirection;
    [SerializeField] float lookAngle;
    [SerializeField] Vector2 lookDirectionUnnormalized;

    //Variables for shooting
    public Vector2 mousePos;
    public float mouseX;
    public float mouseY;
    public Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        mainCam.enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    void Update()
    {
        Fire();
        //SendAnimate();
        SetSprite();
        FlipSprite();
        //ChangeAnimationState(playerIdle);
    }

    //Movement Methods==================================

    public void MoveInvoked(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        //Debug.Log("CONTEXT is: " + context.ToString());
        // Debug.Log("received at PlayerControl, OOG UGG VERY HAPPY");

        //Debug.Log(direction);
       
        //FlipSprite();
        //SetSprite();
        //while input is down do move player

    }


    public void MovePlayer()
    {
        if (direction != Vector2.zero)
        {
            body.velocity = direction * walkSpeed * Time.fixedDeltaTime; //Velocity method

            //body.AddForce(direction * walkSpeed * Time.fixedDeltaTime, ForceMode2D.Force); //Force method to be used w/ dynamic rb in case
            //body.MovePosition(new Vector2(body.transform.position.x + direction.x , body.transform.position.y + direction.y));
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " HAS COLLIDED WITH " + collision.gameObject.name);
    }

    //Shooting Methods============================================================
    public void Fire()
    {
        mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //Takes mouse position from the camera and reads it //.normalized; //- transform.position; //mainCam.ScreenToWorldPoint

        lookDirection = (mousePos - body.position).normalized;
        mouseX = lookDirection.x; //- Screen.width / 2f;
        mouseY = lookDirection.y; //- Screen.height / 2f;


        lookDirectionUnnormalized = mousePos - body.position;
        lookAngle = Mathf.Atan2(lookDirectionUnnormalized.y, lookDirectionUnnormalized.x) * Mathf.Rad2Deg;

        //Debug.Log(lookAngle);
        //Debug.Log("LOOKDIRECTION IS: " + lookDirection.ToString());
        Debug.Log("MOUSEPOSITION" + mousePos.ToString());
        //something something minus character transform normalise then force

    }

    //Animation Methods======================================================

    /*private void SendAnimate()
    {
        animator.SetFloat("LookX", lookDirection.x);
        animator.SetFloat("LookY", lookDirection.y);

    }
    */
    /*
    void ChangeAnimationState(string newstate)
    {
        //Checks if next animation is already playing
        if (currentState != newstate)
        {
            //Plays new animation
            animator.Play(newstate);

            //Updates current animation
            currentState = newstate;
        }
        else return;
        
    }
    */

    //Sprite Methods=======================================================
    void FlipSprite()
    {
        if (!spriteRenderer.flipX && mouseX < 0 ) //Sprite not flipped and input to left
        {
            spriteRenderer.flipX = true;
        } 
        else if (spriteRenderer.flipX && mouseX > 0) // Sprite flipped and input heads right
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

        //Forgive me for what you're about to witness

        switch (mousePos) //7.5 -231
        {
            //NORTH (x,y > y/k and y/-k | also y > 0)
            case Vector2 _ when mouseY > 0 && mouseX > mouseY / screenDivideConst && mouseY > mouseY / screenDivideConst 
            && mouseX > mouseY / -screenDivideConst && mouseY > mouseY / -screenDivideConst:
                Debug.Log("NORTH");
                break;

            //SOUTH (x,y < y/k and y/-k | also y < 0)
            case Vector2 _ when mouseY < 0 && mouseX < mouseY / screenDivideConst && mouseY < mouseY / screenDivideConst
            && mouseX < mouseY / -screenDivideConst && mouseY < mouseY / -screenDivideConst:
                Debug.Log("SOUTH");
                break;

            //EAST (x,y < x/k and > x/-k | also x > 0)
            case Vector2 _ when mouseX > 0 && mouseX < mouseX / screenDivideConst && mouseY < mouseX / screenDivideConst
            && mouseX > mouseX / -screenDivideConst && mouseY > mouseX / -screenDivideConst:
                Debug.Log("EAST");
                break;

            //WEST (x,y < x/-k and > x/k | also x < 0)
            case Vector2 _ when mouseX < 0 && mouseX < mouseX / -screenDivideConst && mouseY < mouseX / -screenDivideConst
            && mouseX > mouseX / screenDivideConst && mouseY > mouseX / screenDivideConst:
                Debug.Log("WEST");
                break;

            //NORTHWEST (x,y < y/-k and > x/-k | also x < 0 and y > 0
            case Vector2 _ when mouseX < 0 && mouseY > 0 && mouseX < mouseY / -screenDivideConst && mouseY < mouseY / -screenDivideConst
            && mouseX > mouseX / -screenDivideConst && mouseY > mouseX / -screenDivideConst:
                Debug.Log("NORTHWEST");
                break;

            //NORTHEAST (x,y < y/k and > x/k | also x > 0 and y > 0
            case Vector2 _ when mouseX > 0 && mouseY > 0 && mouseX < mouseY / screenDivideConst && mouseY < mouseY / screenDivideConst
            && mouseX > mouseX / screenDivideConst && mouseY > mouseX / screenDivideConst:
                Debug.Log("NORTHEAST");
                break;

            //SOUTHWEST (x,y > y/k and < x/k | also x < 0 and y < 0
            case Vector2 _ when mouseX < 0 && mouseY < 0 && mouseX > mouseY / screenDivideConst && mouseY > mouseY / screenDivideConst
            && mouseX < mouseX / screenDivideConst && mouseY < mouseX / screenDivideConst:
                Debug.Log("SOUTHWEST");
                break;

            //SOUTHEAST (x,y > y/-k and < x/-k | also x > 0 and y < 0
            case Vector2 _ when mouseX > 0 && mouseY < 0 && mouseX > mouseY / -screenDivideConst && mouseY > mouseY / -screenDivideConst
            && mouseX < mouseX / -screenDivideConst && mouseY < mouseX / -screenDivideConst:
                Debug.Log("SOUTHEAST");
                break;

            default:
                Debug.LogWarning("Oh no the player look direction");
                break;

        }
        /*
        //Left or right

        //Right case
        if (mousePos.x >= 0)
        {
            if (mousePos.y < mousePos.x / screenDivideConst && mousePos.y > mousePos.x / -screenDivideConst) //Player facing right
            {
                Debug.Log("ooooog ahh ");
                selectedSprites = eastSprites;
            }
            else //Check if nw or sw
            {
                if (mousePos.y > 0)
                {

                }
            }

        }
        //Left case
        else
        {

        }
        */
        /*
        /////North Handling
        if (lookDirection.y > 0) //Going north
        {
            if (Mathf.Abs(lookDirection.x) > 0) //Player moving left or right
            {
                selectedSprites = northEastSprites; //Only north east as FlipSprite() handles west 
            }
            else //Player going solely north
            {
                selectedSprites = northSprites;
            }
        }

        ////South Handling
        else if (lookDirection.y < 0) //Going South
        {
            if (Mathf.Abs(lookDirection.x) > 0) //Player moving left or right
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
            if (Mathf.Abs(lookDirection.x) > 0) //Player moving left or right
            {
                selectedSprites = eastSprites; //Only east as FlipSprite() handles west 
            }
        }
        */
        return selectedSprites;

    }
}
