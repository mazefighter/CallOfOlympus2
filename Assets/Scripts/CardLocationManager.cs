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


    [Command]
    public void CmdAddToDeck(Card cardToAdd,string originLocation, int originPosition)
    {
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
                break;
            default:
                deckCards.Add(cardToAdd);
                break;
        }

    }
    
    
    [Command]
    public void CmdAddToHand(Card cardToAdd, string originLocation, int originPosition)
    {
        switch (originLocation)
        {
            case "Deck":
                handCards.Add(cardToAdd);
                deckCards.RemoveAt(originPosition);
                break;
            case "Bank":
                handCards.Add(cardToAdd);
                bankCards.RemoveAt(originPosition);
                break;
            case "Discard":
                handCards.Add(cardToAdd);
                discardCards.RemoveAt(originPosition);
                break;
        }
    }
    
    
    [Command]
    public void CmdAddToBank(Card cardToAdd, string originLocation, int originPosition)
    {
        switch (originLocation)
        {
            case "Hand":
                handCards.Add(cardToAdd);
                deckCards.RemoveAt(originPosition);
                break;
            case "Deck":
                handCards.Add(cardToAdd);
                bankCards.RemoveAt(originPosition);
                break;
            case "Discard":
                handCards.Add(cardToAdd);
                discardCards.RemoveAt(originPosition);
                break;
        }
    }
    
    
    [Command]
    public void CmdAddToDiscard(Card cardToAdd, string originLocation, int originPosition)
    {
        switch (originLocation)
        {
            case "Hand":
                handCards.Add(cardToAdd);
                deckCards.RemoveAt(originPosition);
                break;
            case "Deck":
                handCards.Add(cardToAdd);
                bankCards.RemoveAt(originPosition);
                break;
            case "Bank":
                handCards.Add(cardToAdd);
                discardCards.RemoveAt(originPosition);
                break;
        }
    }
    
}
