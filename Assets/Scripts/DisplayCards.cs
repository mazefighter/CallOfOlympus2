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
    [SerializeField] private string cardName;
    [SerializeField] private int health;
    [SerializeField] private int attack;
    [SerializeField] private int coinGain;
    [SerializeField] private int heal;
    [SerializeField] private string flavourText;

    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI AttackText;
    [SerializeField] private TextMeshProUGUI CostText;
    [SerializeField] private TextMeshProUGUI coinGainText;
    [SerializeField] private TextMeshProUGUI healText;
    [SerializeField] private TextMeshProUGUI FlavourText;
    [SerializeField] private List<Sprite> _cardImges;
    [SerializeField] public Card originObject;
    

    private Image _image;

    public bool CardBack;
   [SerializeField] private Image cardBack;


    private void Awake()
    {
        _image = gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        
    }

    public void SetStats(Card card)
    {
        originObject = card;
        cost = card.cost;
        attack = card.attack;
        coinGain = card.CoinGain;
        heal = card.heal;
        cardName = card.cardname;
        flavourText = card.flavourText;
        NameText.text = cardName;
        AttackText.text = attack.ToString();
        CostText.text = cost.ToString();
        healText.text = heal.ToString();
        coinGainText.text = coinGain.ToString();
        FlavourText.text = flavourText;

        switch (card.god)
        {
            case Card.God.none:
                _image.sprite = _cardImges[0];
                break;
            case Card.God.Zeus:
                _image.sprite = _cardImges[1];
                break;
            case Card.God.Hades:
                _image.sprite = _cardImges[2];
                break;
            case Card.God.Poseidon:
                _image.sprite = _cardImges[3];
                break;
            case Card.God.Ares:
                _image.sprite = _cardImges[4];
                break;
        }

        if (CardBack)
        {
            cardBack.enabled = true;
        }
    }
}
