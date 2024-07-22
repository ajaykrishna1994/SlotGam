using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelController : MonoBehaviour
{
     public ReelSpin[] reels; // Array of ReelSpin scripts attached to each reel
    public float delayBetweenReels = 0; // Delay between each reel start
    public float staggerDelay = 0.5f;
    void Start()
    {
        // Optional: Start spinning reels immediately
      //  StartSpinning();
    }

    public void StartSpinning()
    {
        foreach (var reel in reels)
        {
            reel.StartSpin();
        }
        StartCoroutine(StopReelsWithDelay());
    }

    private IEnumerator StopReelsWithDelay()
    {
        foreach (var reel in reels)
        {
            yield return new WaitForSeconds(staggerDelay);
            reel.StopSpin();
        }
    }
}
