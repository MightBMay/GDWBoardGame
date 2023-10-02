using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.StatSelect;
    public int currentPlayerTurn = 0;
    public PlayerScript[] players;


    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(PlayerSelectStats());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator PlayerSelectStats()
    {
        //uses recursion, exit case is when you have looped through all players.
        if (currentPlayerTurn < players.Length)
        {
            //temp message will be replaced with proper text later
            Debug.Log("Player #" + currentPlayerTurn + " Press any key to roll dice.");
            // uses a lambda function to wait for any input, 
            yield return new WaitUntil(() => Input.anyKeyDown);
            yield return null;

            //prerolls all 6 stats
            var rollx6 = Dice.instance.RollNSidedDice(6);
            players[currentPlayerTurn].
            AssignTemperaryText(rollx6.DiceRollToString(Dice.DiceRoll.DiceRollSortMode.MinToMax));
            //^^ assigns the 6 values of the prerolled dice to a TMProUGUI so you can see your rolls.
            yield return StartCoroutine(ReorderStats(rollx6));// begins the Stat Selection Coroutine.

            //moves to the next player and continues.
            currentPlayerTurn++;
            StartCoroutine(PlayerSelectStats());
        }
        else { currentPlayerTurn = 0; yield break; } // makes the turn number loop back to 0 ( first player).



    }

    





    public IEnumerator ReorderStats(Dice.DiceRoll roll)
    {
        int[] newRollOrder = new int[6]; // temperarily stores the new order of your dice.
        int[] alreadyChosen = new int[6];// used to check whether or not you have used a dice slot already
        for (int i = 0; i < 6; i++)      // ^^ so you cannot roll one 6 and use it 6 times.
        {
            // for each stat...
            int numDown;
            while (true)
            {
                // get a number key input, check if it has been used, and if it is between 1-6 to assign to stats 1-6,
                numDown = CheckForNumberKeyInput();
                yield return null;
                if (numDown > 0 && numDown <= 6 ) {

                    if (CheckIfDiceSlotUsed(numDown))
                    {
                        alreadyChosen[i] = numDown;
                        break;
                    }
                    else { Debug.Log("Dice Slot Already Used"); }
                }
                else { Debug.Log("Number not between 1-6"); }
            }
                newRollOrder[i] = roll.rolls[numDown - 1];
            // assign the roll from element number you clicked to the new order. 
                
            


        }
        Debug.Log("Reordered");
        yield return new WaitUntil(() => Input.anyKeyDown);
        yield return null;
        players[currentPlayerTurn].RollAndAssignStats( new Dice.DiceRoll(newRollOrder) );
        // assign the new roll order to the player.


        bool CheckIfDiceSlotUsed(int numDown)
        {// self explanatory.
            foreach (int i in alreadyChosen)
            {
                if (i == numDown) { return false; }
            }
            return true;
        }
    }
    int CheckForNumberKeyInput()
    {

        
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                return i;
            }
        }

        // If no numeric key between 0 and 9 is pressed, return -1 (or any other value to indicate no key press)
        return -1;
    }
    public enum GameState
    {
        StatSelect,
        MainGameLoop,
        EndGame
    }
}
