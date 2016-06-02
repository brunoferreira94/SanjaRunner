using UnityEngine;
using System.Collections;

public class RollingObstacle : MonoBehaviour
{
    public Vector3 TriggerVelocityVector; // Velocidade adicionada ao objeto quando o jogador entra na região gatilho
    public float TriggerTorque; // Torque adicionado ao objeto quando o jogador entra na região gatilho
    [Header("On Hit Settings")]
    public Vector2 KnockbackVector;
    [Header("Destruction sequence settings")]
    public Vector2 BreakKnockbackVector; // Vetor de força adicionado ao próprio objeto ao ser acertado
    public float BreakTorque; // Torque adicionado ao próprio objeto ao ser acertado

    private Rigidbody2D _rb;
    private bool _triggered = false;
    private Collider2D[] _colliders;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
        _colliders = GetComponents<Collider2D>();
    }

    void DisableAllColliders()
    {
        for (int counter = 0; counter < _colliders.Length; counter++)
        {
            _colliders[counter].enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tag.Player) && !_triggered)
        {
            _triggered = true;
            _rb.isKinematic = false;
            _rb.AddForce(TriggerVelocityVector, ForceMode2D.Impulse);
            _rb.AddTorque(TriggerTorque);
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
            PlayerMotor.GetInstance().Rigidbody.AddForce(KnockbackVector, ForceMode2D.Impulse);
            DisableAllColliders();
            _rb.isKinematic = false;
            _rb.AddForce(BreakKnockbackVector, ForceMode2D.Impulse);
            _rb.AddTorque(BreakTorque, ForceMode2D.Impulse);
            Invoke("Die", 5);
        }
    }



}
