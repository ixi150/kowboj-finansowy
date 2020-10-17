using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const string goldKey = "gold_amount";
    private const string diamondKey = "diamond_amount";

    [SerializeField] Text goldText;
    [SerializeField] Text diamondText;
    [SerializeField] int gold;
    [SerializeField] int diamonds;

    public static GameManager Ref { get; private set; }

    public int Gold
    {
        get => gold;
        set => gold = value;
    }

    public int Diamonds
    {
        get => diamonds;
        set => diamonds = value;
    }

    public void ResetPlayerProfile()
    {
        Gold = Diamonds = 0;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }

    public void AddDiamonds(int amount)
    {
        Diamonds += amount;
    }

    private void Awake()
    {
        Ref = this;
    }

    private void OnEnable()
    {
        Gold = PlayerPrefs.GetInt(goldKey);
        Diamonds = PlayerPrefs.GetInt(diamondKey);
    }

    private void OnDisable()
    {
         PlayerPrefs.SetInt(goldKey, Gold);
        PlayerPrefs.SetInt(diamondKey, Diamonds);
    }

    void Update()
    {
        goldText.text = gold.ToString();
        diamondText.text = diamonds.ToString();
    }
}
