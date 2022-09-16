using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    //Derived from Speed Tutor's script
    [SerializeField] private Image[] hearts;
    float _healthPoints;
    public Sprite fullheart;
    public Sprite greyHeart;
    
    void Start()
    {
       _healthPoints = gameObject.GetComponent<HealthScript>().healthPoints;
       UpdateHearts(_healthPoints);
    }
    //Checks hearts and updates UI based on it
    public void UpdateHearts(float healthPoints)
    {
        _healthPoints = healthPoints;
        //Debug.Log("update hearts called");
        
        //Iterates through all hearts, when i is more than the health it changes the heart to gray and vice versa
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < _healthPoints)
            {   
                //Debug.Log("got to making colour, i: " + i);
                hearts[i].sprite = fullheart;
                /*
                var tempColour = hearts[i].color;
                tempColour.a = 100f;
                hearts[i].color = tempColour;
                */
            }
            else
            {
                //Debug.Log("got to unmaking colour, i:" + i);
                hearts[i].sprite = greyHeart;
                /*
                var tempColour = hearts[i].color;
                tempColour.a = 0f;
                hearts[i].color = tempColour;
                */
            }
        }
    }
}
