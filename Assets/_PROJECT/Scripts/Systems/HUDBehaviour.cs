﻿using System;
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
    public RectTransform NoteTransform;
    public TextMeshProUGUI NoteText;
    public CanvasGroup GameOverCanvasGroup;
    public TextMeshProUGUI GameOverCogsText;
    public TextMeshProUGUI GameOverTimeText;
    public TextMeshProUGUI TimeCounterGame;
    public TextMeshProUGUI TimeCounterQuiz;
    [Header("Settings")]
    public float HUDTweenTime = 1.5f;
    public float HUDTweenDelay = 1.25f;
    public Ease EaseMode;

    private float _initialCogTextSize;
    private float _initialCounterSize;
    private Vector3 _initialNotePos;
    private Tweener _timeFlashTween;

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
        _initialNotePos = NoteTransform.anchoredPosition3D;
        NoteTransform.anchoredPosition3D += new Vector3(0, 2000, 0);
        GameOverCanvasGroup.alpha = 0;
        GameOverCanvasGroup.blocksRaycasts = true;
        GameOverCanvasGroup.interactable = true;
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
                    LevelBehaviour.GetInstance().StartTimeCounter();
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

    public void ShowNote(string noteText)
    {
        NoteTransform.DOAnchorPos3D(_initialNotePos, 1.5f).SetUpdate(true);
        NoteText.text = noteText;
    }

    public void HideNote()
    {
        NoteTransform.anchoredPosition3D += Vector3.up * 2000;
        TimeBehaviour.SetTimeScale(1, 1);
    }

    public void SetTimeCounterValue(float value)
    {
        TimeCounterGame.text = value.ToString("0.00");
        TimeCounterQuiz.text = value.ToString("0.00");
    }

    public void FlashIngameTimer(Color c)
    {
        if (_timeFlashTween != null)
        {
            _timeFlashTween.Kill();
        }
        TimeCounterGame.color = c;
        _timeFlashTween = TimeCounterGame.DOColor(Color.white, 0.25f);
    }

    public void FlashQuizTimer(Color c)
    {
        if (_timeFlashTween != null)
        {
            _timeFlashTween.Kill();
        }
        TimeCounterQuiz.color = c;
        _timeFlashTween = TimeCounterQuiz.DOColor(Color.white, 0.25f);
    }

    public void Victory()
    {
        LevelBehaviour lb = LevelBehaviour.GetInstance();

        GameOverTimeText.text = lb.GetLevelTime().ToString("0.00");
        GameOverCogsText.text = lb.GetAcquiredCogs().ToString();

        GameOverCanvasGroup.DOFade(1, 0.25f).OnComplete(delegate
        {
            GameOverCanvasGroup.blocksRaycasts = true;
            GameOverCanvasGroup.interactable = true;
        });
    }

}
