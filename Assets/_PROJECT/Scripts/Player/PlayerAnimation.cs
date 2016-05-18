using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Anim Objects")]
    public Animator Anim;

    private PlayerMotor _motor;
    private readonly int _isRunning = Animator.StringToHash("IsRunning");
    private readonly int _isGrounded = Animator.StringToHash("IsGrounded");
    private readonly int _yVelocity = Animator.StringToHash("YVelocity");
//    private readonly int _land = Animator.StringToHash("Land");

    void Awake()
    {
        _motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        Anim.SetBool(_isRunning, _motor.IsMoving());
        Anim.SetBool(_isGrounded, _motor.IsGrounded());
        Anim.SetFloat(_yVelocity, _motor.Rigidbody.velocity.y);
    }
}
