using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Derived from Alan Thorn's Health Script
public class HealthScript : MonoBehaviour
{
    public string parentedTag;
    [SerializeField]
    private float startingHealth; //Health to begin
    private float maxHealth;

    private string objectTagToString;
    public bool invincible = false;
  
    public float healthPoints;
    Animate animateScript;
    public void UpdateHealth(float change)
    {
        //Debug.Log(invincible);
        healthPoints -= change;
        healthPoints = Mathf.Clamp(healthPoints, 0f, maxHealth); //Sets value and makes sure its between 0 and max health

        //Debug.Log("got here updating health");
        if (animateScript == null) return;
        //Debug.Log("made it past null check");

        if (objectTagToString == "Player")
        {
            //Debug.Log("Player calling");
            animateScript.DamageAnimation("playerBlink");
        }
        else if (objectTagToString == "Enemy")
        {
            animateScript.DamageAnimation("EnemyBlink");
        }

        if (healthPoints <= 0f)
        {
            //Debug.Log(healthPoints);
            Eliminate(); //Kill Method, differs between player and enemy
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
    public void Eliminate()
    {
        Debug.Log("Bye: " + objectTagToString);
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
