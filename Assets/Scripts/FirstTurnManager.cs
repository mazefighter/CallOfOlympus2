using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class FirstTurnManager : NetworkBehaviour
{
    public List<GameObject> players;
    public GameObject[] playerArray = new GameObject[2];
    public void Start()
    {
        StartCoroutine(WaitThenChooseFirstPlayer());
    }

    IEnumerator WaitThenChooseFirstPlayer()
    {
        yield return new WaitForSeconds(1f);
        int firstTurn = Random.Range(0, 2);
        playerArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in playerArray)
        {
            players.Add(player);
        }

        CardLocationManager ClmFirstPlayer = players[firstTurn].GetComponent<CardLocationManager>();
        CardLocationManager ClmSecondPlayer = players[1-firstTurn].GetComponent<CardLocationManager>();
        ClmSecondPlayer.SERVERShuffleDeck();
        ClmFirstPlayer.SERVERShuffleDeck();
        ClmSecondPlayer.SERVERDrawFromDeckToHand(5);
        ClmFirstPlayer.SERVERDrawFromDeckToHand(3);
        players[firstTurn].GetComponent<TurnSystem>().isTurn = true;
    }
}
