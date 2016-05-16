using UnityEngine;
using DG.Tweening;

public class CogBehaviour : MonoBehaviour
{

    private Transform _transform;
    private SimpleRotator _rotator;

    private static LevelBehaviour _lb;

    void Awake()
    {
        _transform = GetComponent<Transform>();
        _rotator = _transform.GetChild(0).GetComponent<SimpleRotator>();
    }

    void Start()
    {
        _lb = LevelBehaviour.GetInstance();
        _lb.AddCog(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _rotator.enabled = false;
            _lb.RemoveCog(this);
            _transform.DOLocalRotate(new Vector3(0, 0, 720), 0.2f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
            _transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear).OnComplete(delegate
            {
                Destroy(gameObject);
            });
        }
    }
}
