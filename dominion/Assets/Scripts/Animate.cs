using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    Animator animator;
    string currentAnimState;
    string previousAnimState;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Derived from Dani Krossing's method
    public void ChangeAnimationState(string newState)
    {
        //Stops interrupting itself
        if (currentAnimState == newState) return;
        //Plays animation
        //Debug.Log(gameObject + " CALLED ANIMATOR FOR " + newState);
        animator.Play(newState);
        previousAnimState = currentAnimState;
        //Updates state
        currentAnimState = newState;
    }

    IEnumerator GoBackToPrevious(string damageAnim)
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Empty") == true);
        //Plays previous animation state to go back to
        animator.Play(previousAnimState);
        currentAnimState = previousAnimState;
        previousAnimState = damageAnim;


    }
    public void DamageAnimation(string damageAnim)
    {
        //Stops interrupting itself
        if (currentAnimState == damageAnim) return;
        //Plays animation
        animator.Play(damageAnim);

        //Sets previous state to the current one, and the current one to the one playing
        previousAnimState = currentAnimState;
        currentAnimState = damageAnim;
        StartCoroutine(GoBackToPrevious(damageAnim));
    }
}

