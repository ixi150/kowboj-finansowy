using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiningManager : MonoBehaviour
{
    [SerializeField] Transform characterSlot;
    [SerializeField] Transform rockSlot;
    [SerializeField, Range(0.5f, 10)] float timeout = 3;
    [SerializeField] UnityEvent onSuccessfulHit;
    [SerializeField] UnityEvent onFailedHit;

    Animator animator;
    Coroutine timeoutCoroutine;
    bool wasSuccess;

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
        if (animator.IsInTransition(0))
        {
            return;
        }

        if (state.IsTag("Idle"))
        {
            animator.SetTrigger(animatorJump);
            timeoutCoroutine = StartCoroutine(Timeout());
        }
        else if (state.IsTag("Ready"))
        {
            wasSuccess = true; //todo
            animator.SetTrigger(animatorStrike);
            if (timeoutCoroutine != null)
            {
                StopCoroutine(timeoutCoroutine);
            }
        }
    }

    public void OnHit()
    {
        if (wasSuccess)
        {
            onSuccessfulHit.Invoke();
        }
        else
        {
            onFailedHit.Invoke();
        }
    }

    IEnumerator Timeout()
    {
        yield return new WaitForSeconds(timeout);
        animator.SetTrigger(animatorLand);
    }

}
