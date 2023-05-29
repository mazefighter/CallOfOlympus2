using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerActionOnUser : NetworkBehaviour, IPointerDownHandler
{
    private GameObject _player;
    private GameObject _opponent;
    private CardLocationManager _cardLocationManager;
    private MiddleDeck _middleDeck;
    private PlayerActionToServer _playerActionToServer;
    private AtackAndGoldSum _attackAndGoldSum;
    private TextMeshProUGUI _deckCount;
    private TextMeshProUGUI _goldSum;
    private TextMeshProUGUI _attackSum;
    private TextMeshProUGUI _health;
    private GameObject _hand;
    private GameObject _bank;
    private GameObject _discard;
    private GameObject _discardPile;
    private Button _watchDiscard;
    private GameObject _scrapCanvas;
    private Button _scrapBtn;
    private TextMeshProUGUI _scrapBtnText;
    public GameObject CardToScrap;
    private TextMeshProUGUI _scrapInfo;
    private List<GameObject> middleObjects = new List<GameObject>();
    private List<GameObject> handObjects = new List<GameObject>();
    private List<GameObject> bankObjects = new List<GameObject>();
    private List<GameObject> discardObjects = new List<GameObject>();
    private List<GameObject> discardPileObjects = new List<GameObject>();
    private List<GameObject> middleObjectsToPlaceOn = new List<GameObject>();
    [SerializeField] private GameObject cardInstanciate;
    private GameObject CardToPlay;

    public bool isScrappingDiscard;
    public bool isScrappingHand;
    public bool isScrappingHandAndDeck;
    public bool scrapNothing;

    private int tempInt;
    private void Start()
    {
        _goldSum = GameObject.Find("GoldSumText").GetComponent<TextMeshProUGUI>();
        _attackSum = GameObject.Find("AttackSumText").GetComponent<TextMeshProUGUI>();
        _bank = GameObject.Find("Bank");
        _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
        
        if (isLocalPlayer)
        {
            _playerActionToServer = gameObject.GetComponent<PlayerActionToServer>();
            _cardLocationManager = gameObject.GetComponent<CardLocationManager>();
            _attackAndGoldSum = gameObject.GetComponent<AtackAndGoldSum>();
            //wenn nicht jede Karte einzeln geadded wird gibt es im Build probleme
            middleObjectsToPlaceOn.Add(GameObject.Find("MiddleCard1"));
            middleObjectsToPlaceOn.Add(GameObject.Find("MiddleCard2"));
            middleObjectsToPlaceOn.Add(GameObject.Find("MiddleCard3"));
            middleObjectsToPlaceOn.Add(GameObject.Find("MiddleCard4"));
            middleObjectsToPlaceOn.Add(GameObject.Find("MiddleCard5"));
            _middleDeck.middleBank.Callback += OnMiddleUpdate;
            _cardLocationManager.deckCards.Callback += OnDeckUpdate;
            _cardLocationManager.handCards.Callback += OnHandUpdatePlayer;
            _cardLocationManager.bankCards.Callback += OnBankUpdate;
            _cardLocationManager.discardCards.Callback += OnDiscardUpdate;
            _player = GameObject.FindWithTag("PlayerUI");
            _player.transform.SetParent(transform);
            _discard = GameObject.Find("DiscardTopCard");
            _discardPile = GameObject.Find("DiscardPile");
            _hand = GameObject.Find("Hand");
            _deckCount = GameObject.Find("DeckText").GetComponent<TextMeshProUGUI>();
            _health = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
            _scrapCanvas = GameObject.Find("ScrapCanvas");
            _scrapInfo = _scrapCanvas.GetComponentInChildren<TextMeshProUGUI>();
            _scrapBtn = _scrapCanvas.GetComponentInChildren<Button>();
            _scrapBtnText = _scrapBtn.GetComponentInChildren<TextMeshProUGUI>();

        }
        else
        {
            _cardLocationManager = gameObject.GetComponent<CardLocationManager>();
            _attackAndGoldSum = gameObject.GetComponent<AtackAndGoldSum>();
            _cardLocationManager.deckCards.Callback += OnDeckUpdate;
            _cardLocationManager.handCards.Callback += OnHandUpdateOpp;
            _cardLocationManager.bankCards.Callback += OnBankUpdate;
            _cardLocationManager.discardCards.Callback += OnDiscardUpdateOpp;
            _opponent = GameObject.FindWithTag("OpponentUI");
            _opponent.transform.SetParent(transform);
            _discard = GameObject.Find("DiscardTopCardOpp");
            _discardPile = GameObject.Find("DiscardPileOpp");
            _hand = GameObject.Find("HandOpp");
            _deckCount = GameObject.Find("DeckTextOpp").GetComponent<TextMeshProUGUI>();
            _health = GameObject.Find("HealthTextOpp").GetComponent<TextMeshProUGUI>();
        }
    }

    public void PlayCard(GameObject card)
    {
        _playerActionToServer.CmdPlayCard(handObjects.FindInstanceID(card));
    }
    
    public void SelectMiddleCard(GameObject card)
    {
        _playerActionToServer.CmdChooseMiddleCard(middleObjects.FindInstanceID(card));
    }
    private void OnMiddleUpdate(SyncList<Card>.Operation op, int itemindex, Card olditem, Card newitem)
    {
        switch (op)
        {
            case SyncList<Card>.Operation.OP_INSERT:
                GameObject card = Instantiate(cardInstanciate, middleObjectsToPlaceOn[itemindex].transform);
                DisplayCards displayCards = card.GetComponent<DisplayCards>();
                CardInteraction cardInteraction = card.GetComponent<CardInteraction>();
                cardInteraction.position = CardInteraction.PositionOnField.inMiddle;
                cardInteraction._player = gameObject;
                displayCards.SetStats(newitem);
                card.GetComponent<RectTransform>().sizeDelta = new Vector2(225, 350);
                middleObjects.Insert(itemindex, card);
                break;
            case SyncList<Card>.Operation.OP_REMOVEAT:
                Destroy(middleObjects[itemindex].gameObject);
                middleObjects.RemoveAt(itemindex);
                break;
        }
    }
    
    public void WatchDiscard()
    {
        _discardPile.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    private void OnDiscardUpdate(SyncList<Card>.Operation op, int itemindex, Card olditem, Card newitem)
    {
        switch (op)
        {
            case SyncList<Card>.Operation.OP_ADD:
                GameObject card = Instantiate(cardInstanciate, _discard.transform);
                DisplayCards displayCards = card.GetComponent<DisplayCards>();
                CardInteraction cardInteraction = card.GetComponent<CardInteraction>();
                cardInteraction.position = CardInteraction.PositionOnField.inDiscard;
                cardInteraction._player = gameObject;
                displayCards.SetStats(newitem);
                card.GetComponent<RectTransform>().sizeDelta = new Vector2(110, 185);
                discardObjects.Add(card);
                
                card = Instantiate(cardInstanciate, _discardPile.transform);
                displayCards = card.GetComponent<DisplayCards>();
                cardInteraction = card.GetComponent<CardInteraction>();
                cardInteraction.position = CardInteraction.PositionOnField.inDiscardDisplay;
                cardInteraction._player = gameObject;
                displayCards.SetStats(newitem);
                discardPileObjects.Add(card);
                
                break;
            case SyncList<Card>.Operation.OP_REMOVEAT:
                Destroy(discardObjects[itemindex].gameObject);
                discardObjects.RemoveAt(itemindex);
                Destroy(discardPileObjects[itemindex].gameObject);
                discardPileObjects.RemoveAt(itemindex);
                break;
        }
    }
    private void OnDiscardUpdateOpp(SyncList<Card>.Operation op, int itemindex, Card olditem, Card newitem)
    {
        switch (op)
        {
            case SyncList<Card>.Operation.OP_ADD:
                GameObject card = Instantiate(cardInstanciate, _discard.transform);
                DisplayCards displayCards = card.GetComponent<DisplayCards>();
                CardInteraction cardInteraction = card.GetComponent<CardInteraction>();
                cardInteraction.position = CardInteraction.PositionOnField.inOppDiscard;
                cardInteraction._player = gameObject;
                displayCards.SetStats(newitem);
                card.GetComponent<RectTransform>().sizeDelta = new Vector2(110, 185);
                discardObjects.Add(card);
                
                card = Instantiate(cardInstanciate, _discardPile.transform);
                displayCards = card.GetComponent<DisplayCards>();
                cardInteraction = card.GetComponent<CardInteraction>();
                cardInteraction.position = CardInteraction.PositionOnField.inOppDiscardDisplay;
                displayCards.SetStats(newitem);
                discardPileObjects.Add(card);
                
                break;
            case SyncList<Card>.Operation.OP_REMOVEAT:
                Destroy(discardObjects[itemindex].gameObject);
                discardObjects.RemoveAt(itemindex);
                Destroy(discardPileObjects[itemindex].gameObject);
                discardPileObjects.RemoveAt(itemindex);
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
                CardInteraction cardInteraction = card.GetComponent<CardInteraction>();
                cardInteraction.position = CardInteraction.PositionOnField.inBank;
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

    private void OnHandUpdatePlayer(SyncList<Card>.Operation op, int itemindex, Card olditem, Card newitem)
    {

        switch (op)
        {
            case SyncList<Card>.Operation.OP_ADD:
               GameObject card = Instantiate(cardInstanciate, _hand.transform);
               DisplayCards displayCards = card.GetComponent<DisplayCards>();
               CardInteraction cardInteraction = card.GetComponent<CardInteraction>();
               cardInteraction.position = CardInteraction.PositionOnField.inHand;
               cardInteraction._player = gameObject;
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
    private void OnHandUpdateOpp(SyncList<Card>.Operation op, int itemindex, Card olditem, Card newitem)
    {

        switch (op)
        {
            case SyncList<Card>.Operation.OP_ADD:
                GameObject card = Instantiate(cardInstanciate, _hand.transform);
                DisplayCards displayCards = card.GetComponent<DisplayCards>();
                CardInteraction cardInteraction = card.GetComponent<CardInteraction>();
                cardInteraction.position = CardInteraction.PositionOnField.inOppHand;
                displayCards.CardBack = true;
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
        _health.text = _attackAndGoldSum.playerHealth.ToString();
        if (CardToScrap != null)
        {
            _scrapBtnText.text = CardToScrap.GetComponent<DisplayCards>().originObject.cardname; 
        }
    }

    public void ScrapDeck(GameObject scrapCard)
    {
        scrapNothing = true;
        CardToPlay = scrapCard;
        _scrapCanvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        _scrapInfo.text = "Scrap a card from your discard pile";
        _scrapBtn.onClick.AddListener(ScrapThisCardInDiscard);
    }

    public void ScrapHand(GameObject scrapCard)
    {
        scrapNothing = true;
        CardToPlay = scrapCard;
        _scrapCanvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        _scrapInfo.text = "Scrap a card from your Hand";
        _scrapBtn.onClick.AddListener(ScrapThisCardInHand);
    }

    //Feature to add if enough time, because it could have many bugs
    
    
    // public void ScrapHandAndDeck(GameObject scrapCard)
    // {
    //     scrapNothing = true;
    //     CardToPlay = scrapCard;
    //     _scrapCanvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    //     _scrapInfo.text = "Scrap a card from your Hand or discard pile";
    //     _scrapBtn.onClick.AddListener(ScrapThisCardInHandOrDiscard);
    // }

    // private void ScrapThisCardInHandOrDiscard()
    // {
    //     throw new NotImplementedException();
    // }

    private void ScrapThisCardInHand()
    {
        CardToPlay.SetActive(true);
        if (CardToScrap != null)
        {
            tempInt = handObjects.FindInstanceID(CardToScrap);
        }
        _playerActionToServer.CmdPlayScrapCard(handObjects.FindInstanceID(CardToPlay),1,tempInt, scrapNothing);
        _scrapCanvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, 2000);
        isScrappingHand = false;
        CardToPlay = null;
        CardToScrap = null;
        _scrapBtn.onClick.RemoveListener(ScrapThisCardInHand);
        _scrapBtnText.text = "ScrapNone";
    }

    private void ScrapThisCardInDiscard()
    {
        if (CardToScrap != null)
        {
             tempInt = discardPileObjects.FindInstanceID(CardToScrap);
        }
        _playerActionToServer.CmdPlayScrapCard(handObjects.FindInstanceID(CardToPlay),2,tempInt, scrapNothing);
        _scrapCanvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, 2000);
        isScrappingDiscard = false;
        CardToPlay = null;
        CardToScrap = null;
        _scrapBtn.onClick.RemoveListener(ScrapThisCardInDiscard);
        _scrapBtnText.text = "ScrapNone";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject  == GameObject.Find("DiscardPile")||eventData.pointerCurrentRaycast.gameObject == GameObject.Find("DiscardPileOpp"))
        {
            eventData.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(2200, 2200);
        }
    }
}
