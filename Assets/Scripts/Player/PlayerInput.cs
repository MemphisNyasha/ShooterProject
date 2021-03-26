﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
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
}
