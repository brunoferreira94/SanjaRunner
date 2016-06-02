using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform TargetTransform;
    public float LerpRate;
    [Header("Runover Settings")]
    public bool UseRunover;
    public float StartingSpeed;

    private Transform _transform;
    private float _currentSpeed;
    private PlayerMotor _pm;
    private Camera _cam;

    private static CameraBehaviour _instance;

    public static CameraBehaviour GetInstance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
        _transform = GetComponent<Transform>();
        _currentSpeed = StartingSpeed;
        _cam = GetComponent<Camera>();
    }

    private void Start()
    {
        _pm = PlayerMotor.GetInstance();
    }

    public float GetPlayerViewportPosition()
    {
        return _cam.WorldToViewportPoint(TargetTransform.position).x;
    }


    void Chase()
    {
        Vector3 TargetPosition = new Vector3(TargetTransform.position.x, TargetTransform.position.y,
                _transform.position.z);
        _transform.position = Vector3.Lerp(_transform.position, TargetPosition, LerpRate);
    }

    void RunOver()
    {
        Vector3 targetPosition = new Vector3(_transform.position.x, TargetTransform.position.y, _transform.position.z);
        if (!_pm)
        {
            _pm = PlayerMotor.GetInstance();
        }
        if (_pm && _pm.CanMove)
        {
            targetPosition.x += _currentSpeed * Time.timeScale;
        }
        _transform.position = Vector3.Lerp(_transform.position, targetPosition, LerpRate);
    }

    void FixedUpdate()
    {
        if (TargetTransform)
        {
            if (UseRunover)
            {
                RunOver();
            }
            else
            {
                Chase();
            }
        }
    }
}
