using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class HUDBehaviour : MonoBehaviour
{
    [Header("Objects")]
    public TextMeshProUGUI LevelNameText;
    public Image CogDisplayBG;
    public Image CogDisplayCog;
    public TextMeshProUGUI CogsText;
    public TextMeshProUGUI CounterText;
    public TextMeshProUGUI GameOverText;
    public RectTransform GameOverPanel;
    [Header("Settings")]
    public float HUDTweenTime = 1.5f;
    public float HUDTweenDelay = 1.25f;
    public Ease EaseMode;

    private float _initialCogTextSize;
    private float _initialCounterSize;
    private float _initialGameOverTextSize;
    private Vector3 _initialGameOverPanelPosition;
    private LevelBehaviour _lb;

    private static HUDBehaviour _instance;

    public static HUDBehaviour GetInstance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
        CogDisplayBG.fillAmount = 0;
        CogDisplayCog.fillAmount = 0;
        CogsText.text = "0";
        _initialCogTextSize = CogsText.fontSize;
        CogsText.fontSize = 0;
        _initialCounterSize = CounterText.fontSize;
        CounterText.fontSize = 0;
        _initialGameOverPanelPosition = GameOverPanel.anchoredPosition3D;
        GameOverPanel.anchoredPosition3D += new Vector3(0, 2000, 0);
        _initialGameOverTextSize = GameOverText.fontSize;
        GameOverText.fontSize = 0;
    }

    void Start()
    {
        CogDisplayBG.DOFillAmount(1, HUDTweenTime).SetDelay(HUDTweenDelay).SetEase(EaseMode);
        CogDisplayCog.DOFillAmount(1, HUDTweenTime).SetDelay(HUDTweenDelay).SetEase(EaseMode);
        CogsText.DOFontSize(_initialCogTextSize, HUDTweenTime / 2).SetDelay(HUDTweenDelay + HUDTweenTime).SetEase(EaseMode);
    }

    public void StartCountdown()
    {
        CounterText.text = "3";
        CounterText.DOFontSize(_initialCounterSize, 1).SetEase(EaseMode).SetDelay(HUDTweenDelay).OnComplete(delegate
        {
            CounterText.fontSize = 0;
            CounterText.text = "2";
            CounterText.DOFontSize(_initialCounterSize, 1).SetEase(EaseMode).OnComplete(delegate
            {
                CounterText.fontSize = 0;
                CounterText.text = "1";
                CounterText.DOFontSize(_initialCounterSize, 1).SetEase(EaseMode).OnComplete(delegate
                {
                    CounterText.fontSize = 0;
                    PlayerMotor.GetInstance().CanMove = true;
                });
            });
        });
    }

    public void SetLevelName(string name)
    {
        LevelNameText.text = name;
    }

    public void SetCogAmount(int amount)
    {
        CogsText.text = amount.ToString();
    }

    public float GetGameOverTextSpacing()
    {
        return GameOverText.characterSpacing;
    }

    public void SetGameOverTextSpacing(float value)
    {
        GameOverText.characterSpacing = value;
    }

    public void Victory()
    {
        
    }

}
