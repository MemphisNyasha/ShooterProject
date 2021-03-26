using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private bool isPaused = false;
    private CursorSettings cursorSettings;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public bool Quit()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    public bool EnterCombatState()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }

    public bool GetMoveInput()
    {
        return Input.GetMouseButton(0);
    }

    public float GetHorizontal()
    {
        return Input.GetAxis("Mouse X");
    }

    public float GetVertical()
    {
        return Input.GetAxis("Mouse Y");
    }

    public bool GetFireInput()
    {
        return Input.GetMouseButton(0);
    }
    
    public bool GetFireInputUp()
    {
        return Input.GetMouseButtonUp(0);
    }
    
    public bool GetFireInputDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;

            Cursor.lockState = cursorSettings.LockState;
            Cursor.visible = cursorSettings.IsVisible;
        }
        else
        {
            Time.timeScale = 0;

            cursorSettings.IsVisible = Cursor.visible;
            cursorSettings.LockState = Cursor.lockState;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        isPaused = !isPaused;
    }
}

public struct CursorSettings
{
    public CursorLockMode LockState;
    public bool IsVisible;
}
