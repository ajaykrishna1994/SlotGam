using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReelSpin : MonoBehaviour
{
    public float spinSpeed = 500f; // Speed at which the reel spins
    public float spinDuration = 2f; // Duration of the spin
    private bool isSpinning = false;

    private RectTransform[] symbols;
    private int numSymbols = 12; // Total number of symbols per reel
    private float symbolHeight;
    private float reelHeight; // Height of the entire reel
    private VerticalLayoutGroup vertical;

    void Start()
    {
        vertical = gameObject.GetComponent<VerticalLayoutGroup>();
        // Initialize symbols array
        symbols = new RectTransform[numSymbols];
        for (int i = 0; i < numSymbols; i++)
        {
            symbols[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }

        // Calculate the height of a symbol and the reel
        if (symbols.Length > 0 && symbols[0] != null)
        {
            symbolHeight = symbols[0].sizeDelta.y;
        }
        reelHeight = symbolHeight * numSymbols;
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            StartCoroutine(Spin());
        }
        Invoke("EnableVertical", 3f);
    }

    public void EnableVertical()
    {
        vertical.enabled = true;
    }

    public void StopSpin()
    {
        isSpinning = false;
    }

    private IEnumerator Spin()
    {
        RectTransform lastSymbol = symbols[symbols.Length - 1];
        float lastSymbolYPosition = lastSymbol.anchoredPosition.y - 10f;
        RectTransform firstSymbol = symbols[0];
        float firstSymbolYPosition = firstSymbol.anchoredPosition.y;
        isSpinning = true;
        float elapsedTime = 0f;

        while (isSpinning)
        {
            foreach (var symbol in symbols)
            {
                if (symbol == null)
                {
                    Debug.LogError("A symbol in the array is null.");
                    continue;
                }

                RectTransform symbolTransform = symbol;
                symbolTransform.anchoredPosition -= new Vector2(0, spinSpeed * Time.deltaTime);

                if (symbolTransform.anchoredPosition.y <= lastSymbolYPosition)
                {
                    symbolTransform.anchoredPosition = new Vector2(50, firstSymbolYPosition - 10);
                    // Optional: Trigger animation for landing symbols
                    Animator animator = symbol.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.SetTrigger("Land");
                    }
                    else
                    {
                        Debug.LogWarning("Animator component not found on symbol.");
                    }
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
            vertical.enabled = false;
        }

        // Ensure the final position shows the last 3 symbols
        PositionFinalSymbols();
    }

    private void PositionFinalSymbols()
    {
        // Ensure that the final 3 symbols are visible
        float offset = symbolHeight * 3;

        foreach (var symbol in symbols)
        {
            RectTransform symbolTransform = symbol;
            if (symbolTransform.anchoredPosition.y < -symbolHeight * (numSymbols - 3))
            {
                symbolTransform.anchoredPosition += new Vector2(0, reelHeight);
            }
        }
    }
}
