using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = System.Random;

public class CardLocationManager : NetworkBehaviour
{
    public readonly SyncList<Card> deckCards = new SyncList<Card>();
    public readonly SyncList<Card> discardCards = new SyncList<Card>();
    public readonly SyncList<Card> handCards = new SyncList<Card>();
    public readonly SyncList<Card> bankCards = new SyncList<Card>();

    private MiddleDeck _middleDeck;
    private Random rndm = new Random();
    
    public override void OnStartLocalPlayer()
    {
        
    }

    public void SERVERShuffleDeck()
    {
        for (int i = deckCards.Count; i > 0; i--)
        {
            int random = rndm.Next(1, deckCards.Count);
            (deckCards[i-1], deckCards[random]) = (deckCards[random], deckCards[i-1]);
        }
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
                deckCards.Clear();
                for (int j = discardCards.Count; j > 0; j--)
                {
                    deckCards.Add(discardCards[0]);
                    discardCards.RemoveAt(0);
                }
                SERVERShuffleDeck();
                handCards.Add(deckCards[0]);
                deckCards.RemoveAt(0); 
            }
        }
    }

    public void SERVERPutBankCardsIntoDiscardPile()
    {
        for (int i = bankCards.Count; i > 0; i--)
        {
            discardCards.Add(bankCards[0]);
            bankCards.RemoveAt(0);
            
        }
    }
}
