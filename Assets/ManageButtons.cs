using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageButtons : MonoBehaviour
{

    public void StartGame()
    {
        Application.LoadLevel(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
