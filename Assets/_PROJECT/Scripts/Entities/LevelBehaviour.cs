using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBehaviour : MonoBehaviour
{
    [Header("World Settings")]
    public string LevelName;
    [Header("World Objects")]
    public Transform StartPoint;
    public Transform EndPoint;
    [Header("Background Objects")]
    public Transform BGStartPoint;
    public Transform BGEndPoint;
    public Transform BGCameraTransform;
    [Header("Prefabs")]
    public PlayerMotor PlayerPrefab;

    public bool Debug = true;

    private List<CogBehaviour> _cogList;
    private Transform _playerTransform;
    private PlayerMotor _playerMotor;
    private float _levelProgress;
    private int _cogCounter;

    private static LevelBehaviour _instance;

    public static LevelBehaviour GetInstance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
        _cogList = new List<CogBehaviour>();
    }

    private void Start()
    {
        _playerMotor = (PlayerMotor)Instantiate(PlayerPrefab, StartPoint.position, Quaternion.identity);
        _playerTransform = _playerMotor.transform;
        CameraBehaviour.GetInstance().TargetTransform = _playerTransform;

        HUDBehaviour hud = HUDBehaviour.GetInstance();
        hud.SetLevelName(LevelName);

        // A contagem regressiva vai habilitar o movimento do personagem.
        hud.StartCountdown();
    }

    // Função usada pelas engrenagens: Elas chamam essa função para se adicionarem a lista de engrenagens.
    public void AddCog(CogBehaviour cog)
    {
        _cogList.Add(cog);
        _cogCounter = _cogList.Count;
        HUDBehaviour.GetInstance().SetCogAmount(_cogCounter);
    }

    public void RemoveCog(CogBehaviour cog)
    {
        _cogList.Remove(cog);
        _cogCounter = _cogList.Count;
        HUDBehaviour.GetInstance().SetCogAmount(_cogCounter);
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

    void Finish()
    {
        if (Debug)
        {
            _playerTransform.position = StartPoint.position;
        }
        else
        {
            _playerMotor.CanMove = false;
        }
    }

    public void RestartLevel()
    {
        PersistentBehaviour.GetInstance().ChangeLevel(SceneManager.GetActiveScene().name, 1f);
    }

    public void GoBackLevelSelection()
    {
        PersistentBehaviour.GetInstance().ChangeLevel(2, 1f);
    }

    void Update()
    {
        UpdateBackground();
        CalculateLevelProgress();
    }

}
