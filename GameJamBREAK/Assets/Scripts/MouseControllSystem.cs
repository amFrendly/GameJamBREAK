using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseControllSystem : StandaloneInputModule
{
    Mouse mouse;

    Vector2 mousePos;

    [SerializeField] List<KeyCode> up;
    [SerializeField] List<KeyCode> left;
    [SerializeField] List<KeyCode> down;
    [SerializeField] List<KeyCode> right;
    [SerializeField] float mouseSpeed = 360;

    void Start()
    {
        mouse = Mouse.current;
        mousePos = Input.mousePosition;
    }

    
    void Update()
    {
        MoveMouseIfKeyInListPressed(up, new Vector2(0,1));
        MoveMouseIfKeyInListPressed(left, new Vector2(-1,0));
        MoveMouseIfKeyInListPressed(down, new Vector2(0,-1));
        MoveMouseIfKeyInListPressed(right, new Vector2(1,0));



        if (Input.GetKeyDown("[3]"))
        {
            ClickAt(mousePos, true);
        }
        else if (Input.GetKeyUp("[3]"))
        {
            ClickAt(mousePos, false);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ClickAt(mousePos, true);
        }
        else if (Input.GetKeyUp(KeyCode.M))
        {
            ClickAt(mousePos, false);
        }
    }

    void MoveMouseIfKeyInListPressed(List<KeyCode> keys, Vector2 direction)
    {
        foreach (KeyCode keyCode in keys) 
        {
            if (Input.GetKey(keyCode))
            {
                mousePos += direction * mouseSpeed * Time.unscaledDeltaTime;
                mouse.WarpCursorPosition(mousePos);
                return;
            }
        }
    }

    void ClickAt(Vector2 pos, bool pressed)
    {
        Input.simulateMouseWithTouches = true;
        var pointerData = GetTouchPointerEventData(new Touch()
        {
            position = mousePos
        }, out bool b, out bool bb);

        ProcessTouchPress(pointerData, pressed, !pressed);
    }
}
