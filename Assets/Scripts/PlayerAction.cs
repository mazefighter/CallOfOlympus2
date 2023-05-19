using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerAction : NetworkBehaviour
{
    private CardLocationManager _cardLocationManager;
    [SerializeField] private List<Card> testCards;
    private MiddleDeck _middleDeck;
    public override void OnStartLocalPlayer()
    {
        _cardLocationManager = gameObject.GetComponent<CardLocationManager>();
        _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
        for (int i = 0; i < testCards.Count; i++)
        {
            _cardLocationManager.CmdAddToDeck(testCards[i],"",0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)&& isLocalPlayer)
        {
           DrawFromDeck(1);
        }
    }

    
    private void DrawFromDeck(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _cardLocationManager.CmdAddToHand(_cardLocationManager.deckCards[0], "Deck", 0);
        }
    }
}
