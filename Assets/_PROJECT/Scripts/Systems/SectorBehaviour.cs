using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SectorBehaviour : MonoBehaviour
{
    public bool IsLocked;
    public Vector3 GrowScale;
    public ButtonBehaviour[] Buttons;
    public Color LockedColor;
    public Color UnlockedColor;

    private Transform _transform;
    //private PersistentBehaviour _pb;
    private bool _canTrigger = true;
    private Vector3 _startingPosition;
    private Vector3 _startingScale;
    private MapBehaviour _mb;
    private SpriteRenderer _sprite;

    void Awake()
    {
        _transform = GetComponent<Transform>();
        _startingPosition = _transform.localPosition;
        _startingScale = _transform.localScale;
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _mb = MapBehaviour.GetInstance();
        //_pb = PersistentBehaviour.GetInstance();
    }

    public void UpdateLockedStatus()
    {
        if (IsLocked)
        {
            _sprite.color = LockedColor;
        }
        else
        {
            _sprite.color = UnlockedColor;
        }
    }

    public void UnlockLevels(int levels)
    {
        for (int counter = 0; counter < Buttons.Length; counter++)
        {
            ButtonBehaviour cur = Buttons[counter];
            cur.IsLocked = !(counter <= levels);
            cur.UpdateStatus();
        }
    }

    public void Shrink()
    {
        _transform.DOScale(Vector3.zero, 0.25f).OnComplete(delegate
        {
            gameObject.SetActive(false);
        });
    }

    public void ShowButtons()
    {
        for (int counter = 0; counter < Buttons.Length; counter++)
        {
            Buttons[counter].gameObject.SetActive(true);
            Buttons[counter].Show();
        }
    }

    public void HideButtons()
    {
        for (int counter = 0; counter < Buttons.Length; counter++)
        {
            Buttons[counter].Hide();
        }
    }

    public void ResetSector()
    {
        _transform.DOScale(_startingScale, 0.25f);
        _transform.DOLocalMove(_startingPosition, 0.25f);
        HideButtons();
        _canTrigger = true;
    }

    void OnMouseDown()
    {
        if (_canTrigger && !IsLocked)
        {
            _canTrigger = false;
            _transform.DOScale(GrowScale, 0.5f);
            _transform.DOLocalMove(Vector3.zero, 0.5f);
            ShowButtons();
            _mb.DisableOtherSectors(this);
        }
    }
}
