using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTextManager : MonoBehaviour {

    public Text won;

    public Text lost;

    public bool gameWon = GameManager.IsSuccessGameOver;

    private void Awake()
    {
        DisplayText();
    }

    public void DisplayText()
    {
        if (gameWon)
        {
            won.gameObject.SetActive(true);
            lost.gameObject.SetActive(false);
        }
        else
        {
            lost.gameObject.SetActive(true);
            won.gameObject.SetActive(false);
        }
    }

}
