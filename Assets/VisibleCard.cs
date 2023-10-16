using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class VisibleCard : MonoBehaviour
{
    public TextMeshProUGUI title, info, strength, other, Or;
    public Transform[] cardStrengthHolders;
    public LayoutElement layoutElement;
    // Start is called before the first frame update
    public void UpdateCardText(Card card, bool isScenario)
    {
        title.text = card.cardName;
        info.text = card.cardInfo;
        if (!isScenario)
        {
           GetStrengths(card);
        }
        else
        {
            GetStrengthsScenario(card as ScenarioCard);
        }
        if(other != null)
        {
            other.text = card.cardName;
        }

    }

    public void GetStrengths(Card card)
    {
 
        GameManager gm = GameManager.instance;
        GameObject icon;
        foreach (DeckManager.CardType ct in card.cardTypes)
        {
            switch (ct)
            {
                case DeckManager.CardType.Search:
                    icon = Instantiate(gm.searchIconPrefab);
                    icon.transform.SetParent(cardStrengthHolders[0]);
                    break;
                case DeckManager.CardType.Attack:
                    icon = Instantiate(gm.attackIconPrefab);
                    icon.transform.SetParent(cardStrengthHolders[0]);
                    break;
                case DeckManager.CardType.Defend:
                    icon = Instantiate(gm.defendIconPrefab);
                    icon.transform.SetParent(cardStrengthHolders[0]);
                    break;
                default:
                    break;
            }
        }

    }
    public void GetStrengthsScenario(ScenarioCard card)
    {


        GameManager gm = GameManager.instance;
        int count = 0;
        foreach (CardStrengths cs in card.cardStrengths)
        {
            Transform t = cardStrengthHolders[count];
            for (int i = 0; i < cs.search; i++)
            {
                GameObject icon = Instantiate(gm.searchIconPrefab);
                icon.transform.SetParent(t);
            }
            for (int i = 0; i < cs.attack; i++)
            {
                GameObject icon = Instantiate(gm.attackIconPrefab);
                icon.transform.SetParent(t);
            }
            for (int i = 0; i < cs.defend; i++)
            {
                GameObject icon = Instantiate(gm.defendIconPrefab);
                icon.transform.SetParent(t);
            }
            count++;
        }
        if(count > 1)
        {
            Or.gameObject.SetActive(true);
        }
        else { Or.gameObject.SetActive(false); }
    }

    public void EnlargeSelectedCard()
    {
        transform.localScale = new Vector3(3, 3, 1);
    }
    public void ShrinkSelectedCard()
    {
        transform.localScale = new Vector3(2, 2, 1);
    }
}
