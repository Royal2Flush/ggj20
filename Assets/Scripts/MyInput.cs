using System.Collections.Generic;
using UnityEngine;

public class MyInput : MonoBehaviour
{
    //...
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

    public List<PlayerInputs> myInputs;

    public bool Tick()
    {
        return false;
    }
}
