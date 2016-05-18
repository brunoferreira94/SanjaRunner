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
    public Rigidbody2D Rigidbody { private set; get; }

    private Transform _transform;
    private int _timesJumped;
    private bool _isGrounded;
    private bool _canSink;

    private Collision2D _ground;

    private static PlayerMotor _instance;

    public static PlayerMotor GetInstance()
    {
        return _instance;
    }

    private Vector2 MoveDirection
    {
        get
        {
            if (_ground != null)
            {
                return _ground.Tangent();
            }

            return Vector2.right;
        }
    }

    private

    // Chamado na inicialização.
    void Awake()
    {
        _instance = this;
        _transform = GetComponent<Transform>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.gravityScale = 0;
    }

    public Transform GetTransform()
    {
        return _transform;
    }

    public bool IsMoving()
    {
        return Rigidbody.velocity.x > 0.1f;
    }

    public bool IsGrounded()
    {
        return _isGrounded;
    }


    // Vamos aplicar uma força constante para a direita. Se ultrapassarmos o MaxSpeed, vamos travar a velocidade.
    void ApplyConstantForce()
    {
        if (CanMove)
        {
            Debug.Log(_ground != null);

            if (_ground != null)
            {
                Debug.DrawRay(transform.position, _ground.Tangent() * Acceleration);
                Debug.DrawRay(transform.position, _ground.Normal() * GravityForce);


            }

            Rigidbody.AddForce(MoveDirection * Acceleration, ForceMode2D.Force);

            if (Rigidbody.velocity.x > MaxSpeed)
            {
                ClampHorizontalVelocity();
            }
        }
        else
        {
            Vector3 curVel = Rigidbody.velocity;
            curVel.x = Mathf.Lerp(curVel.x, 0, 0.5f);
            Rigidbody.velocity = curVel;
        }
    }

    // Travando a velocidade...
    void ClampHorizontalVelocity()
    {
        Vector2 velocity = Rigidbody.velocity;
        velocity.x = MaxSpeed;
        Rigidbody.velocity = velocity;
    }

    // Aplicando força gravitacional contra superfície no pé, se ela existir
    void ApplyGravityAgainstSurface()
    {
        if (_ground != null)
        {
            Rigidbody.AddForce(-_ground.Normal() * GravityForce);
        }
        else
        {
            Rigidbody.AddForce(Vector2.down * GravityForce);
        }
    }

    public void Jump()
    {
        if (_timesJumped < MidAirJumps && CanMove)
        {
            _timesJumped++;
            Vector2 velocity = Rigidbody.velocity;
            velocity.y = JumpForce;
            Rigidbody.velocity = velocity;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.Normal().y > .1f)
        {
            _ground = null;
            _isGrounded = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.Normal().y > .1f)
        {
            _ground = collision;
            _isGrounded = true;

            _timesJumped = 0;
            _canSink = true;
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }

    public void Sink()
    {
        if (_canSink)
        {
            _canSink = false;
            Vector2 velocity = Rigidbody.velocity;
            velocity.y = -SinkForce;
            Rigidbody.velocity = velocity;
        }
    }

    void FixedUpdate()
    {
        ApplyConstantForce();
        ApplyGravityAgainstSurface();
    }
}
