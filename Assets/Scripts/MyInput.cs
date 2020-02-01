using System.Collections.Generic;
using UnityEngine;

public enum PlayerInputs
{
    Left,
    Right,
    Up,
    Down,
    CW,
    CounterCW,
    ScaleUp,
    ScaleDown
}

public class MyInput : MonoBehaviour
{
    //...

    public List<PlayerInputs> myInputs;

    public bool Tick()
    {
        return false;
    }
}
