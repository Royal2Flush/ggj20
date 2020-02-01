using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStates : MonoBehaviour
{
    public Sprite[] Sprites;

    public enum GameState {
        Splash,
        Countdown,
        Input,
        Play,
        Transition,
        Win,
        Lose
    }

    public Level[] levels;

    public struct MyTransform
    {
        public int x;
        public int y;
        public int rotation;
        public int scale;
    }

    public struct Level
    {
        public MyTransform target;
        public MyTransform start;
        public Sprite sprite;
        public Color32 bgColor;
        public Color32 spriteColor;
    }

    public int translateCoeff = 100;
    public int rotateCoeff = 30;
    public float scaleCoeff = 1.5f;

    public GameState CurrentState;
    public MyTransform UserTranform;
    public MyTransform TargetTransform;
    public Level CurrentLevel;
    public int CurrentLevelId;

    void Start()
    {
        levels = new Level[]
        {
            new Level
            {
                target = new MyTransform
                {
                    x = 5, y = 3, rotation = 7, scale = 3
                },
                start = new MyTransform
                {
                    x = 3, y = 5, rotation = 0, scale = 1
                },
                sprite = Sprites[0],
                bgColor = Color.red,
                spriteColor = Color.blue
            }
        };

        CurrentLevelId = 0;
        CurrentLevel = levels[CurrentLevelId];
        UserTranform = CurrentLevel.start;
        TargetTransform = CurrentLevel.target;
        

        ChangeState(GameState.Splash);
        
    }

    void Update()
    {
        if (CurrentState == GameState.Splash)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ChangeState(GameState.Countdown);
            }
        }
        else if (CurrentState == GameState.Countdown)
        {

        }
        else if (CurrentState == GameState.Input)
        {

        }
    }

    void ChangeState(GameState nextState) {
    	CurrentState = nextState;
    }

}
