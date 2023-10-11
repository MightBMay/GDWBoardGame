using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState = GameState.StatSelect;
    public int currentPlayerTurn = 0;
    public PlayerScript[] players;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else { instance = this; }
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        StartCoroutine(GameLoop());
    }

    public IEnumerator GameLoop()
    {
        yield return StartCoroutine(PlayerSelectStats());
        yield return new WaitUntil(() => currentGameState != GameState.StatSelect);
        yield return StartCoroutine(SelectCard());
        yield return new WaitUntil(() => currentGameState != GameState.CardSelect);
        yield return StartCoroutine(CheckScenarioOnSelected());
        yield return new WaitUntil(() => currentGameState != GameState.CardCompare);

    }
    IEnumerator CheckScenarioOnSelected()
    {
        
        if (ScenarioCheckToSelected())
        {
            //whatever happens when you have the correct amount of cards
            Debug.Log("yipee");
        }
        else {
            //whatever happens when you do not have the correct amount of cards
            Debug.Log("arr naur");
        }
        currentGameState = GameState.Idle;
        yield return null;




        bool ScenarioCheckToSelected()
        {
            int attack = 0, defence = 0, search = 0;
            ScenarioCard sceneCard = DeckManager.instance.currentScenarioCard;
            // checks if the cards selected by players collectevly surpass the ScenarioCard.cardStrength while taking card type into account.
            foreach (PlayerScript player in GameManager.instance.players)
            {

                switch (player.selectedCard.cardType)
                {
                    case DeckManager.CardType.Attack:
                        attack++;
                        break;

                    case DeckManager.CardType.Defend:
                        defence++;
                        break;

                    case DeckManager.CardType.Search:
                        search++;
                        break;


                    default:
                        Debug.Log("card checked against scenario has no type??");
                        break;
                }

            }

            Debug.Log("attack/Required: " + attack + " / " + sceneCard.attackStrength);
            Debug.Log("defence/Required: " + defence + " / " + sceneCard.defenceStrength);
            Debug.Log("search/Required: " + search + " / " + sceneCard.searchStrength);
            return sceneCard.attackStrength <= attack && sceneCard.defenceStrength <= defence && sceneCard.searchStrength <= search;
        }
    }

    IEnumerator SelectCard()
    {
        Card selectedCard;
        // stops after looping through the 4 players.
        if (currentPlayerTurn < players.Length)
        {
            currentGameState = GameState.CardSelect; // a
            int numPressed;
            Debug.Log("press number 1 - 4 to select card");
            while (true)
            {
                yield return numPressed = CheckForNumberKeyInput();
                if (numPressed <= players[currentPlayerTurn].stats.playerDeck.Count && numPressed > 0)
                {
                   selectedCard = players[currentPlayerTurn].selectedCard = players[currentPlayerTurn].stats.playerDeck[numPressed-1];
                    break;
                }

            }
            Debug.Log(selectedCard.cardName + " selected");
            currentPlayerTurn++;
            StartCoroutine(SelectCard());

        }
        else { currentPlayerTurn = 0; currentGameState = GameState.CardCompare; yield break;  }
    }


    public IEnumerator PlayerSelectStats()
    {
        //uses recursion, exit case is when you have looped through all players.
        if (currentPlayerTurn < players.Length)
        {
            currentGameState = GameState.StatSelect;
            //temp message will be replaced with proper text later
            Debug.Log("Player #" + currentPlayerTurn + " Press any key to roll dice.");
            // uses a lambda function to wait for any input, 
            yield return new WaitUntil(() => Input.anyKeyDown);
            yield return null;

            //prerolls all 6 stats
            var rollx2 = Dice.instance.RollNSidedDice(2);
            players[currentPlayerTurn].
            AssignTemperaryText(rollx2.DiceRollToString(Dice.DiceRoll.DiceRollSortMode.MinToMax));
            //^^ assigns the 6 values of the prerolled dice to a TMProUGUI so you can see your rolls.
            yield return StartCoroutine(ReorderStats(rollx2));// begins the Stat Selection Coroutine.

            //moves to the next player and continues.
            currentPlayerTurn++;
            StartCoroutine(PlayerSelectStats());
        }
        else { currentPlayerTurn = 0; currentGameState = GameState.CardSelect; yield break; } // makes the turn number loop back to 0 ( first player).



    }

    public IEnumerator ReorderStats(Dice.DiceRoll roll)
    {
        int[] newRollOrder = new int[2]; // temperarily stores the new order of your dice.
        int[] alreadyChosen = new int[2];// used to check whether or not you have used a dice slot already
        for (int i = 0; i < 2; i++)      // ^^ so you cannot roll one 6 and use it 6 times.
        {
            // for each stat...
            int numPressed;
            while (true)
            {
                // get a number key input, check if it has been used, and if it is between 1-6 to assign to stats 1-6,
                yield return numPressed = CheckForNumberKeyInput();

                if (numPressed == 1 || numPressed == 2)
                {

                    if (CheckIfDiceSlotUsed(numPressed))
                    {
                        alreadyChosen[i] = numPressed;
                        break;
                    }
                    else { Debug.Log("Dice Slot Already Used"); }
                }

            }
            newRollOrder[i] = roll.rolls[numPressed - 1];
            // assign the roll from element number you clicked to the new order. 




        }
        Debug.Log("Reordered");
        //yield return new WaitUntil(() => Input.anyKeyDown);                               `````
        yield return null;
        players[currentPlayerTurn].RollAndAssignStats(new Dice.DiceRoll(newRollOrder));
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
        Idle,
        StatSelect,
        CardSelect,
        CardCompare,
        EndGame,

    }
}
