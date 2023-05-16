using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DisplayCards : MonoBehaviour
{
    public int cost;
    [SerializeField] private int health;
    [SerializeField] private int atack;

    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI AtackText;
    [SerializeField] private TextMeshProUGUI CostText;
    [SerializeField] private Image GodBorder;

    public bool CardBack;
    public static bool staticCardback;
    

    private void Awake()
    {
        
    }

    private void Update()
    {
        staticCardback = CardBack;
    }

    public void SetStats(Card card)
    {
        
        cost = card.cost;
        atack = card.attack;
        NameText.text = card.Cardname;
        AtackText.text = card.attack.ToString();
        CostText.text = card.cost.ToString();

        switch (card.god)
        {
            case Card.God.Zeus:
                GodBorder.color = Color.yellow;
                break;
            case Card.God.Hades:
                GodBorder.color = Color.green;
                break;
            case Card.God.Poseidon:
                GodBorder.color = Color.blue;
                break;
                
        }
    }
}
