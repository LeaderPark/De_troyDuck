using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum GameKey
{
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
    Attack,
    Dash,
    Skill1,
    Skill2,
    Skill3,
}

public class InputManager : Manager
{
    private Hashtable keyMappingTable; // 나중에 꼭 수정하자 // 그렇게 영원히 잊혀졌다...
    private Dictionary<KeyCode, ButtonState> keyTable;
    private Dictionary<MouseButton, ButtonState> mouseTable;
    public bool isTimeStop = false;

    void Awake()
    {
        keyTable = new Dictionary<KeyCode, ButtonState>();
        mouseTable = new Dictionary<MouseButton, ButtonState>();
        KeySetting();
    }

    void KeySetting()
    {
        keyTable[KeyCode.W] = ButtonState.None;
        keyTable[KeyCode.A] = ButtonState.None;
        keyTable[KeyCode.S] = ButtonState.None;
        keyTable[KeyCode.D] = ButtonState.None;
        keyTable[KeyCode.Q] = ButtonState.None;
        keyTable[KeyCode.E] = ButtonState.None;
        keyTable[KeyCode.R] = ButtonState.None;
        keyTable[KeyCode.F] = ButtonState.None;
        keyTable[KeyCode.Space] = ButtonState.None;

        mouseTable[MouseButton.Left] = ButtonState.None;
        mouseTable[MouseButton.Right] = ButtonState.None;
    }

    public bool CheckKeyState(KeyCode keyCode, ButtonState state)
    {
        return GetKeyState(keyCode) == state;
    }

    public bool CheckMouseState(MouseButton mouse, ButtonState state)
    {
        return GetMouseState(mouse) == state;
    }

    private ButtonState GetKeyState(KeyCode keyCode)
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
        if (!isTimeStop)
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
        else
        {
            foreach (KeyCode keyCode in keyTable.Keys.ToArray())
            {
                keyTable[keyCode] = ButtonState.None;
            }

            foreach (MouseButton mouse in mouseTable.Keys.ToArray())
            {
                mouseTable[mouse] = ButtonState.None;
            }
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
