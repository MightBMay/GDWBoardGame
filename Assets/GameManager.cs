using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState = GameState.StatSelect;
    public int currentPlayerTurn = 0;
    public List<PlayerScript> players = new List<PlayerScript>();
    int roundnumber = 0;
    public Transform cardHolder;
    public GameObject scenarioCard,ScenarioCardHolder;
    public GameObject blankCardPrefab;
    public GameObject searchIconPrefab, attackIconPrefab, defendIconPrefab;


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
        yield return new WaitUntil(() => currentGameState != GameState.StatSelect);// makes it wait for the playerselectstats coroutine to recursively loop through all players.

        foreach (PlayerScript ps in players) // remove temp text that shows your dice rolls, and draws 4 cards.
        {
            PlayerDrawCard(ps, 4);
            ps.AssignTemperaryText("");
        }

        scenarioCard.SetActive(true);


        while (true) // main gameplay loop of selecting cards to compare to scenario card.
        {
            Debug.Log("round #: " + roundnumber);
            DeckManager.instance.AssignNewScenarioCard();// select new scenario.
            ScenarioCardHolder.GetComponent<VisibleCard>().UpdateCardText(DeckManager.instance.currentScenarioCard, true);

            yield return StartCoroutine(SelectCard());
            yield return new WaitUntil(() => currentGameState != GameState.CardSelect); // makes it wait for the selectcard coroutine to recursively loop through all players.
            roundnumber++;
            foreach (PlayerScript ps in players)
            {
                PlayerDrawCard(ps);
                UpdatePlayerStats(ps);
            }
            if (CheckPlayerDeaths())
            {
                currentGameState = GameState.EndGame;
                break;
            }
        }

        Debug.Log("GameOver");

    }
    public void UpdatePlayerStats(PlayerScript player)
    {
        player.AssignTextFromStats(player.stats);
    }
    public void PlayerDrawCard(PlayerScript player, int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
        {
            // pick number from 0-total deck length, round to int with cast, return that index as a card.
            player.stats.playerDeck.Add(DeckManager.instance.GetRandomActionCard((int)Random.Range(0, DeckManager.instance.allActionCards.Count)));
        }
    }

    public void PlayerDiscardRandom(PlayerScript player, int quantity = 1, bool redraw = false)
    {
        for (int i = 0; i < quantity; i++)
        {
            int index = (int)Random.Range(0, player.stats.playerDeck.Count);
            player.stats.playerDeck.RemoveAt(index);

            if (redraw)
            {
                PlayerDrawCard(player);
            }
        }
    }
    public bool CheckPlayerDeaths()
    {

        if (players.Count <= 0)
        {
            Debug.Log("All players have died.");
            return true;
        }
        else
        {
            List<PlayerScript> playersToRemove = new List<PlayerScript>();

            foreach (PlayerScript ps in players)
            {
                if (ps.stats.vitality <= 0 || ps.stats.sanity <= 0)
                {
                    int ind = players.IndexOf(ps);
                    foreach (Card c in ps.stats.playerDeck)
                    {
                        DeckManager.instance.allActionCards.Add(c);
                    }

                    playersToRemove.Add(ps);
                    Debug.Log("PLAYER #" + ind + " HAS DIED");
                }
            }

            // Remove the players marked for removal from the original list
            foreach (PlayerScript psToRemove in playersToRemove)
            {
                players.Remove(psToRemove);
            }
            return false;
        }
    }

    IEnumerator SelectCard()
    {
        Card selectedCard;
        List<GameObject> shownCardList = new List<GameObject>();


        // stops after looping through the 4 players.
        if (currentPlayerTurn < players.Count)
        {
            currentGameState = GameState.CardSelect; // a




            int numPressed;
            ShowCards(players[currentPlayerTurn].stats);
            Debug.Log("press number 1 - 4 to select card, or 0 to Pass.");
            while (true)
            {
                yield return numPressed = CheckForNumberKeyInput();
                if (numPressed == 0)
                {
                    selectedCard = players[currentPlayerTurn].selectedCard = null;
                    break;
                }
                else if (numPressed <= players[currentPlayerTurn].stats.playerDeck.Count && numPressed > 0)
                {
                    selectedCard = players[currentPlayerTurn].selectedCard = players[currentPlayerTurn].stats.playerDeck[numPressed - 1]; // assigns the selected card to the playerscript and a local variable to run later.
                    break;
                }

            }

            if (selectedCard != null)
            {
                Debug.Log(selectedCard.cardName + " selected");
                yield return StartCoroutine( selectedCard.OnPlay() );
                players[currentPlayerTurn].stats.playerDeck.Remove(selectedCard);
                
            }
            HideCards();
            currentPlayerTurn++;
            foreach (PlayerScript ps in players) { UpdatePlayerStats(ps); }
            StartCoroutine(SelectCard());

        }
        else
        {
            currentPlayerTurn = 0; // reset turn counter.
            currentGameState = GameState.Idle; // move to next GameState
            bool requirementsMet = ScenarioCheckToSelected(); // checks if the players selected cards meed the search/attck/defend requirements to succeed.
            foreach (PlayerScript ps in players)
            {
                if (ps.selectedCard != null)
                {
                    ps.selectedCard.OnRoundEnd(requirementsMet); // runs turn end effects on action cards.

                }
            }

            DeckManager.instance.currentScenarioCard.OnRoundEnd(requirementsMet); // runs turn end effects on scenario card.

            yield break;
        }

        List<VisibleCard> ShowCards(PlayerStats stats)
        {
            List<VisibleCard> visCards = new List<VisibleCard>();
            foreach (Card c in stats.playerDeck)
            {
                GameObject newCard = Instantiate(blankCardPrefab);
                newCard.transform.SetParent(cardHolder);
                visCards.Add(newCard.GetComponent<VisibleCard>());
                newCard.GetComponent<VisibleCard>().UpdateCardText(c, false);
                shownCardList.Add(newCard);
            }
            return visCards;

        }

        void HideCards()
        {
            List<GameObject> shownCardListCopy = shownCardList;
            foreach (GameObject g in shownCardListCopy)
            {
                Destroy(g);
            }
            shownCardList.Clear();
        }
    }


    bool ScenarioCheckToSelected()
    {
        int attack = 0, defend = 0, search = 0;
        ScenarioCard sceneCard = DeckManager.instance.currentScenarioCard;
        // checks if the cards selected by players collectevly surpass the ScenarioCard.cardStrength while taking card type into account.
        foreach (PlayerScript player in GameManager.instance.players)
        {
            if (player.selectedCard != null)
            {
                foreach (DeckManager.CardType type in player.selectedCard.cardTypes)
                {
                    switch (type)
                    {
                        case DeckManager.CardType.Attack:
                            attack++;
                            break;

                        case DeckManager.CardType.Defend:
                            defend++;
                            break;

                        case DeckManager.CardType.Search:
                            search++;
                            break;


                        default:
                            Debug.Log("card checked against scenario has no type??");
                            break;
                    }
                }
            }

        }
        foreach (CardStrengths c in sceneCard.cardStrengths)
        {
            if (c.CheckRequirements(search, attack, defend))
            {
                Debug.Log("Scenerio Successful");
                return true;
            }
        }

        Debug.Log("Scenerio Unsuccessful");
        return false;
    }

    public IEnumerator PlayerSelectStats()
    {
        //uses recursion, exit case is when you have looped through all players.
        if (currentPlayerTurn < players.Count)
        {
            currentGameState = GameState.StatSelect;
            //temp message will be replaced with proper text later
            Debug.Log("Player #" + currentPlayerTurn + " Press any key to roll dice.");
            // uses a lambda function to wait for any input, 
            yield return new WaitUntil(() => Input.anyKeyDown);
            yield return null;

            //prerolls all 6 stats
            var rollx2 = Dice.instance.RollNSidedDice(2);
            players[currentPlayerTurn].AssignTemperaryText(rollx2.DiceRollToString(Dice.DiceRoll.DiceRollSortMode.MinToMax));
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
    public int CheckForNumberKeyInput()
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
