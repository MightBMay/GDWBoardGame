using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;
    public List<Card> allActionCards = new List<Card>();
    public List<ScenarioCard> allScenarioCards = new List<ScenarioCard>();
    public ScenarioCard currentScenarioCard;
    [SerializeField] VisibleCard scenearioCard;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        PopulateDecks();
        AssignNewScenarioCard();
    }
    public void AssignNewScenarioCard()
    {
        for (int x = 0; x < scenearioCard.cardStrengthHolders.Length; x++)
        {
            int childCount = scenearioCard.cardStrengthHolders[x].transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject child = scenearioCard.cardStrengthHolders[x].transform.GetChild(i).gameObject;
                Destroy(child);
            }
        }
        currentScenarioCard = GetRandomScenarioCard((int)Random.Range(0, allScenarioCards.Count - 1));
        currentScenarioCard.OnCardChosen();
    }
    public void PopulateDecks()
    {
        for (int x = 0; x < 4; x++)
        {
            allActionCards.Add(new DealWithTheDevil());
            allActionCards.Add(new BurstOfInspiration());
            allActionCards.Add(new ADedicatedSearch());
            allActionCards.Add(new ChairOnTheDoor());
            allActionCards.Add(new LastResort());
            allActionCards.Add(new OneLastBullet());
            allActionCards.Add(new UncontrollableShotgun());
            allActionCards.Add(new AFriendInNeed());
            allActionCards.Add(new GreatTiming());
            allActionCards.Add(new ALazyLookAround());
            allActionCards.Add(new RecklessAssault());
            allActionCards.Add(new PreciseAim());
            allScenarioCards.Add(new AMysteriousObject());
            allScenarioCards.Add(new PlagueRiddenHost());
            allScenarioCards.Add(new RampagingBeast());
            allScenarioCards.Add(new AGhostInTheHold());
            allScenarioCards.Add(new IllnessSpreads());

        }
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
