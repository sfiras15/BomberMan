using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Application.targetFrameRate = 60;
    }
    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
