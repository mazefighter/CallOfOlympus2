using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MiddleDeck : NetworkBehaviour
{
    
    public readonly SyncList<Card> middleDeck = new SyncList<Card>();
    public readonly SyncList<Card> middleBank = new SyncList<Card>();

    [SerializeField] private List<Card> DeckCards;
    private CardLocationManager _cardLocation;
    private AtackAndGoldSum _atackAndGoldSum;

    public override void OnStartServer()
    {
        for (int i = 0; i < DeckCards.Count; i++)
        {
            middleDeck.Add(DeckCards[i]);
        }

        for (int i = 0; i < 5; i++)
        {
            DealToMiddleBank(i);
        }
        
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void SERVERGetFromMiddleToDiscard(int selection, NetworkIdentity id)
    {
        _cardLocation = id.gameObject.GetComponent<CardLocationManager>();
        _atackAndGoldSum = id.gameObject.GetComponent<AtackAndGoldSum>();
        _cardLocation.discardCards.Add(middleBank[selection]);
        _atackAndGoldSum.SERVERRemoveGoldAndAttack(0,middleBank[selection].cost);
        middleBank.RemoveAt(selection);
        if (middleDeck.Count != 0)
        {
            middleBank.Insert(selection,middleDeck[0]);
            middleDeck.RemoveAt(0);
        }
    }
    public void SERVERGetFromMiddleToHand(int selection, NetworkIdentity id)
    {
        _cardLocation = id.gameObject.GetComponent<CardLocationManager>();
        _cardLocation.handCards.Add(middleBank[selection]);
        middleBank.RemoveAt(selection);
        middleBank.Insert(selection,middleDeck[0]);
        middleDeck.RemoveAt(0);
    }
    
    private void DealToMiddleBank(int position)
    {
        middleBank.Insert(position,middleDeck[0]);
        middleDeck.RemoveAt(0);
    }
}
