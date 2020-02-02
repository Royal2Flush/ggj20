using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class GameStates : MonoBehaviour
{
    public List<string> TshirtSize = new List<string>
    {
        "XXS",
        "XS",
        "S",
        "M",
        "L",
        "XL",
        "XXL"
    };

    public Camera Camera;
    public MyInput MyInput;

    public Sprite[] Sprites;

    public GameObject Dot;
    public Transform DotParent;

    [Header("UI")]
    public GameObject Splash;
    public GameObject GameOverText;
    public GameObject WinUI;
    public TextMeshProUGUI CountdownText;
    public TextMeshProUGUI SplashText;
    public TextMeshProUGUI ProgressText;
    public TextMeshProUGUI CukText;
    public Image PlayerImage;
    public Image TargetImage;
    public TextMeshProUGUI PlayerText;
    public TextMeshProUGUI TargetText;
    public AnimationCurve LerpCurve;

    [Space]

    public Level[] levels;

    private int translateCoeff = 100;
    private int rotateCoeff = 45;
    private float scaleCoeff = 1.2f;

    public GameState CurrentState;
    public MyTransform PlayerTransform;
    public MyTransform TargetTransform;
    public Level CurrentLevel;
    public int CurrentLevelId;

    private bool _isCountingDown = false;
    private bool _isInputState = false;
    private bool _isPlaying = false;
    private Material dotMaterial;

    void Start()
    {
        for (int i = 0; i <= Screen.width / 100; i++)
        {
            for (int j = 0; j <= Screen.height / 100; j++)
            {
                GameObject go = Instantiate(Dot, new Vector3(i * 100, j * 100, 0), Quaternion.identity, DotParent);
                dotMaterial = go.GetComponent<Image>().material;
            }
        }

        InitLevels();

        LoadLevel(0);

        ChangeState(GameState.Splash);

    }

    private void LoadLevel(int index)
    {
        CurrentLevelId = index;
        CurrentLevel = levels[CurrentLevelId];
        PlayerTransform = CurrentLevel.start;
        TargetTransform = CurrentLevel.target;
        UpdateSizeTexts();

        PlayerImage.gameObject.SetActive(false);
        TargetImage.gameObject.SetActive(false);

        GameOverText.SetActive(false);
        WinUI.gameObject.SetActive(false);
        CountdownText.text = "";
        ProgressText.text = "";
        CukText.text = "";

        ChangeState(GameState.Countdown);
    }

    void Update()
    {
        if (CurrentState == GameState.Splash)
        {
            Splash.SetActive(true);
            if (Input.GetButtonDown("P1XboxX"))
            {
                ChangeState(GameState.Countdown);
            }
        }
        else if (CurrentState == GameState.Countdown)
        {
            if (!_isCountingDown)
            {
                Splash.SetActive(false);

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

                PlayerImage.rectTransform.position = new Vector3(PlayerTransform.x * translateCoeff, PlayerTransform.y * translateCoeff, 0);
                PlayerImage.rectTransform.localRotation = Quaternion.Euler(0, 0, PlayerTransform.rotation * rotateCoeff);
                PlayerImage.rectTransform.localScale = new Vector3(1, 1, 1) * PlayerTransform.scale * scaleCoeff;

                TargetImage.rectTransform.position = new Vector3(TargetTransform.x * translateCoeff, TargetTransform.y * translateCoeff, 0);
                TargetImage.rectTransform.localRotation = Quaternion.Euler(0, 0, TargetTransform.rotation * rotateCoeff);
                TargetImage.rectTransform.localScale = new Vector3(1, 1, 1) * TargetTransform.scale * scaleCoeff;

                UpdateSizeTexts();

                //TargetImage.color = CurrentLevel.bgColor * 0.9f;
                TargetImage.color = Color.white;
                PlayerImage.color = CurrentLevel.spriteColor;
                dotMaterial.color = PlayerImage.color;

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
                var list = MyInput.myInputs;
                StartCoroutine(PlayCoroutine(list));
            }
        }
        else if (CurrentState == GameState.Transition)
        {
            CurrentLevelId++;
            LoadLevel(CurrentLevelId);
        }
        else if (CurrentState == GameState.Lose)
        {
            GameOverText.gameObject.SetActive(true);
            if (Input.GetButtonDown("P1XboxX"))
            {
                InitLevels();
                LoadLevel(0);
            }
        }
        else if (CurrentState == GameState.Win)
        {
            WinUI.gameObject.SetActive(true);
            if (Input.GetButtonDown("P1XboxX"))
            {
                InitLevels();
                LoadLevel(0);
            }
        }
    }

    private IEnumerator CountdownCoroutine()
    {

        _isCountingDown = true;
        ProgressText.text = CurrentLevelId.ToString() + " out of " + levels.Length.ToString();
        CountdownText.text = "3";
        SplashText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        CountdownText.text = "2";
        yield return new WaitForSeconds(1f);
        CountdownText.text = "1";
        yield return new WaitForSeconds(1f);
        CountdownText.text = "";
        ProgressText.text = "";
        SplashText.gameObject.SetActive(false);
        ChangeState(GameState.Input);
        _isCountingDown = false;
    }

    private IEnumerator PlayCoroutine(List<PlayerInputs> inputs)
    {
        _isPlaying = true;
        for (int i = 0; i < inputs.Count; i++)
        {
            yield return new WaitForSeconds(0.5f);
            PlayerInputs inp = inputs[i];
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
                PlayerTransform.rotation--;
            }
            else if (inp == PlayerInputs.CounterCW)
            {
                PlayerTransform.rotation++;
            }
            else if (inp == PlayerInputs.ScaleUp)
            {
                PlayerTransform.scale++;
            }
            else if (inp == PlayerInputs.ScaleDown)
            {
                PlayerTransform.scale--;
            }
            PlayerTransform.x = Mathf.Clamp(PlayerTransform.x, 0, 15);
            PlayerTransform.y = Mathf.Clamp(PlayerTransform.y, 0, 15);
            PlayerTransform.scale = Mathf.Clamp(PlayerTransform.scale, 1, 7);
            PlayerTransform.rotation += 8;
            PlayerTransform.rotation %= 8;

            StartCoroutine(LerpCoroutine(PlayerImage.rectTransform, PlayerTransform));
            MyInput.PaintAsDone(i, CurrentLevel.spriteColor);
        }
        if (IsTransformsEqual(PlayerTransform, TargetTransform))
        {
            CukText.transform.position = TargetImage.gameObject.transform.position;
            CukText.text = "CUK";
            yield return new WaitForSeconds(0.5f);
            CukText.text = "";
            if (CurrentLevelId+1 == levels.Length)
            {
                ChangeState(GameState.Win);
            }
            else
            {
                ChangeState(GameState.Transition);
            }
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

    void UpdateSizeTexts()
    {
        PlayerText.text = TshirtSize[PlayerTransform.scale - 1];
        TargetText.text = TshirtSize[TargetTransform.scale - 1];
    }

    private IEnumerator LerpCoroutine(RectTransform rectTransform, MyTransform myTransform)
    {
        var sourcePos = rectTransform.position;
        var sourceRot = rectTransform.localRotation;
        var sourceScale = rectTransform.localScale;

        var targetPos = new Vector3(myTransform.x * translateCoeff, myTransform.y * translateCoeff, 0);
        var targetRot = Quaternion.Euler(0, 0, myTransform.rotation * rotateCoeff);
        var targetScale = Vector3.one * myTransform.scale * scaleCoeff;

        const float duration = 0.25f;

        for (float f = 0; f < duration; f += Time.deltaTime)
        {
            float t = LerpCurve.Evaluate(f / duration);

            rectTransform.position = Vector3.Lerp(sourcePos, targetPos, t);
            rectTransform.localRotation = Quaternion.Slerp(sourceRot, targetRot, t);
            rectTransform.localScale = Vector3.Lerp(sourceScale, targetScale, t);

            yield return null;
        }


        rectTransform.position = targetPos;
        rectTransform.localRotation = targetRot;
        rectTransform.localScale = targetScale;
        UpdateSizeTexts();
    }

}
