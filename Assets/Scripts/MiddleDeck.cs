using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MiddleDeck : NetworkBehaviour
{
    
    public readonly SyncList<Card> middleDeck = new SyncList<Card>();
    public readonly SyncList<Card> middleBank = new SyncList<Card>();

    [SerializeField] private List<Card> DeckCards;

    public override void OnStartServer()
    {
        for (int i = 0; i < DeckCards.Count; i++)
        {
            middleDeck.Add(DeckCards[i]);
        }

        for (int i = 0; i < 5; i++)
        {
            dealToMiddleBank(i);
        }
        
    }

    public void dealToMiddleBank(int postition)
    {
            middleBank.Insert(postition,middleDeck[0]);
            middleDeck.RemoveAt(0);
    }
    void Update()
    {
        
    }
}
