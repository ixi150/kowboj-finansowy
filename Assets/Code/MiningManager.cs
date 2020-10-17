using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiningManager : MonoBehaviour
{
    [SerializeField] Transform characterSlot;
    [SerializeField] Transform rockSlot;
    [SerializeField] GameObject rock;
    [SerializeField, Range(0.5f, 10)] float timeout = 3;
    [SerializeField, Range(0, 5)] float minigameDelay = 1;
    [SerializeField, Range(0, 1)] float successThreshold = .5f;
    [SerializeField] float maxHp = 0.9f;
    [SerializeField] UnityEvent onSuccessfulHit;
    [SerializeField] UnityEvent onRockDestroyHit;
    [SerializeField] UnityEvent onFailedHit;
    [SerializeField] float goldChance = 0.8f;
    [SerializeField] int goldRols = 3;
    [SerializeField] float diamondChance = 0.1f;

    [Header("UI")]
    [SerializeField] RectTransform uiContainer;
    [SerializeField] RectTransform uiCursor;
    [SerializeField] float uiSpeed = 1;

    Animator animator;
    Coroutine timeoutCoroutine;
    Coroutine minigameCoroutine;
    bool wasSuccess;
    float offset;
    float currentHp;

    private const string animatorJump = "Jump";
    private const string animatorLand = "Land";
    private const string animatorStrike = "Strike";

    private void Awake()
    {
        offset = uiContainer.sizeDelta.x / 2;
        uiContainer.gameObject.SetActive(false);
        animator = characterSlot.GetComponentInChildren<Animator>(true);
    }

    void DestroyRock()
    {
        rock.SetActive(false);
        StartCoroutine(RespawnRock());
        onRockDestroyHit.Invoke();

        for (var i = 0; i < goldRols; i++)
        {
            if (TestLuck(goldChance))
            {
                GameManager.Ref.Gold++;
            }
        }

        if (TestLuck(diamondChance))
        {
            GameManager.Ref.Diamonds++;
        }
    }

    bool TestLuck(float chance)
    {
        return Random.Range(0f, 1f) < chance;
    }

    void ResetRock()
    {
        currentHp = maxHp;
        rock.SetActive(true);
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
            minigameCoroutine = StartCoroutine(Minigame());
        }
        else if (uiContainer.gameObject.activeSelf)
        {
            var accuracy = 1 - (Mathf.Abs(uiCursor.anchoredPosition.x) / offset);
            wasSuccess = accuracy > successThreshold;
            if (wasSuccess)
            {
                currentHp -= accuracy;
            }

            animator.SetTrigger(animatorStrike);
            StopAllCoroutines();
            StopMinigame();
        }
    }

    public void OnHit()
    {
        if (wasSuccess)
        {
            onSuccessfulHit.Invoke();
            if (currentHp <=0)
            {
                DestroyRock();
            }
        }
        else
        {
            onFailedHit.Invoke();
        }
    }

    IEnumerator RespawnRock()
    {
        yield return new WaitForSeconds(.8f);
        ResetRock();
    }

    IEnumerator Timeout()
    {
        yield return new WaitForSeconds(timeout);
        animator.SetTrigger(animatorLand);
        StopMinigame();
        StopAllCoroutines();
    }

    IEnumerator Minigame()
    {
        yield return new WaitForSeconds(minigameDelay);
        uiContainer.gameObject.SetActive(true);
        float elapsed = Random.Range(0, 10 * uiSpeed);
        while (true)
        {
            elapsed += Time.deltaTime;
            uiCursor.anchoredPosition = new Vector3(Mathf.PingPong(elapsed * uiSpeed, offset * 2) - offset, 0);
            yield return null;
        }
    }

    void StopMinigame()
    {
        uiContainer.gameObject.SetActive(false);
    }

}
