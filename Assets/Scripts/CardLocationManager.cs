using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CardLocationManager : NetworkBehaviour
{
    public readonly SyncList<Card> deckCards = new SyncList<Card>();
    public readonly SyncList<Card> discardCards = new SyncList<Card>();
    public readonly SyncList<Card> handCards = new SyncList<Card>();
    public readonly SyncList<Card> bankCards = new SyncList<Card>();

    private MiddleDeck _middleDeck;

    public override void OnStartLocalPlayer()
    {
       _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
    }

    public void SERVERDrawFromDeckToHand(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (deckCards.Count != 0)
            {
                handCards.Add(deckCards[0]);
                deckCards.RemoveAt(0); 
            }
            else
            {
                //shuffle discard into deck
                handCards.Add(deckCards[0]);
                deckCards.RemoveAt(0); 
            }
        }
    }

    public void SERVERPutCardsIntoDiscardPile()
    {
        for (int i = bankCards.Count; i > 0; i--)
        {
            discardCards.Add(bankCards[0]);
            bankCards.RemoveAt(0);
            
        }
    }

    [Command]
    public void CmdAddToDeck(Card cardToAdd,string originLocation, int originPosition)
    {
        _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
        switch (originLocation)
        {
            case "Hand":
                deckCards.Add(cardToAdd);
                handCards.RemoveAt(originPosition);
                break;
            case "Bank":
                deckCards.Add(cardToAdd);
                bankCards.RemoveAt(originPosition);
                break;
            case "Discard":
                deckCards.Add(cardToAdd);
                discardCards.RemoveAt(originPosition);
                break;
            case "Middle":
                deckCards.Add(cardToAdd);
                _middleDeck.middleBank.RemoveAt(originPosition);
                if (_middleDeck.middleDeck.Count != 0)
                {
                    _middleDeck.middleBank.Insert(originPosition,_middleDeck.middleDeck[0]);
                    _middleDeck.middleDeck.RemoveAt(0); 
                }
                break;
            default:
                deckCards.Add(cardToAdd);
                break;
        }

    }
    
    
    [Command]
    public void CmdAddToHand(Card cardToAdd, string originLocation, int originPosition)
    {
        _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
        switch (originLocation)
        {
            case "Deck":
                SERVERDrawFromDeckToHand(1);
                break;
            case "Bank":
                handCards.Add(cardToAdd);
                bankCards.RemoveAt(originPosition);
                break;
            case "Discard":
                handCards.Add(cardToAdd);
                discardCards.RemoveAt(originPosition);
                break;
            case "Middle":
                handCards.Add(cardToAdd);
                _middleDeck.middleBank.RemoveAt(originPosition);
                if (_middleDeck.middleDeck.Count != 0)
                {
                    _middleDeck.middleBank.Insert(originPosition,_middleDeck.middleDeck[0]);
                    _middleDeck.middleDeck.RemoveAt(0); 
                }
                break;
        }
    }
    
    
    [Command]
    public void CmdAddToBank(Card cardToAdd, string originLocation, int originPosition)
    {
        _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
        switch (originLocation)
        {
            case "Hand":
                bankCards.Add(cardToAdd);
                handCards.RemoveAt(originPosition);
                break;
            case "Deck":
                bankCards.Add(cardToAdd);
                deckCards.RemoveAt(originPosition);
                break;
            case "Discard":
                bankCards.Add(cardToAdd);
                discardCards.RemoveAt(originPosition);
                break;
            case "Middle":
                bankCards.Add(cardToAdd);
                _middleDeck.middleBank.RemoveAt(originPosition);
                if (_middleDeck.middleDeck.Count != 0)
                {
                    _middleDeck.middleBank.Insert(originPosition,_middleDeck.middleDeck[0]);
                    _middleDeck.middleDeck.RemoveAt(0); 
                }
                break;
        }
    }
    
    
    [Command]
    public void CmdAddToDiscard(Card cardToAdd, string originLocation, int originPosition)
    {
        _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
        switch (originLocation)
        {
            case "Hand":
                discardCards.Add(cardToAdd);
                handCards.RemoveAt(originPosition);
                break;
            case "Deck":
                discardCards.Add(cardToAdd);
                deckCards.RemoveAt(originPosition);
                break;
            case "Bank":
                discardCards.Add(cardToAdd);
                bankCards.RemoveAt(originPosition);
                break;
            case "Middle":
                discardCards.Add(cardToAdd);
                _middleDeck.middleBank.RemoveAt(originPosition);
                if (_middleDeck.middleDeck.Count != 0)
                {
                    _middleDeck.middleBank.Insert(originPosition,_middleDeck.middleDeck[0]);
                    _middleDeck.middleDeck.RemoveAt(0); 
                }
                break;
        }
    }
}
