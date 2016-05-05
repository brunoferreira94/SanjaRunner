using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [Header("Movement Settings")]
    public float Acceleration = 5f;
    public float MaxSpeed = 15;
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

    // Estamos no chão? Vamos projetar uma caixa invisível para baixo. Se ela acertar alguma coisa, estamos no chão.
    void GroundedCheck()
    {
        // BoxCast retorna TRUE se acertar algo, FALSE se não acertar nada.
        _isGrounded = Physics2D.BoxCast(_transform.position, GroundBoxCastSize, 0, Vector2.down, GroundBoxCastDistance,
            WorldMask);

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
        _rigidbody.AddForce(Vector2.right * Acceleration, ForceMode2D.Force);

        if (_rigidbody.velocity.x > MaxSpeed)
        {
            ClampHorizontalVelocity();
        }
    }

    // Travando a velocidade...
    void ClampHorizontalVelocity()
    {
        Vector2 velocity = _rigidbody.velocity;
        velocity.x = MaxSpeed;
        _rigidbody.velocity = velocity;
    }

    public void Jump()
    {
        if (_timesJumped < MidAirJumps)
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
    }
}
