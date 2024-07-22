using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Toggle : MonoBehaviour
{
    public  Sprite[] sprite;
    private bool isSprite1Active;
    private Sprite buttonImage;
    public GameObject[] elementtoBedisable;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager=FindAnyObjectByType<AudioManager>();
        if (gameObject.GetComponent<Image>().sprite != null)
            buttonImage = gameObject.GetComponent<Image>().sprite;


    }

   public  void ToggleSprite()
    {
        audioManager.Play("Button");
        if (isSprite1Active)
        {
            buttonImage= sprite[1];
           
        }
        else
        {
            buttonImage = sprite[0];
        }
        foreach(GameObject elements in elementtoBedisable)
        {
            elements.SetActive(isSprite1Active);
        }
        isSprite1Active = !isSprite1Active;
    }
}
