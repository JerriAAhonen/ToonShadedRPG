﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Canvas pauseMenu;

    private void Start()
    {
        pauseMenu.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Pause the game
            var pauseMenuEnabled = !pauseMenu.enabled;
            pauseMenu.enabled = pauseMenuEnabled;
            Cursor.visible = pauseMenuEnabled;
            Cursor.lockState = pauseMenuEnabled ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
