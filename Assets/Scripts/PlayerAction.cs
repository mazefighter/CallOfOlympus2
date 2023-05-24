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
        _turnSystem = gameObject.GetComponent<TurnSystem>();
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

        if (Input.GetKeyDown(KeyCode.Z)&& isLocalPlayer)
        {
            if (_cardLocationManager.handCards.Count != 0)
            {
                for (int i = _cardLocationManager.handCards.Count; i > 0; i--)
                {
                    CmdPlayCard(0);
                }
            }
            else
            {
                _turnSystem.CmdEndTurn(gameObject.GetComponent<NetworkIdentity>());
            }
        }

        #region ChooseMiddleCard

        //To do: Choose Mechanic drag and drop
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)&& _middleDeck.middleBank.Count > 0)
            {
                CmdChooseMiddleCard(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)&& _middleDeck.middleBank.Count > 1)
            {
                CmdChooseMiddleCard(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3)&& _middleDeck.middleBank.Count > 2)
            {
                CmdChooseMiddleCard(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4)&& _middleDeck.middleBank.Count > 3)
            {
                CmdChooseMiddleCard(3);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5)&& _middleDeck.middleBank.Count > 4)
            {
                CmdChooseMiddleCard(4);
            }
        }

        #endregion
    }

    
    public void DrawFromDeck(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _cardLocationManager.CmdAddToHand(_cardLocationManager.deckCards[0], "Deck", 0);
        }
    }

    [Command]
    public void CmdPlayCard(int selection)
    {
        _atackAndGoldSum = gameObject.GetComponent<AtackAndGoldSum>();
        _cardLocationManager = gameObject.GetComponent<CardLocationManager>();
        Card selectedCard = _cardLocationManager.handCards[selection];
        _cardLocationManager.bankCards.Add(selectedCard);
        _atackAndGoldSum.SERVERAddGold(selectedCard.CoinGain);
        _atackAndGoldSum.SERVERAddAttack(selectedCard.attack);
        _cardLocationManager.handCards.Remove(selectedCard);
        
    }
    [Command]
    private void CmdChooseMiddleCard(int btn)
    {
        _atackAndGoldSum = gameObject.GetComponent<AtackAndGoldSum>();
        _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
        if (_middleDeck.middleBank[btn].cost > _atackAndGoldSum.playerGold) return;
        RpcGetFromMiddleToDiscard(btn);
        _atackAndGoldSum.playerGold -= _middleDeck.middleBank[btn].cost;
    }
    [ClientRpc]
    private void RpcGetFromMiddleToDiscard(int selection)
    {
        if (isLocalPlayer)
        {
            _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
            _cardLocationManager = gameObject.GetComponent<CardLocationManager>();
            _cardLocationManager.CmdAddToDiscard(_middleDeck.middleBank[selection], "Middle", selection);
        }
    }
}
