using UnityEngine;
using System.Collections;

public class SimpleRotator : MonoBehaviour
{
    public Vector3 RotationSum;

    private Transform _transform;

    void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 curRot = _transform.localRotation.eulerAngles;
        curRot += RotationSum * Time.deltaTime;
        _transform.localRotation = Quaternion.Euler(curRot);
    }

}
