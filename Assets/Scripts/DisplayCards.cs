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

    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI AttackText;
    [SerializeField] private TextMeshProUGUI CostText;
    [SerializeField] private TextMeshProUGUI coinGainText;
    [SerializeField] private TextMeshProUGUI healText;
    [SerializeField] private List<Sprite> _cardImges;
    [SerializeField] private ScriptableObject originObject;
    

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
        NameText.text = cardName;
        AttackText.text = attack.ToString();
        CostText.text = cost.ToString();
        healText.text = heal.ToString();
        coinGainText.text = coinGain.ToString();

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
        }

        if (CardBack)
        {
            cardBack.enabled = true;
        }
    }
}
