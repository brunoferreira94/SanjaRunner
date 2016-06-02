using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FlyingStraightObstacle : MonoBehaviour
{
    [Header("Setup")]
    public Transform SkinTransform;
    public Collider2D ObjectCollier;
    [Header("Settings")]
    public float Speed;
    public Vector2 KnockbackVector; // Vetor de força adicionado no jogador ao acertá-lo

    private Rigidbody2D _rb;
    private bool _triggered = false;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tag.Player) && !_triggered)
        {
            _triggered = true;
            _rb.AddForce(Vector2.left * Speed, ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.MatchTag(Tag.Player))
        {
            SkinTransform.DOLocalRotate(new Vector3(0, 0, 75), 0.3f, RotateMode.Fast).SetEase(Ease.OutCirc);
            ObjectCollier.enabled = false;
            _rb.gravityScale = 4f;
            PlayerMotor.GetInstance().Rigidbody.AddForce(KnockbackVector, ForceMode2D.Impulse);
            Invoke("Die", 5);
        }
    }

}
