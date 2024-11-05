using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : Singleton<InputManager>
{
    private ShortcutAction _shortcutAction;

    private void OnEnable()
    {
        if (_shortcutAction == null)
        {
            _shortcutAction = new ShortcutAction();
        }

        _shortcutAction.Enable();
    }

    private void OnDisable()
    {
        _shortcutAction.Disable();
    }

    public bool IsActionPressed(string action)
    {

        var inputAction = _shortcutAction.FindAction(action);
        if (inputAction.triggered)
        {
            return true;
        }

        return false;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
}