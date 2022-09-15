using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerControl : MonoBehaviour
{
    [Header ("==General Variables==")]
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D body;
    public PolygonCollider2D playersPolygonCollider;
    [Space (5)]

    //Sprite lists, doesn't include west due to use of spriterender.flipx
    [Header ("==Sprite Lists==")]
    public List<Sprite> northSprites;
    public List<Sprite> northEastSprites;
    public List<Sprite> northWestSprites;
    public List<Sprite> eastSprites;
    public List<Sprite> westSprites;
    public List<Sprite> southSprites;
    public List<Sprite> southEastSprites;
    public List<Sprite> southWestSprites;
    [Space (5)]

    //Variables for movement
    [Header ("==Movement Variables==")]
    public float walkSpeed;
    [SerializeField] Vector2 direction;
    [Space (5)]

    //Variables for look direction
    [Header ("==Mouse Info Variables==")]
    [SerializeField] Vector2 lookDirection;
    [SerializeField] float lookAngle;
    [SerializeField] float threeSixtyLookAngle;
    [SerializeField] Vector2 lookDirectionUnnormalized;
    [Space (5)]

    //Variables for shooting
    [Header ("==Shooting Variables==")]
    public Weapon weapon;
    public Transform weaponTransform;
    public SpriteRenderer weaponSpriter;
    public Transform firePoint;
    public Vector2 mousePos;
    public Camera mainCam;
    public Transform handleTransform;
    public bool holdAccessibility = false;
    bool stopHoldFire = false;
    [SerializeField] float playerCooldown;
    [SerializeField] float playerFireForce;
    //IEnumerator coroutineRef;
    public Vector3[] weaponPositions = {new Vector3(-0.45f,-0.5f,0),new Vector3(0.45f,-0.25f,0)};


    [Space (5)]

    //Variables for animation
    [Header ("==Animation Variables==")]
    Animate animateScript;
    const string playerWalkDown =  "playerWalkDown";
    const string playerWalkUp =  "playerWalkUp";
    const string playerWalkLeft =  "playerWalkLeft";
    const string playerWalkRight =  "playerWalkRight";
    const string playerWalkUpLeft =  "playerWalkUpLeft";
    const string playerWalkUpRight =  "playerWalkUpRight";
    const string playerWalkDownLeft =  "playerWalkDownLeft";
    const string playerWalkDownRight =  "playerWalkDownRight";
    void Start()
    {
        //Sets up the camera and gets a reference
        mainCam = Camera.main;
        mainCam.enabled = true;
        handleTransform = transform.GetChild(0);
        playersPolygonCollider = gameObject.GetComponent<PolygonCollider2D>();
        weapon = weaponTransform.GetComponent<Weapon>();
        weaponSpriter = weaponTransform.GetComponent<SpriteRenderer>();
        animateScript = gameObject.GetComponent<Animate>();
        //coroutineRef = HoldFire(STOP);
    }

    //FixedUpdate is called every 0.02s
    void FixedUpdate()
    {
        MovePlayer();
    }

    //Update is called once per frame
    void Update()
    {
        RetreiveMouseInfo();
        SetSprite();
        //FlipSprite();
        FlipWeapon();
    }

    //Movement Methods
    //Whenever a movement button is pressed, create a reference to the movement direction the player is going
    public void MoveInvoked(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }
    public void MovePlayer()
    {
        if (direction != Vector2.zero)
        {
            body.velocity = direction * walkSpeed * Time.fixedDeltaTime; //Velocity method
        }
    }

    //Shooting Methods============================================================
    //Gets key information for shooting and wepaon direction
    public Vector2 RetreiveMouseInfo()
    {
        //Takes mouse position from the camera 
        mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue()); 

        //Vectors that point from the player to the mouse 
        lookDirection = (mousePos - body.position).normalized;
        lookDirectionUnnormalized = mousePos - body.position;

        //Returns float that is an angle between x axis and the look direction of the player
        lookAngle = Mathf.Atan2(lookDirectionUnnormalized.y, lookDirectionUnnormalized.x) * Mathf.Rad2Deg;

        //Converts the atan look angle which has negatives into 360 degrees, Taken from Stackoverflow User Liam George Betsworth
        threeSixtyLookAngle = (lookAngle + 360) % 360; 

        //Sets the weapon depending on the look angle
        Quaternion rotation = Quaternion.Euler(0, 0, lookAngle - 90f);
        weaponTransform.rotation = rotation;

        //Debug.Log("MOUSEPOSITION" + mousePos.ToString());
        //Debug.Log("ANGLE W/O -90F: " + lookAngle.ToString());
        //Debug.Log(threeSixtyLookAngle);
        return lookDirection;
    }
    //Invoked on F press, starts a coroutine to shoot
    public void Fire(InputAction.CallbackContext context)
    {
        //Debug.Log("Fire called, canceled is: " + context.canceled + " , interatction is: " + context.interaction);
        if (context.performed && context.interaction is TapInteraction)
        {
            StartCoroutine(weapon.Shoot(RetreiveMouseInfo(), playerCooldown, playerFireForce, gameObject.tag, firePoint, holdAccessibility));
        }
        else if (context.performed && context.interaction is HoldInteraction)
        {
            StartCoroutine(HoldFire(stopHoldFire));
        }
        else if (context.canceled && context.interaction is HoldInteraction)
        {
            //Debug.Log("got to stop");
            stopHoldFire = true;
            //StopCoroutine(HoldFire(STOP));
        }
    }
    IEnumerator HoldFire(bool stop)
    {
        if(!stop)
        {
        StartCoroutine(weapon.Shoot(RetreiveMouseInfo(), playerCooldown, playerFireForce, gameObject.tag, firePoint, holdAccessibility));
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(HoldFire(stopHoldFire));
        }
        else yield break;     
        stopHoldFire = false;
    }

    void FlipWeapon()
    {
        switch (lookDirection.x)
        {
            //TOP LEFT
            case float _ when lookDirection.x < -0.5 && lookDirection.y > 0.25:
                weaponSpriter.flipX = false;
                handleTransform.localPosition = weaponPositions[0];
                break;

            //TOP RIGHT
            case float _ when lookDirection.x > 0.5 && lookDirection.y > 0.25:
                weaponSpriter.flipX = true;
                handleTransform.localPosition = weaponPositions[1];
                break;

            //BOTTOM LEFT
            case float _ when lookDirection.x < -0.5 && lookDirection.y < 0.25:
                weaponSpriter.flipX = true;
                handleTransform.localPosition = weaponPositions[0];
                break;

            //BOTTOM RIGHT
            case float _ when lookDirection.x > 0.5 && lookDirection.y < 0.25:
                weaponSpriter.flipX = false;
                handleTransform.localPosition = weaponPositions[1];
                break;

            default:
                break;
        }
    }

    //Sets the sprite of the player to whichever direction they're 'looking' in
    void SetSprite()
    {
        List<Sprite> directionSpritesChosen = SelectSpriteList();

        if (directionSpritesChosen != null) //Chooses sprite to show if there are no sprites
        {
            spriteRenderer.sprite = directionSpritesChosen[0];
            Collider2DExtensions.TryUpdateShapeToAttachedSprite(playersPolygonCollider);
        }
        else
        {
            return;
        }
    }

    //Selects a sprite to change to based on player's mouse 
    List<Sprite> SelectSpriteList()
    {
        //Resets list before selecting a new one
        List<Sprite> selectedSprites = null; 

        //Switch checks where the mouse is on the screen in terms of an angle to the x axis
        switch (threeSixtyLookAngle) 
        {
            //NORTH 
            case float _ when 67.5f < threeSixtyLookAngle && threeSixtyLookAngle < 112.5f:
                //Debug.Log("NORTH");
                selectedSprites = northSprites;
                animateScript.ChangeAnimationState(playerWalkUp);
                break;

            //SOUTH 
            case float _ when 292.5f > threeSixtyLookAngle && threeSixtyLookAngle > 247.5f:
                //Debug.Log("SOUTH");
                selectedSprites = southSprites;
                animateScript.ChangeAnimationState(playerWalkDown);
                break;

            //WEST 
            case float _ when 157.5f < threeSixtyLookAngle && threeSixtyLookAngle < 202.5f: 
                //Debug.Log("WEST");
                selectedSprites = westSprites;
                animateScript.ChangeAnimationState(playerWalkLeft);
                break;

            //EAST 
            case float _ when 22.5f > threeSixtyLookAngle || threeSixtyLookAngle > 337.5f:
                //Debug.Log("EAST");
                selectedSprites = eastSprites;
                animateScript.ChangeAnimationState(playerWalkRight);
                break;

            //NORTH WEST 
            case float _ when 112.5f < threeSixtyLookAngle && threeSixtyLookAngle < 157.5f:
                //Debug.Log("NORTH WEST");
                selectedSprites = northWestSprites;
                animateScript.ChangeAnimationState(playerWalkUpLeft);
                break;

            //NORTH EAST
            case float _ when 22.5f < threeSixtyLookAngle && threeSixtyLookAngle < 67.5f:
                //Debug.Log("NORTH EAST");
                selectedSprites = northEastSprites;
                animateScript.ChangeAnimationState(playerWalkUpRight);
                break;

            //SOUTH WEST
            case float _ when 202.5f < threeSixtyLookAngle && threeSixtyLookAngle < 247.5f:
                //Debug.Log("SOUTH WEST");
                selectedSprites = southWestSprites;
                animateScript.ChangeAnimationState(playerWalkDownLeft);
                break;

            //SOUTH EAST 
            case float _ when 292.5f < threeSixtyLookAngle && threeSixtyLookAngle < 337.5f:
                //Debug.Log("SOUTH EAST");
                selectedSprites = southEastSprites;
                animateScript.ChangeAnimationState(playerWalkDownRight);
                break;

            default:
                Debug.LogWarning("Error: Look angle switch failure | lines 182 to 227");
                break;

        }
        return selectedSprites;
    }
}
