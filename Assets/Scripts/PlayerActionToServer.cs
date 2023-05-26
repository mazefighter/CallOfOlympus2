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

        #region ChooseHandCard
        //To do: Choose Mechanic drag and drop
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) && _cardLocationManager.handCards.Count > 0)
            {
                CmdPlayCard(0);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2) && _cardLocationManager.handCards.Count > 1)
            {
                CmdPlayCard(1);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3) && _cardLocationManager.handCards.Count > 2)
            {
                CmdPlayCard(2);
            }
            if (Input.GetKeyDown(KeyCode.Keypad4) && _cardLocationManager.handCards.Count > 3)
            {
                CmdPlayCard(3);
            }
            if (Input.GetKeyDown(KeyCode.Keypad5) && _cardLocationManager.handCards.Count > 4)
            {
                CmdPlayCard(4);
            }
            if (Input.GetKeyDown(KeyCode.Keypad6) && _cardLocationManager.handCards.Count > 5)
            {
                CmdPlayCard(5);
            }
            if (Input.GetKeyDown(KeyCode.Keypad7) && _cardLocationManager.handCards.Count > 6)
            {
                CmdPlayCard(6);
            }
            if (Input.GetKeyDown(KeyCode.Keypad8) && _cardLocationManager.handCards.Count > 7)
            {
                CmdPlayCard(7);
            }
            if (Input.GetKeyDown(KeyCode.Keypad9) && _cardLocationManager.handCards.Count > 8)
            {
                CmdPlayCard(8);
            }
        }
        

        #endregion
    }

    
    

    [Command]
    public void CmdPlayCard(int selection)
    {
        Card selectedCard = _cardLocationManager.handCards[selection];
        _cardLocationManager.bankCards.Add(selectedCard);
        
        //add cardActions like draw,discardOpponent or destroy DiscardPileCard
        
        _atackAndGoldSum.SERVERAddGoldAndAttack(selectedCard.attack,selectedCard.CoinGain);


        _cardLocationManager.handCards.Remove(selectedCard);
        
    }
    [Command]
    private void CmdChooseMiddleCard(int btn)
    {
        if (_middleDeck.middleBank[btn].cost > _atackAndGoldSum.playerGold) return;
        _middleDeck.SERVERGetFromMiddleToDiscard(btn, gameObject.GetComponent<NetworkIdentity>());
    }
    [Command]
    public void CmdAddToDeck(Card cardToAdd)
    {
        _cardLocationManager.deckCards.Add(cardToAdd);
    }
}
