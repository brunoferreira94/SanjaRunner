using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [Header("Movement Settings")]
    public float Acceleration = 5f;
    public float MaxSpeed = 15;
    public float GravityForce = 15f;
    public bool CanMove = false;
    [Header("Jump Settings")]
    public float JumpForce = 10f;
    public int MidAirJumps = 1;
    [Header("Sink Settings")]
    public float SinkForce;

    [Header("Grounded Checks")]
    public LayerMask WorldMask;
    public Vector2 GroundBoxCastSize;
    public float GroundBoxCastDistance;

    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private int _timesJumped;
    private bool _isGrounded;
    private bool _canSink;
    private RaycastHit2D _rhit;

    private static PlayerMotor _instance;

    public static PlayerMotor GetInstance()
    {
        return _instance;
    }

    // Chamado na inicialização.
    void Awake()
    {
        _instance = this;
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public Transform GetTransform()
    {
        return _transform;
    }

    public bool IsMoving()
    {
        return _rigidbody.velocity.x > 0.1f;
    }

    public bool IsGrounded()
    {
        return _isGrounded;
    }

    // Estamos no chão? Vamos projetar uma caixa invisível para baixo. Se ela acertar alguma coisa, estamos no chão.
    void GroundedCheck()
    {
        // BoxCast retorna RaycastHit2D.
        _rhit = Physics2D.BoxCast(_transform.position, GroundBoxCastSize, 0, Vector2.down, GroundBoxCastDistance,
            WorldMask);

        // Se BoxCast retornou algo, _isGrounded será TRUE. Caso contrário, será FALSE.
        _isGrounded = _rhit;

        // Se encostamos no chão, vamos resetar o contador de saltos.
        if (_isGrounded)
        {
            _timesJumped = 0;
            _canSink = true;
        }
    }

    // Vamos aplicar uma força constante para a direita. Se ultrapassarmos o MaxSpeed, vamos travar a velocidade.
    void ApplyConstantForce()
    {
        if (CanMove)
        {
            _rigidbody.AddForce(Vector2.right * Acceleration, ForceMode2D.Force);

            if (_rigidbody.velocity.x > MaxSpeed)
            {
                ClampHorizontalVelocity();
            }
        }
    }

    // Travando a velocidade...
    void ClampHorizontalVelocity()
    {
        Vector2 velocity = _rigidbody.velocity;
        velocity.x = MaxSpeed;
        _rigidbody.velocity = velocity;
    }

    // Aplicando força gravitacional contra superfície no pé, se ela existir
    void ApplyGravityAgainstSurface()
    {
        if (_rhit)
        {
            _rigidbody.AddForce(-_rhit.normal * GravityForce);
        }
        else
        {
            _rigidbody.AddForce(Vector2.down * GravityForce);
        }
    }

    public void Jump()
    {
        if (_timesJumped < MidAirJumps && CanMove)
        {
            _timesJumped++;
            Vector2 velocity = _rigidbody.velocity;
            velocity.y = JumpForce;
            _rigidbody.velocity = velocity;
        }
    }

    public void Sink()
    {
        if (_canSink)
        {
            _canSink = false;
            Vector2 velocity = _rigidbody.velocity;
            velocity.y = -SinkForce;
            _rigidbody.velocity = velocity;
        }
    }

    void FixedUpdate()
    {
        GroundedCheck();
        ApplyConstantForce();
        ApplyGravityAgainstSurface();

        // Não vamos utilizar a gravidade do Rigidbody.
        _rigidbody.gravityScale = 0;
    }
}
