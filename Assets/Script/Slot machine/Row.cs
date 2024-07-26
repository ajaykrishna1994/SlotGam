using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;


public class Row : MonoBehaviour
{
    private int randomValue;
    private float timeIntervel;
    public bool isRowStop;
    public string stoppedSloat;
    private GameController gameController;
    RectTransform rectTransform;
    private AudioManager audioManager;
    private List<RectTransform> symbolList = new List<RectTransform>();

    [SerializeField]
    private float spinSpeed = 20.47f; // Control the speed of the spin

    [SerializeField]
    private float spinDuration = 0.5f; // Control the duration of the spin
    private string dummyResponse;
  
    public bool isSpinning=true;
    private int symbolCounter;
        // Dictionary for symbol values
    public Dictionary<string, int> symbolValues = new Dictionary<string, int>
    {
        {"H1", 100},
        {"H2", 200},
        {"H3", 300},
        {"H4", 400},
        {"L1", 500},
        {"L2", 600},
        {"L3", 700},
        {"L4", 800},
        {"Scatter", 1000},
        {"Wild", 1200},
        {"Bonus", 1500}
    };

    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        rectTransform = GetComponent<RectTransform>();
        isRowStop = true;
        GameController.handlePulled += StartRotating;
        gameController = FindObjectOfType<GameController>();
        CollectSymbols();
    }
    private void CollectSymbols()
    {
        
        symbolList.Clear();

       
        RectTransform reelRectTransform  = gameObject.GetComponent<RectTransform>();

        foreach (RectTransform child in reelRectTransform)
        {
            
            symbolList.Add(child);
        }
       

       
    }

    private void StartRotating()
    {
        isSpinning=true;
        isRowStop = true;
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        audioManager.Play("Spin");
        isRowStop = false;

     
        float duration = spinDuration; // Duration for smooth downward movement
      
        float startInterval = spinDuration / 30f; // Fastest animation start
        float endInterval = spinDuration / 5f; // Slowest animation end

        randomValue = Random.Range(100, 140);
        for (int i = 0; i < randomValue; i++)
        {
            if (rectTransform.anchoredPosition.y <= -321f)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -85.5f);
            }
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - spinSpeed);

            // Smooth transition calculation
            float t = (float)i / randomValue; // Calculate the progress from 0 to 1
            float smoothInterval = Mathf.Lerp(startInterval, endInterval, Mathf.SmoothStep(0, 1, t)); // Smooth transition of interval InterpolatedValue=initialY+(targetY−initialY)×smoothValue
            //(elapsedTime / stopDuration));

            yield return new WaitForSeconds(smoothInterval);
        }

       
        float[] stopPositions = { -85.5f, -103f, -122.56f, -142f, -161.4f, -182.5f, -204.4f, -221.2f, -245.1f, -262.8f, -283.2f, -300.5f, -321.6f };
        System.Array.Reverse(stopPositions);

        
        DetermineStoppedSloat(stopPositions);

        StopSpinning();
    }
    public void StopSpinning()
    {
        audioManager.Play("StopSpin");
        isRowStop = true;
      
    }
    public bool IsSpinning()
    {
        return isRowStop;
    }
    private void DetermineStoppedSloat(float[] stopPositions)
    {
        symbolCounter = 0;
        dummyResponse = "";
       // float epsilon = 4.4f; //

        // Map positions to symbols

        float closestPosition = stopPositions[0];
        float minDistance = Mathf.Abs(rectTransform.anchoredPosition.y - closestPosition);

        foreach (float position in stopPositions)
        {
            float distance = Mathf.Abs(rectTransform.anchoredPosition.y - position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPosition = position;
            }
        }
      

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, closestPosition);
        float topmostVisiblePosition = rectTransform.position.y;

        foreach (RectTransform symbol in symbolList)
        {
            Vector3 childLocalPosition = symbol.localPosition;
            Vector3 childWorldPosition = rectTransform.TransformPoint(childLocalPosition);
            float childWorldPositionY = childWorldPosition.y;
         
            if ((childWorldPositionY >= 500 && childWorldPositionY <= 1200) )
            {
            
                // Optional: Set the dummy response based on the symbol's name if needed
                if (symbol.gameObject.name == "WILD")
                {
                    Debug.Log(symbol.gameObject.name);
                    symbolCounter++;
                    dummyResponse = "WILD";
                
                    
                  


                }
                else
                {
                    audioManager.Play("lowPay");
                }
            }
        }
        isSpinning = false;

    }

    
    public string GetDummyResponse()
    {
        return dummyResponse;
    }

    private void OnDestroy()
    {
        GameController.handlePulled -= StartRotating;
    }
}
