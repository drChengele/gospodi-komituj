using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageButtons : MonoBehaviour
{

    public void StartGame()
    {
        Application.LoadLevel(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
