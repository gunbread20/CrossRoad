using System;
using UniRx;
using UnityEngine;

public class WindowInputComponent : InputComponent
{

    public WindowInputComponent()
    {

    }

    public override Direction GetDirection(string keycode)
    {
        switch (keycode.ToUpper())
        {
            case "W":
                return Direction.Forward;
            case "A":
                return Direction.Left;
            case "S":
                return Direction.Back;
            case "D":
                return Direction.Right;
            default:
                return Direction.None;
        }
    }

}