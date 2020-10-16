using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningManager : MonoBehaviour
{
    [SerializeField] Transform characterSlot;
    [SerializeField] Transform rockSlot;
    [SerializeField, Range(0.5f, 10)] float timeout = 3;
    
    Animator animator;
    Coroutine timeoutCoroutine;

    private const string animatorJump = "Jump";
    private const string animatorLand = "Land";
    private const string animatorStrike = "Strike";

    private void Awake()
    {
        animator = characterSlot.GetComponentInChildren<Animator>(true);
    }

    public void Click()
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsTag("Idle"))
        {
            animator.SetTrigger(animatorJump);
            timeoutCoroutine = StartCoroutine(Timeout());
        }
        else if (state.IsTag("Ready"))
        {
            animator.SetTrigger(animatorStrike);
            if (timeoutCoroutine != null)
            {
                StopCoroutine(timeoutCoroutine);
            }
        }
    }

    public void OnHit()
    {

    }

    IEnumerator Timeout()
    {
        yield return new WaitForSeconds(timeout);
        animator.SetTrigger(animatorLand);
    }

}
