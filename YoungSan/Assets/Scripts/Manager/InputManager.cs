using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : Manager
{
    private Dictionary<KeyCode, ButtonState> keyTable;
    private Dictionary<MouseButton, ButtonState> mouseTable;

    void Awake()
    {
        keyTable = new Dictionary<KeyCode, ButtonState>();
        mouseTable = new Dictionary<MouseButton, ButtonState>();
    }

    void Start()
    {
        keyTable[KeyCode.W] = ButtonState.None;
        keyTable[KeyCode.A] = ButtonState.None;
        keyTable[KeyCode.S] = ButtonState.None;
        keyTable[KeyCode.D] = ButtonState.None;

        mouseTable[MouseButton.Left] = ButtonState.None;
        mouseTable[MouseButton.Right] = ButtonState.None;
    }

    public ButtonState GetKeyState(KeyCode keyCode)
    {
        if (keyTable.ContainsKey(keyCode))
        {
            return (ButtonState)keyTable[keyCode];
        }
        return ButtonState.None;
    }

    public ButtonState GetMouseState(MouseButton mouse)
    {
        if (mouseTable.ContainsKey(mouse))
        {
            return (ButtonState)mouseTable[mouse];
        }
        return ButtonState.None;
    }

    void Update()
    {
        foreach (KeyCode keyCode in keyTable.Keys.ToArray())
        {
            if (Input.GetKeyDown(keyCode))
            keyTable[keyCode] = ButtonState.Down;
            else if (Input.GetKeyUp(keyCode))
            keyTable[keyCode] = ButtonState.Up;
            else if (Input.GetKey(keyCode))
            keyTable[keyCode] = ButtonState.Stay;
            else
            keyTable[keyCode] = ButtonState.None;
        }

        foreach (MouseButton mouse in mouseTable.Keys.ToArray())
        {
            if (Input.GetMouseButtonDown((int)mouse))
            mouseTable[mouse] = ButtonState.Down;
            else if (Input.GetMouseButtonUp((int)mouse))
            mouseTable[mouse] = ButtonState.Up;
            else if (Input.GetMouseButton((int)mouse))
            mouseTable[mouse] = ButtonState.Stay;
            else
            mouseTable[mouse] = ButtonState.None;
        }
        
    }
}

public enum ButtonState
{
    None,
    Down,
    Stay,
    Up
}

public enum MouseButton
{
    Left,
    Right,
    Wheel,
    Button1,
    Button2,
    Button3,
    Button4
}