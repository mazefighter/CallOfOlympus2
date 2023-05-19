using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ChooseCardFromMiddle : NetworkBehaviour
{
    private MiddleDeck _middleDeck;
    private CardLocationManager _cardLocationManager;
    private AtackAndGoldSum _atackAndGoldSum;
    public override void OnStartLocalPlayer()
    {
        _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
        _cardLocationManager = gameObject.GetComponent<CardLocationManager>();
        _atackAndGoldSum = gameObject.GetComponent<AtackAndGoldSum>();
    }

    // Update is called once per frame
    void Update()
    {
        ChooseMiddleCard();
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
