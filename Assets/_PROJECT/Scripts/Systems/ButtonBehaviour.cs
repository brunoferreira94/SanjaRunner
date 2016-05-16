using UnityEngine;
using DG.Tweening;
using TMPro;

public class ButtonBehaviour : MonoBehaviour
{

    public bool IsLocked = false;
    public SpriteRenderer ButtonSprite;
    public SpriteRenderer LockSprite;
    public TextMeshPro Text;
    [Header("Settings")]
    public string LevelName;

    void Start()
    {
        gameObject.SetActive(false);
        ButtonSprite.color = new Color(0, 0, 0, 0);
        LockSprite.color = new Color(0, 0, 0, 0);
        Text.color = new Color(0, 0, 0, 0);
    }

    public void UpdateStatus()
    {
        LockSprite.enabled = IsLocked;

    }

    public void Show()
    {
        UpdateStatus();
        ButtonSprite.DOColor(new Color(1, 1, 1, 1), 0.25f);
        LockSprite.DOFade(1, 0.25f);
        Text.DOColor(new Color(1, 1, 1, 1), 0.25f);
    }

    public void Hide()
    {
        ButtonSprite.DOColor(new Color(1, 1, 1, 0), 0.25f);
        Text.DOColor(new Color(1, 1, 1, 0), 0.25f);
        LockSprite.DOFade(0, 0.25f).OnComplete(delegate
        {
            gameObject.SetActive(false);
        });
    }

    void OnMouseDown()
    {
        if (!IsLocked)
        {
            PersistentBehaviour.GetInstance().ChangeLevel(LevelName, 0.25f);
        }
    }
}
