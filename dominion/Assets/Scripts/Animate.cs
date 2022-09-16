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

    // Derived from Dani Krossing's method, plays animation without interrupting self
    public void ChangeAnimationState(string newState)
    {
        //Stops interrupting itself
        if (currentAnimState == newState) return;
        animator.Play(newState);

        //Updates states
        previousAnimState = currentAnimState;
        currentAnimState = newState;
    }

    //Goes back to previous animation after being damaged and blinked
    IEnumerator GoBackToPrevious(string damageAnim)
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Empty") == true);

        animator.Play(previousAnimState);
        currentAnimState = previousAnimState;
        previousAnimState = damageAnim;
    }

    //Plays damage animation
    public void DamageAnimation(string damageAnim)
    {
        if (currentAnimState == damageAnim) return;
        animator.Play(damageAnim);

        //Updates state values
        previousAnimState = currentAnimState;
        currentAnimState = damageAnim;

        StartCoroutine(GoBackToPrevious(damageAnim));
    }
}

