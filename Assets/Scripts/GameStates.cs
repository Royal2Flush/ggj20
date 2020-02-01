using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStates : MonoBehaviour
{
    public Camera Camera;
    public MyInput MyInput;

    public Sprite[] Sprites;

    [Header("UI")]
    public GameObject Splash;
    public TextMeshProUGUI CountdownText;
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
    private bool _isInputState = false;
    private bool _isPlaying = false;

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

        Splash.SetActive(true);
        CountdownText.text = "";

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
            if (!_isInputState)
            {
                _isInputState = true;
                Camera.backgroundColor = CurrentLevel.bgColor;

                PlayerImage.gameObject.SetActive(true);
                TargetImage.gameObject.SetActive(true);

                PlayerImage.color = CurrentLevel.bgColor * 0.9f;
                TargetImage.color = CurrentLevel.spriteColor;
                MyInput.Init();
            }

            var res = MyInput.Tick();
            if (res == InputTickResult.Confirm)
            {
                _isInputState = false;
                ChangeState(GameState.Play);
            }
            else if (res == InputTickResult.Timeout)
            {
                _isInputState = false;
                ChangeState(GameState.Lose);
            }
        }
        else if (CurrentState == GameState.Play)
        {
            if (!_isPlaying)
            {
                StartCoroutine(PlayCoroutine(new List<PlayerInputs>()));
            }
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

    private IEnumerator PlayCoroutine(List<PlayerInputs> inputs)
    {
        _isPlaying = true;
        float playSpeed = 0.5f;
        foreach (var inp in inputs) {
            if (inp == PlayerInputs.Left)
            {
                PlayerTransform.x--;
            }
            else if (inp == PlayerInputs.Right)
            {
                PlayerTransform.x++;
            }
            else if (inp == PlayerInputs.Up)
            {
                PlayerTransform.y++;
            }
            else if (inp == PlayerInputs.Down)
            {
                PlayerTransform.y--;
            }
            else if (inp == PlayerInputs.CW)
            {
                PlayerTransform.rotation++;
            }
            else if (inp == PlayerInputs.CounterCW)
            {
                PlayerTransform.rotation--;
            }
            else if (inp == PlayerInputs.ScaleUp)
            {
                PlayerTransform.scale++;
            }
            else if (inp == PlayerInputs.ScaleDown)
            {
                PlayerTransform.scale--;
            }
            PlayerImage.gameObject.transform.position = new Vector3(PlayerTransform.x * translateCoeff, PlayerTransform.y * translateCoeff, 0);
            PlayerImage.gameObject.transform.Rotate(new Vector3(0, 0, PlayerTransform.rotation * rotateCoeff));
            PlayerImage.gameObject.transform.localScale = new Vector3(1, 1, 1) * PlayerTransform.scale * scaleCoeff;
            yield return new WaitForSeconds(playSpeed);
        }
        if (IsTransformsEqual(PlayerTransform, TargetTransform))
        {
            ChangeState(GameState.Transition);
        }
        else
        {
            ChangeState(GameState.Lose);
        }
        _isPlaying = false;
    }

    void ChangeState(GameState nextState)
    {
    	CurrentState = nextState;
    }

    bool IsTransformsEqual(MyTransform player, MyTransform target)
    {
        return player.x == target.x && player.y == target.y
            && player.rotation == target.rotation
            && player.scale == target.scale;
    }

}
