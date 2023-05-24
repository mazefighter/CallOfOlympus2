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

    
    public void SERVERAddGold(int gold)
    {
        playerGold += gold;
    }

    
    public void SERVERAddAttack(int attack)
    {
        playerAttack += attack;
    }
}
