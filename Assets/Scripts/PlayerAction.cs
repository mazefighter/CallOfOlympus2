using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerAction : NetworkBehaviour
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
        for (int i = 0; i < testCards.Count; i++)
        {
            _cardLocationManager.CmdAddToDeck(testCards[i],"",0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChooseMiddleCard();
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
    
    private void ChooseMiddleCard()
    {
        if (!isLocalPlayer) return;
        //To do: Choose Mechanic drag and drop
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_middleDeck.middleBank[0].cost <= _atackAndGoldSum.PlayerGold)
            {
                GetFromMiddleToDeck(0);
                _atackAndGoldSum.PlayerGold -= _middleDeck.middleBank[0].cost;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_middleDeck.middleBank[1].cost <= _atackAndGoldSum.PlayerGold)
            {
                GetFromMiddleToDeck(1);
                _atackAndGoldSum.PlayerGold -= _middleDeck.middleBank[1].cost;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_middleDeck.middleBank[2].cost <= _atackAndGoldSum.PlayerGold)
            {
                GetFromMiddleToDeck(2);
                _atackAndGoldSum.PlayerGold -= _middleDeck.middleBank[2].cost;
            }        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (_middleDeck.middleBank[3].cost <= _atackAndGoldSum.PlayerGold)
            {
                GetFromMiddleToDeck(3);
                _atackAndGoldSum.PlayerGold -= _middleDeck.middleBank[3].cost;
            }        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (_middleDeck.middleBank[4].cost <= _atackAndGoldSum.PlayerGold)
            {
                GetFromMiddleToDeck(4);
                _atackAndGoldSum.PlayerGold -= _middleDeck.middleBank[4].cost;
            }        }
    }
    
    private void GetFromMiddleToDeck(int selection)
    {
        _cardLocationManager.CmdAddToDeck(_middleDeck.middleBank[selection], "Middle", selection);
        _middleDeck.dealToMiddleBank(selection);
    }
}
