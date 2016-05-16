using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform TargetTransform;
    public float LerpRate;

    private Transform _transform;
    private static CameraBehaviour _instance;

    public static CameraBehaviour GetInstance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
        _transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (TargetTransform)
        {
            Vector3 TargetPosition = new Vector3(TargetTransform.position.x, TargetTransform.position.y,
                _transform.position.z);
            _transform.position = Vector3.Lerp(_transform.position, TargetPosition, LerpRate);
        }
    }
}
