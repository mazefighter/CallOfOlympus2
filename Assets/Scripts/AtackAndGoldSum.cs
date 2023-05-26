using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AtackAndGoldSum : NetworkBehaviour
{
    [SyncVar]
    public int playerAttack;
    [SyncVar]
    public int playerGold;

    
    public void SERVERAddGoldAndAttack(int attack, int gold)
    {
        playerGold += gold;
        playerAttack += attack;
        RpcAtackAndGoldChanged(playerAttack, playerGold);
    }

    public void SERVERRemoveGoldAndAttack(int attack, int gold)
    {
        playerGold -= gold;
        playerAttack -= attack;
        RpcAtackAndGoldChanged(playerAttack, playerGold);
    }

    public void SERVERResetGoldAndAttack()
    {
        playerGold = 0;
        playerAttack = 0;
        RpcAtackAndGoldChanged(playerAttack, playerGold);
    }

    [ClientRpc]
    private void RpcAtackAndGoldChanged(int attack, int gold)
    {
        PlayerActionOnUser playerActionOnUser = gameObject.GetComponent<PlayerActionOnUser>();
        playerActionOnUser.AttackAndGold(attack, gold);
    }
}
