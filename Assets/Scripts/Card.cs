using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "newCard", menuName = "Card Data")]
public class Card : ScriptableObject
{
     public string cardname;
     public int cost;
     public int attack;
     public int heal;
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
