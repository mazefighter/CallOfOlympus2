using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionToServer : NetworkBehaviour
{
    private CardLocationManager _cardLocationManager;
    [SerializeField] private List<Card> testCards;
    private MiddleDeck _middleDeck;
    private TurnSystem _turnSystem;
    private AtackAndGoldSum _atackAndGoldSum;
    
    public override void OnStartLocalPlayer()
    {
        _cardLocationManager = gameObject.GetComponent<CardLocationManager>();
        _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
        _atackAndGoldSum = gameObject.GetComponent<AtackAndGoldSum>();
        _turnSystem = gameObject.GetComponent<TurnSystem>();
        for (int i = 0; i < testCards.Count; i++)
        {
            CmdAddToDeck(testCards[i]);
        }
    }
    
    public override void OnStartServer()
    {
        _cardLocationManager = gameObject.GetComponent<CardLocationManager>();
        _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
        _atackAndGoldSum = gameObject.GetComponent<AtackAndGoldSum>();
        _turnSystem = gameObject.GetComponent<TurnSystem>();
    }
    
    
    
    
    
    
    void Update()
    {
        
    }

    
    

    [Command]
    public void CmdPlayCard(int selection)
    {
        Card selectedCard = _cardLocationManager.handCards[selection];
        _cardLocationManager.bankCards.Add(selectedCard);
        
        //add cardActions like draw,discardOpponent or destroy DiscardPileCard
        if (selectedCard.drawCard)
        {
            _cardLocationManager.SERVERDrawFromDeckToHand(selectedCard.drawAmount);
        }
        _atackAndGoldSum.SERVERAddGoldAndAttack(selectedCard.attack,selectedCard.CoinGain);
        _atackAndGoldSum.SERVERAddHealth(selectedCard.heal);

        _cardLocationManager.handCards.Remove(selectedCard);
        
    }

    [Command]
    public void CmdPlayScrapCard(int selection, int scrapplace, int scrapPosition, bool scrapNothing)
    {
        Card selectedCard = _cardLocationManager.handCards[selection];
        _cardLocationManager.bankCards.Add(selectedCard);
        if (!scrapNothing)
        {
            if (scrapplace == 1)
            {
                _cardLocationManager.handCards.RemoveAt(scrapPosition);
            }

            if (scrapplace == 2)
            {
                _cardLocationManager.discardCards.RemoveAt(scrapPosition);
            }
        }
        
        //add cardActions like draw,discardOpponent or destroy DiscardPileCard
        if (selectedCard.drawCard)
        {
            _cardLocationManager.SERVERDrawFromDeckToHand(selectedCard.drawAmount);
        }
        _atackAndGoldSum.SERVERAddGoldAndAttack(selectedCard.attack,selectedCard.CoinGain);
        _atackAndGoldSum.SERVERAddHealth(selectedCard.heal);

        _cardLocationManager.handCards.Remove(selectedCard);
    }
    [Command]
    public void CmdChooseMiddleCard(int btn)
    {
        _middleDeck.SERVERGetFromMiddleToDiscard(btn, gameObject.GetComponent<NetworkIdentity>());
    }
    [Command]
    public void CmdAddToDeck(Card cardToAdd)
    {
        _cardLocationManager.deckCards.Add(cardToAdd);
    }
}
