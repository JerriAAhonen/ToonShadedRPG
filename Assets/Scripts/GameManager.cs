using UnityEngine;
using Util;

public class GameManager : SingletonBehaviour<GameManager>
{
    public Canvas pauseMenu;

    private void Start()
    {
        if (pauseMenu != null)
            pauseMenu.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (pauseMenu != null && Input.GetKeyDown(KeyCode.Escape))
        {
            // Pause the game
            var pauseMenuEnabled = !pauseMenu.enabled;
            pauseMenu.enabled = pauseMenuEnabled;
            Cursor.visible = pauseMenuEnabled;
            Cursor.lockState = pauseMenuEnabled ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
