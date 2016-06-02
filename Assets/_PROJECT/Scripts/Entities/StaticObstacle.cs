using UnityEngine;
using System.Collections;

public class StaticObstacle : MonoBehaviour
{
    [Header("On Hit Settings")]
    public Vector2 KnockbackVector; // Vetor de força adicionado no jogador ao acertá-lo

    [Header("Destruction sequence settings")]
    public bool BreakOnTouch = true; // Objeto pode ser destruído ao ser esbarrado?
    public Vector2 BreakKnockbackVector; // Vetor de força adicionado ao próprio objeto ao ser acertado
    public float BreakTorque; // Torque adicionado ao próprio objeto ao ser acertado

    private PolygonCollider2D _collider;
    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<PolygonCollider2D>();
        _collider.enabled = true;
        _rb.isKinematic = true;

        if (!BreakOnTouch)
        {
            gameObject.tag = Tag.World;
            Destroy(_rb);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.MatchTag(Tag.Player) && BreakOnTouch)
        {
            PlayerMotor.GetInstance().Rigidbody.AddForce(KnockbackVector, ForceMode2D.Impulse);
            _collider.enabled = false;
            _rb.isKinematic = false;
            _rb.AddForce(BreakKnockbackVector, ForceMode2D.Impulse);
            _rb.AddTorque(BreakTorque, ForceMode2D.Impulse);
            Invoke("Die", 5);
        }
    }

}
