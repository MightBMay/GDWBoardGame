using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Card
{
    public string cardName { get; set; }
    public string cardInfo { get; set; }
    public DeckManager.CardType[] cardTypes { get; set; }
    public bool isTraitor { get; set; }


    public string GetCardInfo()
    {
        return cardName + ": " + cardInfo;
    }

    public virtual IEnumerator OnPlay()
    {
        if (CheckValidPlay())
        {
            DeckManager.instance.allActionCards.Add(this);
        }
        yield return null;
    }

    public virtual bool CheckValidPlay()
    {
        return true;
    }
    public virtual void OnRoundEnd(bool requirementsMet)
    {
    }
}


[System.Serializable]
public class BurstOfInspiration : Card //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
{

    public BurstOfInspiration()
    {
        cardName = "Burst Of Inspiration";
        cardInfo = "This card has no effect.";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Search };
    }
}

[System.Serializable]
public class ADedicatedSearch : Card //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
{

    public ADedicatedSearch()
    {
        cardName = "A dedicated search";
        cardInfo = "Lose 1 Sanity";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Search, DeckManager.CardType.Search };
    }
    public override IEnumerator OnPlay()
    {
        if (CheckValidPlay())
        {
            GameManager.instance.players[GameManager.instance.currentPlayerTurn].stats.sanity--;
            DeckManager.instance.allActionCards.Add(this);

        }
        yield return null;
    }
}


[System.Serializable]
public class ChairOnTheDoor : Card //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
{

    public ChairOnTheDoor()
    {
        cardName = "ChairOnTheDoor";
        cardInfo = "Draw and replace a random card in your deck.";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Defend };
    }
    public override IEnumerator OnPlay()
    {
        if (CheckValidPlay())
        {
            GameManager gm = GameManager.instance;
            DeckManager.instance.allActionCards.Add(this);
            gm.PlayerDiscardRandom(gm.players[gm.currentPlayerTurn],1,true);
        }
        yield return null;
    }
}

[System.Serializable]
public class LastResort : Card //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
{

    public LastResort()
    {
        cardName = "Last Resort";
        cardInfo = " -1 Vitality";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Defend };
    }
    public override IEnumerator OnPlay()
    {
        if (CheckValidPlay())
        {
            GameManager.instance.players[GameManager.instance.currentPlayerTurn].stats.vitality--;
            DeckManager.instance.allActionCards.Add(this);
        }
        yield return null;
    }
}


[System.Serializable]
public class OneLastBullet : Card //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
{

    public OneLastBullet()
    {
        cardName = "OneLastBullet";
        cardInfo = "This card has no effect. ";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Attack };
    }
    public override IEnumerator OnPlay()
    {
        if (CheckValidPlay())
        {
            DeckManager.instance.allActionCards.Add(this);
        }
        yield return null;
    }
}

[System.Serializable]
public class UncontrollableShotgun : Card //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
{

    public UncontrollableShotgun()
    {
        cardName = "Uncontrollable Shotgun";
        cardInfo = "-1 vitality to the next player.";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Attack };
    }
    public override IEnumerator OnPlay()
    {
        if (CheckValidPlay())
        {
            GameManager gm = GameManager.instance;
            int targetIndex = gm.currentPlayerTurn + 1;
            if (targetIndex >= gm.players.Count)
            {
                targetIndex = 0;
            }
            gm.players[targetIndex].stats.vitality--;

            DeckManager.instance.allActionCards.Add(this);

        }
        yield return null;
    }
}

[System.Serializable]
public class AFriendInNeed : Card //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~000000000000000000000000000000000000000000000000000000
{

    public AFriendInNeed()
    {
        cardName = "A Friend In Need";
        cardInfo = "Draw 1 card and give it to the next player.";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Defend };
    }
    public override IEnumerator OnPlay()
    {
        if (CheckValidPlay())
        {
            GameManager gm = GameManager.instance;
            int targetIndex = gm.currentPlayerTurn + 1;
            if (targetIndex >= gm.players.Count)
            {
                targetIndex = 0;
            }
            gm.PlayerDrawCard(gm.players[targetIndex]);

            DeckManager.instance.allActionCards.Add(this);
        }
        yield return null;
    }
}

[System.Serializable]
public class GreatTiming : Card //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~00000000000000000000000000000000000000000000000000000000000000000
{

