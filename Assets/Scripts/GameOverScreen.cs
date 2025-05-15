using UnityEngine;
using UnityEngine.SceneManagement;
// Created by Herman Bergström
public class GameOverScreen : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
