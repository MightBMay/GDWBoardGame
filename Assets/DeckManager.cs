using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;
    public List<Card> allActionCards = new List<Card>();
    public List<ScenarioCard> allScenarioCards = new List<ScenarioCard>();
    public ScenarioCard currentScenarioCard;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        currentScenarioCard = GetRandomScenarioCard(1); //```````````````````````````````````````````````````````````
    }
    public void Update()
    {
    }
    public Card GetRandomActionCard(int index)
    {
        // done this way with a copy being assigned, then copying the values over to avoid issues with modifying the allActionCards list.
        Card copyOfCard = allActionCards[index];
        allActionCards.Remove(allActionCards[index]);
        return copyOfCard;
    }
    public ScenarioCard GetRandomScenarioCard(int index)
    {
        // done this way with a copy being assigned, then copying the values over to avoid issues with modifying the allActionCards list.
        ScenarioCard copyOfCard = allScenarioCards[index];
        allScenarioCards.Remove(allScenarioCards[index]);     
        return copyOfCard;
    }
   
    public enum CardType
    {
        Search,
        Attack,
        Defend,
    }



}

/*public bool DeckContain(string name)
{
    bool cardFound = false;
    foreach (Card c in allActionCards)
    {
        if (c.cardName.Equals(name, System.StringComparison.InvariantCultureIgnoreCase))
        {
            cardFound = true;
        }
    }
    return cardFound;
}
*/
