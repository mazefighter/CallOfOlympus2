using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newCard", menuName = "Card Data")]
public class Card : ScriptableObject
{
     public string Cardname;
     public int cost;
     public int attack;
     public int CoinGain;
     public God god;
     
     
     public enum God
     {
          none,
          Zeus,
          Poseidon,
          Hades
     }

     

}
