using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerActionOnUser : NetworkBehaviour
{
    private GameObject _player;
    private GameObject _opponent;
    private CardLocationManager _cardLocationManager;
    private AtackAndGoldSum _attackAndGoldSum;
    private TextMeshProUGUI _deckCount;
    private TextMeshProUGUI _goldSum;
    private TextMeshProUGUI _attackSum;
    private GameObject _hand;
    private GameObject _bank;
    private GameObject _discard;
    private List<GameObject> handObjects = new List<GameObject>();
    private List<GameObject> bankObjects = new List<GameObject>();
    private List<GameObject> discardObjects = new List<GameObject>();
    [SerializeField] private GameObject cardInstanciate;
    

    private void Start()
    {
        _goldSum = GameObject.Find("GoldSumText").GetComponent<TextMeshProUGUI>();
        _attackSum = GameObject.Find("AttackSumText").GetComponent<TextMeshProUGUI>();
        _bank = GameObject.Find("Bank");
        if (isLocalPlayer)
        {
            _cardLocationManager = gameObject.GetComponent<CardLocationManager>();
            _attackAndGoldSum = gameObject.GetComponent<AtackAndGoldSum>();
            _cardLocationManager.deckCards.Callback += OnDeckUpdate;
            _cardLocationManager.handCards.Callback += OnHandUpdate;
            _cardLocationManager.bankCards.Callback += OnBankUpdate;
            _cardLocationManager.discardCards.Callback += OnDiscardUpdate;
            _player = GameObject.FindWithTag("PlayerUI");
            _player.transform.SetParent(transform);
            _discard = GameObject.Find("DiscardTopCard");
            _hand = GameObject.Find("Hand");
            _deckCount = GameObject.Find("DeckText").GetComponent<TextMeshProUGUI>();
        }
        else
        {
            _cardLocationManager = gameObject.GetComponent<CardLocationManager>();
            _attackAndGoldSum = gameObject.GetComponent<AtackAndGoldSum>();
            _cardLocationManager.deckCards.Callback += OnDeckUpdate;
            _cardLocationManager.handCards.Callback += OnHandUpdate;
            _cardLocationManager.bankCards.Callback += OnBankUpdate;
            _cardLocationManager.discardCards.Callback += OnDiscardUpdate;
            _opponent = GameObject.FindWithTag("OpponentUI");
            _opponent.transform.SetParent(transform);
            _discard = GameObject.Find("DiscardTopCardOpp");
            _hand = GameObject.Find("HandOpp");
            _deckCount = GameObject.Find("DeckTextOpp").GetComponent<TextMeshProUGUI>();
        }
    }

    private void OnDiscardUpdate(SyncList<Card>.Operation op, int itemindex, Card olditem, Card newitem)
    {
        switch (op)
        {
            case SyncList<Card>.Operation.OP_ADD:
                GameObject card = Instantiate(cardInstanciate, _discard.transform);
                DisplayCards displayCards = card.GetComponent<DisplayCards>();
                displayCards.SetStats(newitem);
                card.GetComponent<RectTransform>().sizeDelta = new Vector2(110, 185);
                discardObjects.Add(card);
                break;
            case SyncList<Card>.Operation.OP_REMOVEAT:
                // index is where it was removed from the list
                // oldItem is the item that was removed
                Destroy(discardObjects[itemindex].gameObject);
                discardObjects.RemoveAt(itemindex);
                break;
        }
    }

    private void OnBankUpdate(SyncList<Card>.Operation op, int itemindex, Card olditem, Card newitem)
    {
        switch (op)
        {
            case SyncList<Card>.Operation.OP_ADD:
                GameObject card = Instantiate(cardInstanciate, _bank.transform);
                DisplayCards displayCards = card.GetComponent<DisplayCards>();
                displayCards.SetStats(newitem);
                bankObjects.Add(card);
                break;
            case SyncList<Card>.Operation.OP_REMOVEAT:
                // index is where it was removed from the list
                // oldItem is the item that was removed
                Destroy(bankObjects[itemindex].gameObject);
                bankObjects.RemoveAt(itemindex);
                break;
        }
    }

    private void OnHandUpdate(SyncList<Card>.Operation op, int itemindex, Card olditem, Card newitem)
    {

        switch (op)
        {
            case SyncList<Card>.Operation.OP_ADD:
               GameObject card = Instantiate(cardInstanciate, _hand.transform);
               DisplayCards displayCards = card.GetComponent<DisplayCards>();
               displayCards.SetStats(newitem);
               handObjects.Add(card);
                break;
            case SyncList<Card>.Operation.OP_REMOVEAT:
                // index is where it was removed from the list
                // oldItem is the item that was removed
               Destroy(handObjects[itemindex].gameObject);
                handObjects.RemoveAt(itemindex);
                break;
        }
    }

    private void OnDeckUpdate(SyncList<Card>.Operation op, int itemindex, Card olditem, Card newitem)
    {
       _deckCount.text = _cardLocationManager.deckCards.Count.ToString();
    }

    public void AttackAndGold(int attack, int gold)
    {
        _attackSum.text = attack.ToString();
        _goldSum.text = gold.ToString();
    }
    void Update()
    {
        
    }
}
