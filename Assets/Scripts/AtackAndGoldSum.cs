using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AtackAndGoldSum : NetworkBehaviour
{
    [SyncVar]
    public int PlayerAttack;
    [SyncVar]
    public int PlayerGold;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            PlayerGold++;
        }
    }
}
