using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [HideInInspector]
    public List<PlayerInputs> myInputs;

    public Transform InputIconsParent;
    public GameObject InputIconPrefab;
    public Slider Slider;
    public AnimationCurve PaintCurve;

    [Header("InputIcons")]

    public Sprite LeftSprite;
    public Sprite RightSprite;
    public Sprite UpSprite;
    public Sprite DownSprite;
    public Sprite CwSprite;
    public Sprite CCwSprite;
    public Sprite ScaleUpSprite;
    public Sprite ScaleDownSprite;

    private Dictionary<PlayerInputs, Sprite> _resourceMap;

    private bool confirm1;
    private bool confirm2;

    private void Start()
    {
        _resourceMap = new Dictionary<PlayerInputs, Sprite>
        {
            {PlayerInputs.Left,  LeftSprite},
            {PlayerInputs.Right,  RightSprite},
            {PlayerInputs.Up,  UpSprite},
            {PlayerInputs.Down,  DownSprite},
            {PlayerInputs.CW,  CwSprite},
            {PlayerInputs.CounterCW,  CCwSprite},
            {PlayerInputs.ScaleUp,  ScaleUpSprite},
            {PlayerInputs.ScaleDown,  ScaleDownSprite},
        };
    }

    public void Init()
    {
        myInputs = new List<PlayerInputs>();
        _timer = TimeLimit;
        
        foreach (Transform t in InputIconsParent)
        {
            Destroy(t.gameObject);
        }

        ResetConfirms();
    }

    public void ResetConfirms()
    {
        confirm1 = false;
        confirm2 = false;
    }

    public InputTickResult Tick()
    {
        if (Input.GetButtonDown("P2XboxY")) Add(PlayerInputs.Up);
        if (Input.GetButtonDown("P2XboxX")) Add(PlayerInputs.Left);
        if (Input.GetButtonDown("P2XboxA")) Add(PlayerInputs.Down);
        if (Input.GetButtonDown("P2XboxB")) Add(PlayerInputs.Right);
        if (Input.GetButtonDown("P1XboxY")) Add(PlayerInputs.ScaleUp);
        if (Input.GetButtonDown("P1XboxA")) Add(PlayerInputs.ScaleDown);
        if (Input.GetButtonDown("P1XboxBumperLeft")) Add(PlayerInputs.CounterCW);
        if (Input.GetButtonDown("P1XboxBumperRight")) Add(PlayerInputs.CW);

        _timer -= Time.deltaTime;

        Slider.value = _timer / TimeLimit;

        if (_timer <= 0)
        {
            return InputTickResult.Timeout;
        }

        if (Input.GetButtonDown("P2XboxBumperLeft") || Input.GetButtonDown("P2XboxBumperRight"))
        {
            confirm1 = true;
        }

        if (Input.GetButtonDown("P1XboxX"))
        {
            confirm2 = true;
        }

        if (confirm1 && confirm2)
        {
            return InputTickResult.Confirm;
        }

        return InputTickResult.Pending;
    }

    private void Add(PlayerInputs inputType)
    {
        myInputs.Add(inputType);
        var go = Instantiate(InputIconPrefab, InputIconsParent);
        go.GetComponent<Image>().sprite = _resourceMap[inputType];
    }

    public void PaintAsDone(int doneStepIndex, Color paintColor)
    {
        Image lastImage = InputIconsParent.GetChild(doneStepIndex).GetComponent<Image>();
        StartCoroutine(PaintCoroutine(lastImage, paintColor));
    }

    private IEnumerator PaintCoroutine(Image image, Color targetColor)
    {
        const float duration = 0.25f;
        Color srcColor = image.color;
        for (float f = 0; f < duration; f += Time.deltaTime)
        {
            float t = PaintCurve.Evaluate(f / duration);

            image.color = Color.Lerp(srcColor, targetColor, t);

            yield return null;
        }

        image.color = targetColor;
    }

}
