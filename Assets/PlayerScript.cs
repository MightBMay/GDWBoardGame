using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    public int playerNumber;
    public PlayerStats stats;
    public TextMeshProUGUI Vitality, Sanity;
    public TextMeshProUGUI temperaryPlayertext;
    public Card selectedCard;

    private void Awake()
    {
       // playerVisDeck = GetComponentInChildren<PlayerVisibleDeck>();
    }
    /// <summary>
    /// Takes a dice roll (MAKE SURE IT HAS 6 OR MORE VALUES) and assigns it to the players stats.
    /// </summary>
    /// <param name="statRoll"> Dice roll to apply to stats, in order of 0-5th element.</param>
    public void RollAndAssignStats(Dice.DiceRoll statRoll)
    {
        if (statRoll == null || statRoll.rolls.Length < 2)
        {
            Debug.LogError("Stat Assignement Error: DiceRoll Either Null Or Contains Less Than 6 Values");
        }
        stats.vitality = statRoll.rolls[0] +3;
        stats.sanity   = statRoll.rolls[1] +3;


        AssignTextFromStats(new PlayerStats(stats.vitality, stats.sanity));
    }
    /// <summary>
    /// Takes the players stats and updates the text display in the game view.
    /// </summary>
    /// <param name="stats">Players stats to update the TMPro object to</param>
    public void AssignTextFromStats(PlayerStats stats)
    {
        Vitality.text = "Vitality: " + stats.vitality.ToString();
        Sanity.text = "Sanity: " + stats.sanity.ToString();
    }


    public void AssignTemperaryText(string newText)
    {
        temperaryPlayertext.text = newText;
    }
}


