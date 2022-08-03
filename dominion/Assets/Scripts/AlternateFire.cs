using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateFire : MonoBehaviour
{
    // Start is called before the first frame update
    float timeAfterLastShot = 0f;
    const float fireCooldown = 0.2f;

    // Update is called once per frame
    void Update()
    {
        timeAfterLastShot += Time.deltaTime;

    
    }

    //Only need to check when the palyer is trying to shoot
    //Called by input even
    void Fire()
    {
        if (timeAfterLastShot >= fireCooldown)
        {
            timeAfterLastShot = 0f;
            Debug.Log("UGG");
        }
    }
}
