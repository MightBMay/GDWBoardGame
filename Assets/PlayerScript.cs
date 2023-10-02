using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    public int playerNumber;
    public PlayerStats stats;
    public TextMeshProUGUI Vitality, Search, Sanity, Stat4, Stat5, Stat6;
    public TextMeshProUGUI temperaryPlayertext;

    private void Start()
    {
        
    }
    /// <summary>
    /// Takes a dice roll (MAKE SURE IT HAS 6 OR MORE VALUES) and assigns it to the players stats.
    /// </summary>
    /// <param name="statRoll"> Dice roll to apply to stats, in order of 0-5th element.</param>
    public void RollAndAssignStats(Dice.DiceRoll statRoll)
    {
        if(statRoll == null || statRoll.rolls.Length < 6)
        {
            Debug.LogError("Stat Assignement Error: DiceRoll Either Null Or Contains Less Than 6 Values");
        }
        int vit = statRoll.rolls[0];
        int ser = statRoll.rolls[1];
        int san = statRoll.rolls[2];
        int s4 = statRoll.rolls[3];
        int s5 = statRoll.rolls[4];
        int s6 = statRoll.rolls[5];

        AssignTextFromStats(new PlayerStats(vit, ser, san, s4, s5, s6));
    }
    /// <summary>
    /// Takes the players stats and updates the text display in the game view.
    /// </summary>
    /// <param name="stats">Players stats to update the TMPro object to</param>
    public void AssignTextFromStats(PlayerStats stats)
    {
        Vitality.text = "Vitality: " + stats.vitality.ToString();
        Search.text = "Search: " +     stats.search.ToString();
        Sanity.text = "Sanity: " +     stats.sanity.ToString();
        Stat4.text = "Stat4: " +       stats.stat4.ToString();
        Stat5.text = "Stat5: " +       stats.stat5.ToString();
        Stat6.text = "Stat6: " +       stats.stat6.ToString();
    }

    public void AssignTemperaryText(string newText)
    {
        temperaryPlayertext.text = newText;
    }
}


