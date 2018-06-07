using UnityEngine;

public class ManageButtons : MonoBehaviour
{

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    public void StartExplorationGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