    public GreatTiming()
    {
        cardName = "Great Timing";
        cardInfo = "Must have passed last turn";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Defend, DeckManager.CardType.Defend };
    }

    public override bool CheckValidPlay()
    {
        return GameManager.instance.players[GameManager.instance.currentPlayerTurn].selectedCard == null;
    }
    public override IEnumerator OnPlay()
    {
        if (CheckValidPlay())
        {
            DeckManager.instance.allActionCards.Add(this);
        }
        yield return null;
    }
}



[System.Serializable]
public class ALazyLookAround : Card //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
{

    public ALazyLookAround()
    {
        cardName = "A Lazy Look Around";
        cardInfo = "Can only play if one Search is required.";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Search };
    }
    public override bool CheckValidPlay()
    {
        foreach(CardStrengths c in DeckManager.instance.currentScenarioCard.cardStrengths)
        {
            if (c.search == 1)
            {
                return true;
            }
        }
        return false;
    }
    public override IEnumerator OnPlay()
    {
        if (CheckValidPlay())
        {
            DeckManager.instance.allActionCards.Add(this);
        }
        yield return null;
    }
}

[System.Serializable]
public class RecklessAssault : Card //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
{

    public RecklessAssault()
    {
        cardName = "Reckless Assault";
        cardInfo = "This card has no effect.";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Attack };
    }
    public override IEnumerator OnPlay()
    {
        if (CheckValidPlay())
        {
            DeckManager.instance.allActionCards.Add(this);
        }
        yield return null;
    }
}

[System.Serializable]
public class PreciseAim : Card //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
{

    public PreciseAim()
    {
        cardName = "Precise Aim";
        cardInfo = "This card is ignored if there is no Search required.";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Attack };
    }
    public override bool CheckValidPlay()
    {

        foreach (CardStrengths c in DeckManager.instance.currentScenarioCard.cardStrengths)
        {
            if (c.search > 0)
            {
                return true;
            }
        }
        return false;
    }
    public override IEnumerator OnPlay()
    {
        if (CheckValidPlay())
        {
            DeckManager.instance.allActionCards.Add(this);
        }
        yield return null;
    }
}

[System.Serializable]
public class DealWithTheDevil : Card
{
    public DealWithTheDevil()
    {
        cardName = "Deal With The Devil";
        cardInfo = "+1 Vitality to you and a chosen player.";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Attack };
    }

    public override IEnumerator OnPlay()
    {
        GameManager gm = GameManager.instance;
        int numPressed;
        gm.players[gm.currentPlayerTurn].stats.vitality += 1;
        while (true)
        {
            Debug.Log("Select The Player Number you wish to give Vitality to (1-4 self excluded);");
            yield return numPressed = gm.CheckForNumberKeyInput();
            if (numPressed > 0 && numPressed < gm.players.Count)
            {
                if (numPressed - 1 != gm.currentPlayerTurn)
                {
                    Debug.Log("player number " + (numPressed - 1));
                    gm.players[numPressed - 1].stats.vitality += 1;
                    break;
                }
                else
                {
                    if (gm.players.Count <= 1) { break; }
                    else
                    {
                        Debug.Log("You cannot select yourself. Pick a different Number.");
                    }
                }
            }
            else { Debug.Log("Number not between 1-Player Count. Selecte another number."); }
        }


    }
}


[System.Serializable]
public class  a: Card
{
    public a()
    {
        cardName = "";
        cardInfo = "This card has no effect.";
        cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Defend, DeckManager.CardType.Attack };
    }

}
[System.Serializable]
public class b : Card
{
    public b()
{
    cardName = "";
    cardInfo = "This card has no effect.";
    cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Search, DeckManager.CardType.Defend };
}

}
[System.Serializable]
public class c : Card
{
    public c()
{
    cardName = "";
    cardInfo = "No effect.";
    cardTypes = new DeckManager.CardType[] { DeckManager.CardType.Defend, DeckManager.CardType.Defend };
}

}


public struct CardStrengths
{
    public int search;
    public int attack;
    public int defend;

    public CardStrengths(int search, int attack, int defend)
    {
        this.search = search;
        this.attack = attack;
        this.defend = defend;
    }

    public bool CheckRequirements(int Search, int Attack, int Defend)
    {

        return attack == Attack && defend == Defend && search == Search;
    }
    public void GetRequirements(out int Search, out int Attack, out int Defend)
    {
        Search = this.search;
        Attack = this.attack;
        Defend = this.defend;
    }
}

