using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PaperBehaviour : MonoBehaviour
{

    public string Note;

    private Transform _transform;
    private LevelBehaviour _lb;

    void Awake()
    {
        _transform = GetComponent<Transform>();
        _lb = LevelBehaviour.GetInstance();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _lb.PickupNote();
            _transform.DOLocalRotate(new Vector3(0, 0, 720), 0.2f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
            _transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear).OnComplete(delegate
            {
                Destroy(gameObject);
            });
        }
    }

}
