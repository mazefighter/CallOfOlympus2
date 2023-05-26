using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class TurnSystem : NetworkBehaviour
{
    public List<NetworkIdentity> networkList;
    private PlayerActionToServer _playerActionToServer;
    private CardLocationManager _cardLocationManager;
    [SyncVar] public bool isTurn;
    public Button ActionButton;
    public TextMeshProUGUI ActionButtonText;

    public override void OnStartLocalPlayer()
    {
        _playerActionToServer = GetComponent<PlayerActionToServer>();
        _cardLocationManager = gameObject.GetComponent<CardLocationManager>();
        ActionButton = GameObject.FindGameObjectWithTag("ActionButton").GetComponent<Button>();
        ActionButtonText = GameObject.Find("ActionButtonText").GetComponent<TextMeshProUGUI>();
        ActionButton.onClick.AddListener(ActionButtonClicked);
    }

    private void ActionButtonClicked()
    {
        if (_cardLocationManager.handCards.Count != 0)
        {
            for (int i = _cardLocationManager.handCards.Count; i > 0; i--)
            {
               _playerActionToServer.CmdPlayCard(0);
            }
        }
        else
        {
            CmdEndTurn(gameObject.GetComponent<NetworkIdentity>());
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            ActionButtonText.gameObject.SetActive(isTurn);
            ActionButton.gameObject.SetActive(isTurn);
            if (_cardLocationManager.handCards.Count!= 0)
            {
                ActionButtonText.text = "Play All Cards";
            }
            else
            {
                ActionButtonText.text = "EndTurn";
            }
        }
        if (isTurn && isOwned)
        { 
            _playerActionToServer.enabled = true;
        }
        if (!isTurn && isOwned)
        {
            _playerActionToServer.enabled = false;
        }
    }

    [Command]
    public void CmdEndTurn(NetworkIdentity id)
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            networkList.Add(player.GetComponent<NetworkIdentity>());
        }
        networkList.Remove(id);
        CardLocationManager cardLocationManager = id.gameObject.GetComponent<CardLocationManager>();
        AtackAndGoldSum atackAndGoldSum = id.gameObject.GetComponent<AtackAndGoldSum>();
        id.gameObject.GetComponent<TurnSystem>().isTurn = false;
        atackAndGoldSum.SERVERResetGoldAndAttack();
        cardLocationManager.SERVERPutBankCardsIntoDiscardPile();
        cardLocationManager.SERVERDrawFromDeckToHand(5);
        networkList[0].gameObject.GetComponent<TurnSystem>().isTurn = true;
        networkList.Clear();
        
    }
}
