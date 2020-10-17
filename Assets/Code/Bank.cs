using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bank : MonoBehaviour
{
    [SerializeField] public float basePercentage = 7.2f;
    [SerializeField] public float stepPercentageDiscount = .5f;
    [SerializeField] public int maxStepsPerMonth = 10;
    [SerializeField] public int costOfStep = 10;
    [SerializeField] public int diamondDayLimit = 3;
    [SerializeField] public int diamondLimitBonus = 1;

    [SerializeField] Text percentageText;
    [SerializeField] Text discountText;
    [SerializeField] Text discountCostText;
    [SerializeField] Button discountButton;
    [SerializeField] Slider stepsSlider;

    [SerializeField] public int boughtSteps;

    private const string boughtDiscountStepsKey = "boughtDiscountSteps";

    public void TryBuyDiscount()
    {
        if (GameManager.Ref.Diamonds >= costOfStep && boughtSteps < maxStepsPerMonth)
        {
            GameManager.Ref.Diamonds -= costOfStep;
            boughtSteps++;
        }
    }

    private void Update()
    {
        percentageText.text = $"{basePercentage - stepPercentageDiscount*boughtSteps:0.00}%";
        stepsSlider.value = (float)boughtSteps / maxStepsPerMonth;
        discountCostText.text = $"{costOfStep}x";
        bool isMaxDiscount = boughtSteps < maxStepsPerMonth;
        bool isDiscountAffordable = GameManager.Ref.Diamonds >= costOfStep;
        discountButton.interactable = isMaxDiscount && isDiscountAffordable;
        discountText.text = isMaxDiscount
            ? $"-{stepPercentageDiscount}%"
            : "max!";
    }

    private void OnEnable()
    {
        boughtSteps = PlayerPrefs.GetInt(boughtDiscountStepsKey);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(boughtDiscountStepsKey, boughtSteps);
    }
}
