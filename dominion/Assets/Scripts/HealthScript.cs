using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Derived from Alan Thorn's Health Script
public class HealthScript : MonoBehaviour
{
    public GameObject parented;
    public string parentedTag;
    [SerializeField]
    private float startingHealth; //Health to begin
    private float maxHealth;
    [SerializeField]
    private float _healthPoints = 100f;
    private string objectTagToString;
    public bool invincible = false;

    public float healthPoints //Updates and clamps health points
    {
        get { return _healthPoints; }
        set
        {
            _healthPoints = Mathf.Clamp(value, 0f, maxHealth); //Sets value and makes sure its between 0 and 100
            if (_healthPoints <= 0f)
            {
                Eliminate(); //Kill Method, differs between player and enemy
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        healthPoints = startingHealth;
        maxHealth = startingHealth;
        parentedTag = gameObject.tag;
    }
    public void Eliminate()
    {
        objectTagToString = parentedTag.ToString();
        Debug.Log("Bye: " + objectTagToString);
       // _healthPoints -= 10f;
    }
       
}
