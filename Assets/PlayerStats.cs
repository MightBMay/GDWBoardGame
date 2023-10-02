using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerStats 
{
    public int vitality, search, sanity, stat4, stat5, stat6;
    public PlayerStats(int vitality, int search, int sanity, int stat4, int stat5, int stat6)
    {
        this.vitality = vitality;
        this.search = search;
        this.sanity = sanity;
        this.stat4 = stat4;
        this.stat5 = stat5;
        this.stat6 = stat6;
    }
}
