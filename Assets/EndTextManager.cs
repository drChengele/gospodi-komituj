using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTextManager : MonoBehaviour {

    public Text won;
    public Text lost;
    public Text stats;
    
    private void Awake()
    {
        DisplayText();
    }

    public void DisplayText()
    {
        stats.text =  $"Distance travelled : {GameManager.GlobalData.MetersCrossed} kliks"
                    + $"\nBounty accrued: {GameManager.GlobalData.Bounty}$$"
                    + "\nPress enter to continue";

        won.gameObject.SetActive(GameManager.GlobalData.IsGameOverAVictory);
        lost.gameObject.SetActive(!GameManager.GlobalData.IsGameOverAVictory);
    }

}
