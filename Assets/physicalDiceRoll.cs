using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class physicalDiceRoll : MonoBehaviour
{
    public int[] roll = new int[1]; // this is an array as a cheap hack to allow me to set a specific diceroll in Dice.diceroll.
    public Sprite[] sprites;
    Image img;
    // Start is called before the first frame update

    private void Start()
    {
        img = GetComponent<Image>();
        roll[0] = 0;
        StartCoroutine(RollDie());
    }
    // Update is called once per frame
    IEnumerator RollDie()
    {
        int x = 0;
        while (true)
        {
            if(x >= sprites.Length)
            {
                x = 0;
            }
            if (Input.anyKeyDown) { break; }
            img.sprite = sprites[x];
            x++;
            roll[0] = x;
            
            yield return new WaitForSecondsRealtime(0.1f);
           
        }
        SendDiceRoll();
        CreateVisualDice();
        yield return null;
    }

    private void SendDiceRoll()
    {
        Dice.instance.diceRollHistory.Add(new Dice.DiceRoll(roll));
    }

    private void CreateVisualDice()
    {
        var temp = new GameObject("TempSprite");
        var tempSRend = temp.AddComponent<SpriteRenderer>();
        tempSRend.sprite = sprites[roll[0]-1];
        temp.transform.SetParent(GameManager.instance.players[GameManager.instance.currentPlayerTurn].transform.Find("DiceHolder"));
    }
}
