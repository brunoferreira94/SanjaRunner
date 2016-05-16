using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TweenerBehaviourUI : MonoBehaviour
{

    public Vector3 StartingOffset;
    public float TweenTime;
    public float TweenDelay;
    public Ease EaseMode;

    private RectTransform _transform;
    private Vector3 _startingPos;

    void Awake()
    {
        _transform = GetComponent<RectTransform>();
        _startingPos = _transform.anchoredPosition3D;

        _transform.anchoredPosition3D += StartingOffset;
    }

    void Start()
    {
        _transform.DOAnchorPos3D(_startingPos, TweenTime).SetEase(EaseMode).SetDelay(TweenDelay);
    }



}
