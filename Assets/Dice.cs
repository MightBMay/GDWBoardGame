using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dice : MonoBehaviour
{
    public static Dice instance;
    public List<DiceRoll> diceRollHistory = new List<DiceRoll>();
    public GameObject d6Prefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }
    }
    public DiceRoll RollNSidedDice(int diceCount = 1)
    {
        
        DiceRoll roll = new DiceRoll(diceCount);
        diceRollHistory.Add(roll);
        return roll;

    }
    public DiceRoll RollNSidedDice(int min = 1, int max = 6, int diceCount = 1)
    {

        DiceRoll roll = new DiceRoll(min,max,1);
        diceRollHistory.Add(roll);
        return roll ;
        
    }
    
    [System.Serializable]
    public class DiceRoll
    {
        public int[] rolls; // array of all individual rolls.
        public int sum;     // sum of all elements in the rolls array.
        public enum DiceRollSortMode // used for sorting die if needed.
        {
            MaxToMin,
            MinToMax,
            None,
        }

        public DiceRoll(int diceCount) // assumes a d6, and just takes a quantity of them.
        {
            this.rolls = new int[diceCount];
            for (int x = 0; x < diceCount; x++)
            {
                int diceValue = Mathf.FloorToInt(UnityEngine.Random.Range(1, 6));
                rolls[x] = diceValue;
                sum += diceValue;
            }
        }
        public DiceRoll(int min = 1, int max = 6, int diceCount = 1) // allows for different values of dice,
        {                                                            // for example, a d20, or a dice numbered from 3 to 9.
            this.rolls = new int[diceCount];
            for (int x = 0; x < diceCount; x++)
            {
                int diceValue = Mathf.FloorToInt(UnityEngine.Random.Range(min, max));
                rolls[x] = diceValue;
                sum += diceValue ;
            }
        }
        public DiceRoll(int[] setValues)// for the event you need a dice roll, but must manually set the values.
        {
            rolls = setValues;
            foreach(int i in this.rolls)
            {
                sum += i;
            }
        }

        /// <summary>
        /// Applies a sort to the all values in the "Rolls" array and returns it as a string.
        /// </summary>
        /// <param name="sortMode"> Sorting option for the order of the values.</param>
        /// <returns></returns>
        public string DiceRollToString(DiceRollSortMode sortMode)
        {
            string str = "";// make blank string
            if (sortMode == DiceRollSortMode.None)                     
            {

                foreach (int i in this.rolls)
                {
                    str += i.ToString() + ", "; // adds the string for each value in the array
                }
            }
            else if(sortMode== DiceRollSortMode.MaxToMin)
            {
                Array.Sort(this.rolls); //sorts from lowest to highest,
                Array.Reverse(this.rolls); // then reverses it, so it's highest to lowest.
                foreach (int i in this.rolls)
                {
                    str += i.ToString() + ", "; // adds the string for each value in the array
                }
            }
            else if (sortMode == DiceRollSortMode.MinToMax)
            {
                Array.Sort(this.rolls); // sorts from lowest to highest.
                
                foreach (int i in this.rolls)
                {
                    str += i.ToString() + ", "; // adds the string for each value in the array
                }
            }
            return str;
        }
    }
}
