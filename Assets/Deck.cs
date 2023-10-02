using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : MonoBehaviour
{
    public List<Card> deckCards = new List<Card>();
    // Start is called before the first frame update
    public Card GetCard(int i)
    {
       return deckCards[i];
    }
    public bool DeckContain(string name)
    {
        bool cardFound = false;
        foreach(Card c in deckCards)
        {
            if (c.cardName.Equals(name, System.StringComparison.InvariantCultureIgnoreCase))
            {
                cardFound = true;
            }
        }
        return cardFound;
    }
}
