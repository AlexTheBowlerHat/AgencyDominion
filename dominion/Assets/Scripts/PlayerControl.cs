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

    //Sprite lists, doesn't include west due to use of spriterender.flipx
    public List<Sprite> northSprites;
    public List<Sprite> northEastSprites;
    public List<Sprite> eastSprites;
    public List<Sprite> southSprites;
    public List<Sprite> southEastSprites;

    //Variables for movement
    public float walkSpeed;
    [SerializeField] Vector2 direction;

    //Variables for look direction
    [SerializeField] Vector2 lookDirection;
    [SerializeField] float lookAngle;
    [SerializeField] float threeSixtyLookAngle;
    [SerializeField] Vector2 lookDirectionUnnormalized;

    //Variables for shooting
    public Vector2 mousePos;
    public Camera mainCam;
    bool hasFired = false;
    public Transform handleTransform;
    public Vector3[] weaponPositions = {new Vector3(-0.5f,0,0),new Vector3(0.5f,0,0)};

    void Start()
    {
        //Sets up the camera and gets a reference
        mainCam = Camera.main;
        mainCam.enabled = true;
        handleTransform = transform.GetChild(0);
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
        FlipSprite();
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

    //Coroutine that fires bullet with a cooldown
    IEnumerator Shoot()
    {
        if (!hasFired)
        {
            hasFired = true;
            Vector2 lookVector = RetreiveMouseInfo();
            weapon.Fire(lookVector, gameObject.tag);
            yield return new WaitForSeconds(0.2f);
            hasFired = false;
        }
        yield break;
    }

    //Invoked on F press, starts a coroutine to shoot
    public void Fire(InputAction.CallbackContext context)
    {
        StartCoroutine(Shoot());
    }

    //Sprite Methods=======================================================
    //Flips player sprite depending on whether mouse is to left or right of the player
    void FlipSprite()
    {
        if (!spriteRenderer.flipX && lookDirection.x < 0) 
        {
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && lookDirection.x > 0) 
        {
            spriteRenderer.flipX = false;
        }
    }
    void FlipWeapon()
    {
        if (lookDirection.x < -0.5) 
        {
            //Debug.Log("LEFT CHANGE ON" + lookDirection.ToString());
            handleTransform.localPosition = weaponPositions[0];
        }
        else if (lookDirection.x > 0.5) 
        {
            //Debug.Log("RIGHT CHANGE ON"+ lookDirection.ToString());
            handleTransform.localPosition = weaponPositions[1];
        }
    }

    //Sets the sprite of the player to whichever direction they're 'looking' in
    void SetSprite()
    {
        List<Sprite> directionSpritesChosen = SelectSpriteList();

        if (directionSpritesChosen != null) //Chooses sprite to show if there are no sprites
        {
            spriteRenderer.sprite = directionSpritesChosen[0];
            //gameObject.GetComponent<CompositeCollider2D>().GenerateGeometry();
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
                break;

            //SOUTH 
            case float _ when 292.5f > threeSixtyLookAngle && threeSixtyLookAngle > 247.5f:
                //Debug.Log("SOUTH");
                selectedSprites = southSprites;
                break;

            //Could tehnically put these together west & east
            //WEST 
            case float _ when 157.5f < threeSixtyLookAngle && threeSixtyLookAngle < 202.5f: 
                //Debug.Log("WEST");
                selectedSprites = eastSprites;
                break;

            //EAST 
            case float _ when 22.5f > threeSixtyLookAngle || threeSixtyLookAngle > 337.5f:
                //Debug.Log("EAST");
                selectedSprites = eastSprites;
                break;

            //NORTH WEST 
            case float _ when 112.5f < threeSixtyLookAngle && threeSixtyLookAngle < 157.5f:
                //Debug.Log("NORTH WEST");
                selectedSprites = northEastSprites;
                break;

            //NORTH EAST
            case float _ when 22.5f < threeSixtyLookAngle && threeSixtyLookAngle < 67.5f:
                //Debug.Log("NORTH EAST");
                selectedSprites = northEastSprites;
                break;

            //SOUTH WEST
            case float _ when 202.5f < threeSixtyLookAngle && threeSixtyLookAngle < 247.5f:
                //Debug.Log("SOUTH WEST");
                selectedSprites = southEastSprites;
                break;

            //SOUTH EAST 
            case float _ when 292.5f < threeSixtyLookAngle && threeSixtyLookAngle < 337.5f:
                //Debug.Log("SOUTH EAST");
                selectedSprites = southEastSprites;
                break;

            default:
                Debug.LogWarning("Error: Look angle switch failure | lines 182 to 227");
                break;

        }
        return selectedSprites;
    }
}
