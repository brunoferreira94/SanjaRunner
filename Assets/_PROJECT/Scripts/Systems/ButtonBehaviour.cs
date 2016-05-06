using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

public class ButtonBehaviour : MonoBehaviour
{

    public bool IsLocked = false;
    public SpriteRenderer ButtonSprite;
    public SpriteRenderer LockSprite;
    public TextMeshPro Text;

    private Collider2D _col;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
    }

    void Start()
    {
        gameObject.SetActive(false);
        ButtonSprite.color = new Color(0, 0, 0, 0);
        LockSprite.color = new Color(0, 0, 0, 0);
        Text.color = new Color(0, 0, 0, 0);
    }

    public void Show()
    {
        if (IsLocked)
        {
            LockSprite.enabled = true;
        }
        else
        {
            LockSprite.enabled = false;
        }
        ButtonSprite.DOColor(new Color(1, 1, 1, 1), 0.25f);
        LockSprite.DOFade(1, 0.25f);
        Text.DOFade(1, 0.25f);
        _col.enabled = true;
    }

    public void Hide()
    {
        _col.enabled = false;
        ButtonSprite.DOColor(new Color(1, 1, 1, 0), 0.25f);
        Text.DOFade(0, 0.25f);
        LockSprite.DOFade(0, 0.25f).OnComplete(delegate
        {
            gameObject.SetActive(false);
        });
    }
}
