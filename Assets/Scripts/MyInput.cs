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

public enum InputTickResult
{
    Pending,
    Confirm,
    Timeout,
}

public class MyInput : MonoBehaviour
{
    public const float TimeLimit = 5f;
    private float _timer;

    public List<PlayerInputs> myInputs;

    public void Init()
    {
        myInputs = new List<PlayerInputs>();
        _timer = 0;
    }

    public InputTickResult Tick()
    {
        if (Input.GetKeyDown(KeyCode.W)) myInputs.Add(PlayerInputs.Up);
        if (Input.GetKeyDown(KeyCode.A)) myInputs.Add(PlayerInputs.Left);
        if (Input.GetKeyDown(KeyCode.S)) myInputs.Add(PlayerInputs.Down);
        if (Input.GetKeyDown(KeyCode.D)) myInputs.Add(PlayerInputs.Right);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) myInputs.Add(PlayerInputs.CW);
        if (Input.GetKeyDown(KeyCode.RightArrow)) myInputs.Add(PlayerInputs.CounterCW);
        if (Input.GetMouseButtonDown(0)) myInputs.Add(PlayerInputs.ScaleDown);
        if (Input.GetMouseButtonDown(1)) myInputs.Add(PlayerInputs.ScaleUp);

        _timer += Time.deltaTime;

        if (_timer > TimeLimit)
        {
            return InputTickResult.Timeout;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            return InputTickResult.Confirm;
        }

        return InputTickResult.Pending;
    }
}
