using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Derived from Alan Thorn's Health Script
public class HealthScript : MonoBehaviour
{
    public string parentedTag;
    [SerializeField]
    private float startingHealth; 
    private float maxHealth;
    public float healthPoints;
    [SerializeField] private HealthUI healthUI;
    private string objectTagToString;
    public bool invincible = false;
    Animate animateScript;
    //Called whenever a health change is needed
    public void UpdateHealth(float change)
    {
        //Changes health
        healthPoints -= change;
        healthPoints = Mathf.Clamp(healthPoints, 0f, maxHealth); //Makes sure health is in the correct range

        //Debug.Log("got here updating health");
        if (animateScript == null) return;
        //Debug.Log("made it past null check");

        //Animations
        if (objectTagToString == "Player")
        {
            //Debug.Log("Player calling");
            animateScript.DamageAnimation("playerBlink");
            healthUI.UpdateHearts(healthPoints);
        }
        else if (objectTagToString == "Enemy")
        {
            animateScript.DamageAnimation("EnemyBlink");
        }

        //Death check
        if (healthPoints <= 0f)
        {
            Eliminate();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(startingHealth);
        maxHealth = startingHealth;
        healthPoints = startingHealth;
        parentedTag = gameObject.tag;
        animateScript = gameObject.GetComponent<Animate>();
        objectTagToString = parentedTag.ToString();
    }
    //Destroys or reloads scene depending on object being killed
    public void Eliminate()
    {
        //Debug.Log("Bye: " + objectTagToString);
        if (objectTagToString == "Enemy")
        {
            Destroy(gameObject);
        }
        else if(objectTagToString == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
       
}
