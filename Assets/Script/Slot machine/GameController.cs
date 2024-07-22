using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action handlePulled = delegate { };

    [SerializeField]
    private TextMeshProUGUI priceText;
    [SerializeField]
    private Row[] rows; // Assuming rows array is set to 5 in Unity Inspector

    private int prizeValue;
    private bool isResult;
    private Dictionary<string, int> symbolPrizes = new Dictionary<string, int>();
    private AudioManager audioManager;
    private bool isButtonActive;
    public string[] winningSymbols = { "WILD", "WILD", "WILD" };
    private bool spinningInProgress = false;
    [SerializeField]
    private GameObject payline;
    private Animator paylineAnim;
    private List<string> reelResults = new List<string>();
    private bool isrotate;
    private int prize;


    void Start()
    {
        
            if (rows.Length > 0)
            {
                symbolPrizes = new Dictionary<string, int>(rows[0].symbolValues);
            }
            audioManager = FindAnyObjectByType<AudioManager>();
          
       
        // Initialize symbolPrizes from the first Row
       
     
    }
  
    public void StartRotate()
    {
        if (!isrotate)
        {
            isrotate = true;
            handlePulled();
            audioManager.Play("Button");
            isButtonActive = true;
            StartCoroutine(SpinReels());

        }
        
       
    }

    private IEnumerator SpinReels()
    {
        // Start spinning each reel
      

        // Wait until all reels stop spinning
        bool allReelsStopped = false;
        while (!allReelsStopped)
        {
            allReelsStopped = true;
            foreach (Row row in rows)
            {
                if (row.isSpinning)
                {
                    allReelsStopped = false;
                    break;
                }
            }
            yield return null;
        }

      
        spinningInProgress = false;
      
        Invoke("CheckResults", 1f);
    }


    public void CheckResult()
    {
        // Collect stopped symbols from all rows
        string[] stoppedSymbols = new string[rows.Length];
        for (int i = 0; i < rows.Length; i++)
        {
            stoppedSymbols[i] = rows[i].stoppedSloat;
        }

        // Check for matching symbols
        bool allMatch = true;
        for (int i = 1; i < stoppedSymbols.Length; i++)
        {
            if (stoppedSymbols[i] != stoppedSymbols[0])
            {
                allMatch = false;
                break;
            }
        }

        if (allMatch && symbolPrizes.ContainsKey(stoppedSymbols[0]))
        {
            prizeValue = symbolPrizes[stoppedSymbols[0]];
            isResult = true;
        }
        else
        {
            // Check for specific combinations
            if (IsSpecialCombination(stoppedSymbols))
            {
                prizeValue = 1000; // Example prize value for special combinations
                isResult = true;
            }
            else
            {
                prizeValue = 0; // No prize
                isResult = true;
            }
        }

        // Update the UI
        priceText.enabled = true;
        priceText.text = "Prize " + prizeValue;
    }
    private void CheckResults()
    {
        // Collect dummy responses from each reel
    
     
        foreach (Row reel in rows)
        {
            reelResults.Add(reel.GetDummyResponse());
          
        }


        int wildCount = 0;
        foreach (string result in reelResults)
        {
            if (result == "WILD")
            {
                wildCount++;
            }
        }
        if (wildCount > 2)
        {
            prize = prize + 100;
            priceText.text= prize.ToString();
            Debug.Log("You win!");
            audioManager.Play("Bigwin");
            payline.SetActive(true);
            Transform child=payline.transform.GetChild(0);
            paylineAnim = child.GetComponent<Animator>();
            paylineAnim.SetTrigger("PayLinestart");
            Invoke("StopAnimation", 2f);
           
        }
        //
        reelResults.Clear();
        isrotate = false;
    }
    public void StopAnimation()
    {
        paylineAnim.SetTrigger("StopPayline");
        payline.SetActive(false);
    }
    private bool CheckWinningCondition(List<string> results)
    {
        // Check if the results match the predefined winning symbols
        int matchedSymbols = 0;
        foreach (string result in results)
        {
            if (Array.Exists(winningSymbols, symbol => symbol == result))
            {
                matchedSymbols++;
            }
        }

        // Example condition: All symbols in the winning pattern should match
        return matchedSymbols == winningSymbols.Length;
    }
    private bool IsSpecialCombination(string[] symbols)
    {
        // Example special combinations
        return (symbols[0] == "H1" && symbols[1] == "H2" && symbols[2] == "H3") ||
               (symbols[0] == "Bonus" && symbols[1] == "Wild" && symbols[2] == "Scatter") ||
               (symbols[0] == "L1" && symbols[1] == "L2" && symbols[2] == "L3") ||
               (symbols[0] == "Bonus" && symbols[1] == "L1" && symbols[2] == "L3");
    }

    public void UpdateSymbolValue(string symbol, int value)
    {
        if (symbolPrizes.ContainsKey(symbol))
        {
            symbolPrizes[symbol] = value;
        }
    }

    void Update()
    {
       // HandleGameLogic();
    }

   
}
