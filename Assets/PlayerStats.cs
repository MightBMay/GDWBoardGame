using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class PlayerStats 
{

    public int vitality, sanity;
    public List<Card> playerDeck = new List<Card>();
    public PlayerStats(int vitality,int sanity)
    {
        this.vitality = vitality;
        this.sanity = sanity;

    }
   

    
}
