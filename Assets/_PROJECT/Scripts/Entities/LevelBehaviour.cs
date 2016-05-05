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
    private float _levelProgress; // Precisa ser travado entre 0 e 1.

    void Start()
    {
        _playerTransform = PlayerMotor.GetInstance().GetTransform();
    }

    void UpdateBackground()
    {
        BGCameraTransform.position = Vector3.Lerp(BGStartPoint.position, BGEndPoint.position, _levelProgress);
    }

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
