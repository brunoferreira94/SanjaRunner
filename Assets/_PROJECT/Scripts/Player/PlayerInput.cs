using UnityEngine;

// Struct usado para passar o comando para o motor
public struct PlayerCommand
{
    public bool SwipeDown;
    public bool Touch;
}

// RequireComponent diz que um componente depende de outro. Criar um componente do tipo PlayerInput automaticamente cria um do tipo PlayerMotor.
[RequireComponent(typeof(PlayerMotor))]
public class PlayerInput : MonoBehaviour
{
    // Header adiciona uma "categoria" em negrito no Inspector do Unity.
    [Header("Input Settings")]
    public bool UseKeyboard;

    [Header("Keyboard-specific settings")]
    public string SwipeDownKey;
    public string TouchKey;

    private PlayerMotor _motor;
    private PlayerCommand _cmd;

    // Awake, chamado na inicialização da cena. É chamado antes de Start.
    // Start é chamado logo após o primeiro quadro do jogo.
    void Awake()
    {
        // _motor agora se refere ao componente do tipo PlayerMotor do mesmo GameObject.
        _motor = GetComponent<PlayerMotor>();
    }

    // Aqui coletamos o input pelo teclado.
    void DoKeyboardInput()
    {
        _cmd.SwipeDown = Input.GetKeyDown(SwipeDownKey);
        _cmd.Touch = Input.GetKeyDown(TouchKey);
    }

    // Aqui coletamos o input pelo touch.
    void DoTouchInput()
    {
        
    }

    // Estamos passando o nosso input para o motor.
    void SendInput()
    {
        if (_cmd.Touch)
        {
            _motor.Jump();
        }

        else if (_cmd.SwipeDown)
        {
            _motor.Sink();
        }
    }

    // Update é chamado sempre que possível.
    void Update()
    {
        _cmd = new PlayerCommand();

        if (UseKeyboard)
        {
            DoKeyboardInput();
        }
        else
        {
            DoTouchInput();
        }

        SendInput();
    }
}
