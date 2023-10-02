using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseIdea : MonoBehaviour
{
    

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int RollNSidedDice(int min=1, int max=6,int diceCount=1) {

        int sum = 0;

        for (int x = 0; x<=diceCount; x++)
        {
           sum += (int)Random.Range(min, max);
        }
        return sum;
    }



    

    
}
