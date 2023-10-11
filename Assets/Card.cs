using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName =" Card", menuName = "Assets/Custom/Create Card")]
[System.Serializable]
public class Card : ScriptableObject
{
    // Start is called before the first frame update
    public string cardName;
    public Sprite cardSprite;
    public string cardInfo;
    public DeckManager.CardType cardType;
    public bool IsTraitor;

    public string GetCardInfo()
    {
        return cardName + ": " + cardInfo;
    }

    public bool checkCardType(DeckManager.CardType CardType)
    {
        return cardType == CardType;
    }

}
[CreateAssetMenu(fileName = " ScenarioCard", menuName = "Assets/Custom/Create Scenario Card")]
[System.Serializable]
public class ScenarioCard: Card
{
    
    public int searchStrength;  // how many search the scenario requires.
    public int attackStrength;  // how many attack the scenario requires.
    public int defenceStrength; // how manydefence the scenario requires.
}

