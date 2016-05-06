using UnityEngine;
using System.Collections;
using DG.Tweening;

public class LevelBehaviour : MonoBehaviour
{
    [Header("World Objects")]
    public Transform StartPoint;
    public Transform EndPoint;
    [Header("Background Objects")]
    public Transform BGStartPoint;
    public Transform BGEndPoint;
    public Transform BGCameraTransform;

    private Transform _playerTransform;
    private float _levelProgress;

    void Start()
    {
        _playerTransform = PlayerMotor.GetInstance().GetTransform();
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
        _levelProgress = 1 - (walkedDistance/distance);
    }

    void Update()
    {
        UpdateBackground();
        CalculateLevelProgress();
    }

}
