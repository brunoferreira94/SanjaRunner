using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class WindArea : MonoBehaviour
{
    public float Intensity = 1f;

    private Rigidbody2D _playerRigidbody;

    private bool _canPush;
    private Transform _transform;

    void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.CompareTag("Player"))
        {
            if (!_playerRigidbody)
            {
                _playerRigidbody = PlayerMotor.GetInstance().Rigidbody;
            }
            _canPush = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _canPush = false;
        }
    }

    void FixedUpdate()
    {
        if (_canPush)
        {
            _playerRigidbody.AddForce(_transform.right * Intensity);
        }
    }
}