using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Anim Objects")]
    public Animator Anim;

    private PlayerMotor _motor;

    void Awake()
    {
        _motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        Anim.SetBool("IsRunning", _motor.IsMoving());
        Anim.SetBool("IsGrounded", _motor.IsGrounded());
    }
}
