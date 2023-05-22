using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class TurnSystem : NetworkBehaviour
{
    public readonly SyncList<GameObject> players = new SyncList<GameObject>();
    public GameObject[] playerArray;

    private void Start()
    {
        playerArray = GameObject.FindGameObjectsWithTag("Player");
        CmdRegisterPlayer();
    }

    [Command]
    public void CmdRegisterPlayer()
    {
        foreach (GameObject player in playerArray)
        {
            players.Add(player);
        }
    }
}
