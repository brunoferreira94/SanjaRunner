using UnityEngine;
using System.Collections;

public class SpriteSwitcher : MonoBehaviour
{

    public Sprite[] Sprites;
    public float SwitchTimeInterval;

    private SpriteRenderer _renderer;
    private float _lastSwapTime;
    private int _spriteCounter;

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.time > _lastSwapTime + SwitchTimeInterval)
        {
            _lastSwapTime = Time.time;
            _spriteCounter++;
            if (_spriteCounter >= Sprites.Length)
            {
                _spriteCounter = 0;
            }
            _renderer.sprite = Sprites[_spriteCounter];
        }
    }
}
