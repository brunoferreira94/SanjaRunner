using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBehaviour : MonoBehaviour
{
    public enum Answer
    {
        FIRST, SECOND, THIRD, FOURTH
    }

    [System.Serializable]
    public struct QuizData
    {
        public string Question;
        public string Answer1;
        public string Answer2;
        public string Answer3;
        public string Answer4;
        public Answer RealAnswer;
    }

    [Header("World Settings")]
    public string LevelName;
    public float TimeLimit;
    [Header("World Objects")]
    public Transform StartPoint;
    public Transform EndPoint;
    [Header("Background Objects")]
    public Transform BGStartPoint;
    public Transform BGEndPoint;
    public Transform BGCameraTransform;
    [Header("Prefabs")]
    public PlayerMotor PlayerPrefab;
    [Header("Endgame Canvas Groups")]
    public CanvasGroup FinishWinCanvasGroup;
    public CanvasGroup FinishFailCanvasGroup;
    public CanvasGroup CameraCullCanvasGroup;
    [Header("Quiz Objects")]
    public GameObject[] DisableOnQuiz;
    public GameObject[] EnableOnQuiz;
    public Toggle[] Togglers;
    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI QuizOption1;
    public TextMeshProUGUI QuizOption2;
    public TextMeshProUGUI QuizOption3;
    public TextMeshProUGUI QuizOption4;
    [Header("Quiz Options")]
    public float QuizWrongAnswerTimeSubtract;
    public QuizData[] Quizzes;

    [Header("Debug")]
    public bool DebugMode = true;

    private List<CogBehaviour> _cogList;
    private Transform _playerTransform;
    private PlayerMotor _playerMotor;
    private float _levelProgress;
    private int _cogCounter;
    private float _finishTime;
    private float _startingTime;
    private int _totalCogAmount = 0;
    private bool _hasFinished;
    private int _currentQuiz;
    private int _currentAnswer;
    private HUDBehaviour _hb;
    private float _winTime = 0;
    private CameraBehaviour _camBehaviour;

    public bool HasStarted { get; set; }

    private static LevelBehaviour _instance;

    public static LevelBehaviour GetInstance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
        _cogList = new List<CogBehaviour>();
        SetQuizObjects(false);
        HasStarted = false;
    }

    void Start()
    {
        _playerMotor = (PlayerMotor)Instantiate(PlayerPrefab, StartPoint.position, Quaternion.identity);
        _playerTransform = _playerMotor.transform;
        CameraBehaviour.GetInstance().TargetTransform = _playerTransform;

        _hb = HUDBehaviour.GetInstance();
        _hb.SetLevelName(LevelName);

        _camBehaviour = CameraBehaviour.GetInstance();

        // A contagem regressiva vai habilitar o movimento do personagem.
        _hb.StartCountdown();
    }

    public void StartTimeCounter()
    {
        _startingTime = Time.time;
        HasStarted = true;
    }

    public float GetLevelTime()
    {
        return _finishTime - _startingTime;
    }

    public float GetCurrentTime()
    {
        if (HasStarted)
        {
            if (_winTime == 0)
            {
                return Mathf.Clamp(TimeLimit - (Time.time - _startingTime), 0, 100);
            }
            else
            {
                return _winTime;
            }
        }
        else
        {
            return TimeLimit;
        }
    }

    // Função usada pelas engrenagens: Elas chamam essa função para se adicionarem a lista de engrenagens.
    public void AddCog(CogBehaviour cog)
    {
        _cogList.Add(cog);
        _cogCounter = _cogList.Count;
        HUDBehaviour.GetInstance().SetCogAmount(_cogCounter);
        _totalCogAmount++;
    }

    public void RemoveCog(CogBehaviour cog)
    {
        TimeLimit += cog.TimeSum;
        _cogList.Remove(cog);
        _cogCounter = _cogList.Count;
        _hb.FlashIngameTimer(Color.green);
        _hb.SetCogAmount(_cogCounter);
    }

    public int GetAcquiredCogs()
    {
        return _totalCogAmount - _cogCounter;
    }

    // Atualizamos a posição do fundo aqui.
    void UpdateBackground()
    {
        BGCameraTransform.position = Vector3.Lerp(BGStartPoint.position, BGEndPoint.position, _levelProgress);
    }

    // Calculamos o progresso do jogador na fase aqui, em _levelProgress, de 0 a 1.
    void CalculateLevelProgress()
    {
        if (!_playerTransform)
        {
            return;
        }
        float distance = EndPoint.position.x - StartPoint.position.x;
        float walkedDistance = EndPoint.position.x - _playerTransform.position.x;
        _levelProgress = 1 - (walkedDistance / distance);

        if (_levelProgress >= 1)
        {
            Finish();
        }
    }

    void UpdateHUD()
    {
        _hb.SetTimeCounterValue(GetCurrentTime());
    }

    void EvaluateTime()
    {
        if (GetCurrentTime() <= 0)
        {
            LoseGameTimer();
        }
    }

    void EvaluatePlayerPosition()
    {
        if (_camBehaviour.GetPlayerViewportPosition() < -0.05f && _playerMotor.CanMove)
        {
            LoseGameCameraCull();
        }
    }

    void Finish()
    {
        if (DebugMode)
        {
            _playerTransform.position = StartPoint.position;
        }
        else
        {
            if (!_hasFinished)
            {
                _hasFinished = true;
                _playerMotor.CanMove = false;
                _finishTime = Time.time;
                Debug.Log(GetLevelTime());
                //HUDBehaviour.GetInstance().Victory();
                DoQuiz();
            }
        }
    }

    public void RestartLevel()
    {
        PersistentBehaviour.GetInstance().ChangeLevel(Application.loadedLevelName, 1f);
    }

    public void GoBackLevelSelection()
    {
        PersistentBehaviour.GetInstance().ChangeLevel(2, 1f);
    }

    public void DoQuiz()
    {
        StartCoroutine(QuizRoutine());
    }

    IEnumerator QuizRoutine()
    {
        PersistentBehaviour pb = PersistentBehaviour.GetInstance();
        yield return null;
        pb.FadeIn(0.5f, 0);
        yield return new WaitForSeconds(0.5f);

        // The screen is now blank. Go nuts!
        SetNonQuizObjects(false);
        SetQuizObjects(true);
        UpdateQuizPanel();

        pb.FadeOut(0.5f, 0);
    }

    void UpdateQuizPanel()
    {
        QuestionText.text = Quizzes[_currentQuiz].Question;
        QuizOption1.text = Quizzes[_currentQuiz].Answer1;
        QuizOption2.text = Quizzes[_currentQuiz].Answer2;
        QuizOption3.text = Quizzes[_currentQuiz].Answer3;
        QuizOption4.text = Quizzes[_currentQuiz].Answer4;
    }

    void SetNonQuizObjects(bool b)
    {
        for (int counter = 0; counter < DisableOnQuiz.Length; counter++)
        {
            DisableOnQuiz[counter].SetActive(b);
        }
    }

    void SetQuizObjects(bool b)
    {
        for (int counter = 0; counter < EnableOnQuiz.Length; counter++)
        {
            EnableOnQuiz[counter].SetActive(b);
        }
    }

    public void SetAnswer(int value)
    {
        _currentAnswer = value;
    }

    public void QuizAccept()
    {
        if (_currentAnswer == (int)Quizzes[_currentQuiz].RealAnswer)
        {
            QuizSuccess();
        }
        else
        {
            QuizFail();
        }
    }

    void QuizSuccess()
    {
        _currentQuiz++;
        if (_currentQuiz >= Quizzes.Length)
        {
            WinGame();
        }
        else
        {
            UpdateQuizPanel();
        }
        Debug.Log("CERTO");
    }

    void QuizFail()
    {
        _currentQuiz++;
        TimeLimit -= QuizWrongAnswerTimeSubtract;
        if (_currentQuiz >= Quizzes.Length)
        {
            WinGame();
        }
        else
        {
            UpdateQuizPanel();
        }
        Debug.Log("ERRADO");
    }

    void WinGame()
    {
        _winTime = GetCurrentTime();
        FinishWinCanvasGroup.DOFade(1, 0.5f);
        Invoke("GoBackLevelSelection", 3);
    }

    void LoseGameTimer()
    {
        FinishFailCanvasGroup.DOFade(1, 0.5f);
        Invoke("GoBackLevelSelection", 3);
    }

    void LoseGameCameraCull()
    {
        _winTime = GetCurrentTime();
        CameraCullCanvasGroup.DOFade(1, 0.5f);
        PlayerMotor.GetInstance().CanMove = false;
        Invoke("GoBackLevelSelection", 3);
    }

    void Update()
    {
        UpdateBackground();
        CalculateLevelProgress();
        UpdateHUD();
        EvaluateTime();
        EvaluatePlayerPosition();
    }

}
