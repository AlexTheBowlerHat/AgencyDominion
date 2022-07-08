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
        //Debug.Log("CONTEXT is: " + context.ToString());
       // Debug.Log("received at PlayerControl, OOG UGG VERY HAPPY");

        direction = context.ReadValue<Vector2>();
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
        mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position; //mainCam.ScreenToWorldPoint
        lookDirection = (mousePos - body.position).normalized;
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
        if (!spriteRenderer.flipX && lookDirection.x < 0 ) //Sprite not flipped and input to left
        {
            spriteRenderer.flipX = true;
        } 
        else if (spriteRenderer.flipX && lookDirection.x > 0) // Sprite flipped and input heads right
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

        /*switch (lookAngle)
        {
            case float _ when lookAngle > 0:
                Debug.Log("AHHHHH");
                break;

            case float _ when lookAngle < 0:

                break;

            default:
                break;

        }

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

        return selectedSprites;

    }
}
