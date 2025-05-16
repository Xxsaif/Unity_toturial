using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector] public static bool paused = false;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private EventSystem eventSystem;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Pause();
            }
            else if (paused)
            {
                Resume();
            }
        }
    }
    private void Pause()
    {
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
    }
    public void Resume()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
    }

    public void QuitToMainMenu()
    {
        paused = false;
        Time.timeScale = 1f;
        eventSystem.SetSelectedGameObject(null);
        SceneManager.LoadScene(0);
    }
}
