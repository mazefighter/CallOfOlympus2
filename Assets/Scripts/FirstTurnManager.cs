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
        //shuffle all players decks at start
        players[1 - firstTurn].GetComponent<CardLocationManager>().SERVERDrawFromDeckToHand(5);
        players[firstTurn].GetComponent<CardLocationManager>().SERVERDrawFromDeckToHand(3);
        players[firstTurn].GetComponent<TurnSystem>().isTurn = true;
    }
}
