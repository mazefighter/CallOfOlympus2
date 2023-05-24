using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class TurnSystem : NetworkBehaviour
{
    public List<NetworkIdentity> networkList;
    private PlayerAction _playerAction;
    [SyncVar] public bool isTurn;

    public override void OnStartLocalPlayer()
    {
        _playerAction = GetComponent<PlayerAction>();
        
    }

    private void Update()
    {
        if (isTurn && isOwned)
        {
            _playerAction.enabled = true;
            
        }
        if (!isTurn && isOwned)
        {
            _playerAction.enabled = false;
            
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
        atackAndGoldSum.playerGold = 0;
        atackAndGoldSum.playerAttack = 0;
        cardLocationManager.SERVERPutCardsIntoDiscardPile();
        cardLocationManager.SERVERDrawFromDeckToHand(5);
        networkList[0].gameObject.GetComponent<TurnSystem>().isTurn = true;
        networkList.Clear();
        
    }
}
