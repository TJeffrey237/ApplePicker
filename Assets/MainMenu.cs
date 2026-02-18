using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayEasy()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void PlayMedium()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void PlayHard()
    {
        SceneManager.LoadSceneAsync(3);
    }
}
