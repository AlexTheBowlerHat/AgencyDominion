using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControl : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D body;
    public Weapon weapon;
    public Transform weaponTransform;
    //Sprite lists don't include west due to use of spriterender.flipx

    public List<Sprite> northSprites;
    public List<Sprite> northEastSprites;
    public List<Sprite> eastSprites;
    public List<Sprite> southSprites;
    public List<Sprite> southEastSprites;
    

    // public InputActionMap PlayerActions;
    //private Dictionary<string, string> ActionToMovement = new Dictionary<string,Vector2>();

    //const float screenDivideConst = 3;
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
    bool hasFired = false;



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
        RetreiveMouseInfo();
        SetSprite();
        FlipSprite();
        //SendAnimate();
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
        }
        //body.AddForce(direction * walkSpeed * Time.fixedDeltaTime, ForceMode2D.Force); //Force method to be used w/ dynamic rb in case
        //body.MovePosition(new Vector2(body.transform.position.x + direction.x , body.transform.position.y + direction.y));

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " HAS COLLIDED WITH " + collision.gameObject.name);
    }

    //Shooting Methods============================================================

    public Vector2 RetreiveMouseInfo()
    {
        mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //Takes mouse position from the camera and reads it
        lookDirection = (mousePos - body.position).normalized;
        lookDirectionUnnormalized = mousePos - body.position;   //Sets lookdirectionunormalized to a vector that points from the player object to the mouse

        lookAngle = Mathf.Atan2(lookDirectionUnnormalized.y, lookDirectionUnnormalized.x) * Mathf.Rad2Deg; //Returns float that is an angle between x axis and the look direction vector
        Quaternion rotation = Quaternion.Euler(0, 0, lookAngle - 90f);
        weaponTransform.rotation = rotation;

        //Debug.Log("MOUSEPOSITION" + mousePos.ToString());
        Debug.Log("ANGLE W/O -90F: " + lookAngle.ToString());
        return lookDirection;

    }
    IEnumerator Shoot()
    {

        if (!hasFired)
        {
            hasFired = true;
            Vector2 lookVector = RetreiveMouseInfo();
            weapon.Fire(lookVector);
            yield return new WaitForSeconds(0.2f);
            hasFired = false;

        }
        yield break;

    }
        
       
    public void Fire(InputAction.CallbackContext context)
    {
        StartCoroutine(Shoot());
       
        


        //mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //Takes mouse position from the camera and reads it //.normalized; //- transform.position; //mainCam.ScreenToWorldPoint()

        //lookDirection = (mousePos - body.position).normalized;
        //mouseX = lookDirection.x; //- Screen.width / 2f;
        //mouseY = lookDirection.y; //- Screen.height / 2f;


        //lookDirectionUnnormalized = mousePos - body.position;
        //lookAngle = Mathf.Atan2(lookDirectionUnnormalized.y, lookDirectionUnnormalized.x) * Mathf.Rad2Deg;

        //Debug.Log(lookAngle);
        //Debug.Log("LOOKDIRECTION IS: " + lookDirection.ToString());
        //Debug.Log("MOUSEPOSITION" + mousePos.ToString());
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
        if (!spriteRenderer.flipX && lookDirection.x < 0) //Sprite not flipped and input to left
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

        //Forgive me for what you're about to witness
        //Checks where the mouse is on the screen in terms of two semi circles, one 0 to 180, another -0 to -180
        switch (lookAngle) 
        {
            //NORTH 
            case float _ when 67.5f < lookAngle && lookAngle < 112.5f:
                Debug.Log("NORTH");
                selectedSprites = northSprites;
                break;

            //SOUTH 
            case float _ when -67.5f > lookAngle && lookAngle > -112.5f:
                Debug.Log("SOUTH");
                selectedSprites = southSprites;
                break;

            //Could tehnically put these together west & east
            //WEST 
            case float _ when 157.5f < lookAngle || lookAngle < -157.5f: 
                Debug.Log("WEST");
                selectedSprites = eastSprites;
                break;

            //EAST 
            case float _ when 22.5f > lookAngle && lookAngle > -22.5f:
                Debug.Log("EAST");
                selectedSprites = eastSprites;
                break;

            //NORTH WEST 
            case float _ when 112.5f < lookAngle && lookAngle < 157.5f:
                Debug.Log("NORTH WEST");
                selectedSprites = northEastSprites;
                break;

            //NORTH EAST
            case float _ when 22.5f < lookAngle && lookAngle < 67.5f:
                Debug.Log("NORTH EAST");
                selectedSprites = northEastSprites;
                break;

            //SOUTH WEST
            case float _ when -157.5f < lookAngle && lookAngle < -112.5f:
                Debug.Log("SOUTH WEST");
                selectedSprites = southEastSprites;
                break;

            //SOUTH EAST 
            case float _ when -67.5f < lookAngle && lookAngle < -22.5f:
                Debug.Log("SOUTH EAST");
                selectedSprites = southEastSprites;
                break;

            default:
                Debug.LogWarning("Error: Look angle switch failure | lines 182 to 227");
                break;

        }
        return selectedSprites;
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

    }
}
