using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStates : MonoBehaviour
{
    public Camera Camera;
    public MyInput MyInput;

    public Sprite[] Sprites;

    [Header("UI")]
    public GameObject Splash;
    public Text CountdownText;
    public Image PlayerImage;
    public Image TargetImage;

    [Space]
    public Level[] levels;

    public int translateCoeff = 100;
    public int rotateCoeff = 30;
    public float scaleCoeff = 1.5f;

    public GameState CurrentState;
    public MyTransform PlayerTransform;
    public MyTransform TargetTransform;
    public Level CurrentLevel;
    public int CurrentLevelId;

    private bool _isCountingDown = false;

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
        PlayerTransform = CurrentLevel.start;
        TargetTransform = CurrentLevel.target;

        PlayerImage.gameObject.SetActive(false);
        TargetImage.gameObject.SetActive(false);

        ChangeState(GameState.Splash);
    }

    void Update()
    {
        if (CurrentState == GameState.Splash)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Splash.SetActive(false);
                ChangeState(GameState.Countdown);
            }
        }
        else if (CurrentState == GameState.Countdown)
        {
            if (!_isCountingDown)
            {
                StartCoroutine(CountdownCoroutine());
            }
        }
        else if (CurrentState == GameState.Input)
        {
            Camera.backgroundColor = CurrentLevel.bgColor;

            PlayerImage.gameObject.SetActive(true);
            TargetImage.gameObject.SetActive(true);

            PlayerImage.color = CurrentLevel.bgColor * 0.9f;
            TargetImage.color = CurrentLevel.spriteColor;

            MyInput.Tick(ref PlayerTransform);
        }
    }

    private IEnumerator CountdownCoroutine()
    {

        _isCountingDown = true;
        CountdownText.text = "3";
        yield return new WaitForSeconds(1f);
        CountdownText.text = "2";
        yield return new WaitForSeconds(1f);
        CountdownText.text = "1";
        yield return new WaitForSeconds(1f);
        ChangeState(GameState.Input);
        _isCountingDown = false;
    }

    void ChangeState(GameState nextState)
    {
    	CurrentState = nextState;
    }

}
