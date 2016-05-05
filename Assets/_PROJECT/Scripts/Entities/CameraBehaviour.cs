using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform TargetTransform;
    public float LerpRate;

    private Transform _transform;

    void Awake()
    {
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
