using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ReelSetup : MonoBehaviour
{
    public Transform reelContainer; // Parent transform to hold all reels
    public Sprite[] symbolSprites; // Array of symbols
    private int rows = 3;

    void Start()
    {
        //PopulateReels();
    }

    void PopulateReels()
    {
        foreach (Transform reel in reelContainer)
        {
            PopulateReel(reel);
        }
    }
    void PopulateReel(Transform reel)
    {
        for (int i = 0; i < rows + 2; i++) // +2 for the extra symbols above and below the visible area
        {
            Image symbolImage = reel.GetChild(i).GetComponent<Image>();
            int randomIndex = Random.Range(0, symbolSprites.Length);
            symbolImage.sprite = symbolSprites[randomIndex];
        }
    }

}
