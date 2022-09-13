using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Derived from Alan Thorn's Health Script
public class HealthScript : MonoBehaviour
{
    public GameObject parented;
    public string parentedTag;
    [SerializeField]
    private float startingHealth; //Health to begin
    private float maxHealth;

    private string objectTagToString;
    public bool invincible = false;
  
    public float healthPoints;
    public void UpdateHealth(float change)
    {
        //Debug.Log(invincible);
        healthPoints -= change;
        healthPoints = Mathf.Clamp(healthPoints, 0f, maxHealth); //Sets value and makes sure its between 0 and max health
        //TODO: ANIMATION BLINKING, CHANGE ALPHA TO HALF THEN BACK, RESEARCH 
        if (healthPoints <= 0f)
        {
            Debug.Log(healthPoints);
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
    }
    public void Eliminate()
    {
        objectTagToString = parentedTag.ToString();
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
