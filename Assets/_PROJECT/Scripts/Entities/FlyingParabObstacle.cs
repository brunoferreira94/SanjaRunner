using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FlyingParabObstacle : MonoBehaviour
{

    [Header("Setup")]
    public Transform SkinTransform;
    public Collider2D ObjectCollier;
    [Header("Settings")]
    public Vector2 KnockbackVector; // Vetor de força adicionado no jogador ao acertá-lo
    [Header("Parabola Settings")]
    public float InverseGravity;
    public Vector2 TriggerVelocity;

    private Rigidbody2D _rb;
    private bool _triggered = false;
    private Vector3 _startingPos;
    private Vector3 _lastPos;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _lastPos = _transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tag.Player) && !_triggered)
        {
            _triggered = true;
            _rb.gravityScale = InverseGravity;
            _rb.AddForce(TriggerVelocity, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.MatchTag(Tag.Player))
        {
            SkinTransform.DOLocalRotate(new Vector3(0, 0, 75), 0.3f, RotateMode.Fast).SetEase(Ease.OutCirc);
            ObjectCollier.enabled = false;
            _rb.gravityScale = 4f;
            PlayerMotor.GetInstance().Rigidbody.AddForce(KnockbackVector, ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        if (_transform.position != _lastPos)
        {
            Vector2 dir = _transform.position - _lastPos;

            dir = dir.normalized;

            SkinTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(50, -50, dir.y)));
            _lastPos = _transform.position;
            Invoke("Die", 5);
        }
    }
}