[System.Serializable]
public class ScenarioCard : Card
{

    public CardStrengths[] cardStrengths;

    public virtual void OnCardChosen()
    {

    }
    public override void OnRoundEnd(bool requirementsMet)
    {
        DeckManager.instance.allScenarioCards.Add(this);
    }
}


public class AMysteriousObject : ScenarioCard
{
    public AMysteriousObject()
    {
        cardName = "A Mysterious Object";
        cardInfo = "On Success:  Gain 1 Sanity.\n" +
                   "On Failure:  Lose 1 Sanity.";

        cardStrengths = new CardStrengths[] { new CardStrengths(3, 0, 0) };
    }

    public override void OnCardChosen()
    {
        foreach (PlayerScript ps in GameManager.instance.players)
        {
            ps.stats.sanity--;
        }
    }
    public override void OnRoundEnd(bool requirementsMet)
    {
        if (requirementsMet)
        {
            foreach(PlayerScript ps in GameManager.instance.players)
            {
                ps.stats.sanity += 1;
            }
        }
        else
        {
            foreach (PlayerScript ps in GameManager.instance.players)
            {
                ps.stats.sanity -= 1;
            }
        }
    }
}


public class PlagueRiddenHost : ScenarioCard
{
    public PlagueRiddenHost()
    {
        cardName = "Plague Ridden Host";
        cardInfo = "On Success:  Nothing happens." +
                   "\nOn Failure: -1 Vitality and -1 Sanity to 2 players.";
        cardStrengths = new CardStrengths[] { new CardStrengths(0, 3, 0), new CardStrengths(3, 0, 2)  };
    }

    public override void OnCardChosen()
    {

    }

    public override void OnRoundEnd(bool requirementsMet)
    {
        if (requirementsMet)
        {
            return;
        }
        else
        {
            GameManager gm = GameManager.instance;
            for (int i=0; i < 2; i++)
            {
                int random = Random.Range(0, gm.players.Count);
                gm.players[random].stats.sanity   -= 1;
                gm.players[random].stats.vitality -= 1;
            }
        }
    }
}


public class RampagingBeast : ScenarioCard
{
    public RampagingBeast()
    {
        cardName = "Rampaging Beast";
        cardInfo = "On Success: Nothing Happens." +
                   "\nOn Failure: -2 Vitality.";
        cardStrengths = new CardStrengths[] { new CardStrengths(0, 0, 3), new CardStrengths(1, 2, 0) };
    }

    public override void OnCardChosen()
    {

    }

    public override void OnRoundEnd(bool requirementsMet)
    {
        if (requirementsMet) { return; }
        else
        {
            foreach (PlayerScript ps in GameManager.instance.players)
            {
                ps.stats.vitality -= 2;
            }
        }
    }
}



public class AGhostInTheHold : ScenarioCard
{
    public AGhostInTheHold()
    {
        cardName = "A Ghost In The Hold";
        cardInfo = "On Success:  +1 Vitality and +1 Sanity." +
            "\nOn Failure: -2 Sanity.";
        cardStrengths = new CardStrengths[] { new CardStrengths(1, 1, 2) };
    }

    public override void OnCardChosen()
    {

    }

    public override void OnRoundEnd(bool requirementsMet)
    {
        if (requirementsMet)
        {
            foreach (PlayerScript ps in GameManager.instance.players)
            {
                ps.stats.sanity   += 1;
                ps.stats.vitality += 1;
            }
        }
        else
        {
            foreach (PlayerScript ps in GameManager.instance.players)
            {
                ps.stats.sanity -= 2;
            }
        }
    }
}



public class IllnessSpreads : ScenarioCard
{
    public IllnessSpreads()
    {
        cardName = "Illness Spreads";
        cardInfo = "On Success: +1 Vitality." +
            "\nOn Failure: -2 vitality.";
        cardStrengths = new CardStrengths[] { new CardStrengths(3, 0, 1) };
    }


    public override void OnCardChosen()
    {
    }
    public override void OnRoundEnd(bool requirementsMet)
    {
        if (requirementsMet)
        {
            foreach (PlayerScript ps in GameManager.instance.players)
            {
                ps.stats.vitality += 1;
            }
        }
        else
        {
            foreach (PlayerScript ps in GameManager.instance.players)
            {
                ps.stats.vitality -= 2;
            }
        }
    }
}


