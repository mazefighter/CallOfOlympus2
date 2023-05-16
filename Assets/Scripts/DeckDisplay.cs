using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Deck : MonoBehaviour
{
    // [SyncVar]
    // public int index;
    public List<Card> DeckList;
    public int DeckSize;
    private Card container;

    [SerializeField]private List<GameObject> CardDisplay;

    // Update is called once per frame
    void Update()
    {
        switch (DeckSize)
        {
            case >=5:
                CardDisplay[0].SetActive(true);
                CardDisplay[1].SetActive(true);
                CardDisplay[2].SetActive(true);
                CardDisplay[3].SetActive(true);
                CardDisplay[4].SetActive(true);
                break;
            case 4 :
                CardDisplay[0].SetActive(false);
                CardDisplay[1].SetActive(true);
                CardDisplay[2].SetActive(true);
                CardDisplay[3].SetActive(true);
                CardDisplay[4].SetActive(true);
                break;
            case 3 :
                CardDisplay[1].SetActive(false);
                CardDisplay[2].SetActive(true);
                CardDisplay[3].SetActive(true);
                CardDisplay[4].SetActive(true);
                break;
            case 2 :
                CardDisplay[2].SetActive(false);
                CardDisplay[3].SetActive(true);
                CardDisplay[4].SetActive(true);
                break;
            case 1 :
                CardDisplay[3].SetActive(false);
                CardDisplay[4].SetActive(true);
                break;
            case 0 :
                CardDisplay[4].SetActive(false);
                break;
        }
    }

   public void Shuffle()
    {
        for (int i = 0; i < DeckList.Count; i++)
        {
            container = DeckList[i];
            int randomIndex = Random.Range(i,DeckList.Count);
            DeckList[i] = DeckList[randomIndex];
            DeckList[randomIndex] = container;
        }
    }
}
