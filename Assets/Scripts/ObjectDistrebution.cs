using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObjectDistrebution : NetworkBehaviour
{
    private GameObject opponent;
    private GameObject player;

    private void Awake()
    {
        opponent = GameObject.FindWithTag("Opponent");
        player = GameObject.FindWithTag("Player");
    }
    private void Start()
    {
        if (isLocalPlayer)
        {
            if (player != null)
            {
                player.transform.SetParent(transform);
            }
        }
        else
        {
            if (opponent != null)
            {
                opponent.transform.SetParent(transform);
            }
        }
    }
}
