using HedgehogTeam.EasyTouch;
using UnityEngine;

// RequireComponent diz que um componente depende de outro. Criar um componente do tipo PlayerInput automaticamente cria um do tipo PlayerMotor.
[RequireComponent(typeof(PlayerMotor))]
public class PlayerInput : MonoBehaviour
{
    private PlayerMotor _motor;
    private bool _canSwipeUp;

    // Awake, chamado na inicialização da cena. É chamado antes de Start.
    // Start é chamado logo após o primeiro quadro do jogo.
    void Awake()
    {
        // _motor agora se refere ao componente do tipo PlayerMotor do mesmo GameObject.
        _motor = GetComponent<PlayerMotor>();

    }

    void OnEnable()
    {
        EasyTouch.On_Swipe += OnSwipe;
        EasyTouch.On_TouchUp += OnTouchUp;
    }

    void OnDisable()
    {
        EasyTouch.On_Swipe -= OnSwipe;
        EasyTouch.On_TouchUp -= OnTouchUp;
    }

    void OnDestroy()
    {
        EasyTouch.On_Swipe -= OnSwipe;
        EasyTouch.On_TouchUp -= OnTouchUp;
    }

    public void OnTouchUp(Gesture gesture)
    {
        _canSwipeUp = true;
    }

    public void OnSwipe(Gesture gesture)
    {
        Vector2 swipeDirection = gesture.swipeVector;
        
        if (Mathf.Abs(swipeDirection.y) > Mathf.Abs(swipeDirection.x))
        {
            if (swipeDirection.y > 0 && _canSwipeUp)
            {
                _motor.Jump();
                _canSwipeUp = false;
            }

            if (swipeDirection.y < 0)
            {
                _motor.Sink();
            }
        }
    }
}
