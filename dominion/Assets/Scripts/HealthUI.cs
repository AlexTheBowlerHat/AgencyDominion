using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    //Derived from Speed Tutor's script
    [SerializeField] private Image[] hearts;
    float healthPoints;
    
    void Start()
    {
       healthPoints = gameObject.GetComponent<HealthScript>().healthPoints;
       UpdateHearts();
    }
    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < healthPoints)
            {   
                var tempColour = hearts[i].color;
                tempColour.a = 100f;
                hearts[i].color = tempColour;
            }
            else
            {
                var tempColour = hearts[i].color;
                tempColour.a = 0f;
                hearts[i].color = tempColour;
            }
        }
    }
}
