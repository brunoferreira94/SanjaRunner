using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PaperBehaviour : MonoBehaviour
{

    public string Note;
    public float TimeSum;

    private Transform _transform;

    void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _transform.DOLocalRotate(new Vector3(0, 0, 720), 0.2f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
            _transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear).OnComplete(delegate
            {
                Destroy(gameObject);
                TimeBehaviour.SetTimeScale(0, 1f);
                HUDBehaviour.GetInstance().ShowNote(Note);
                LevelBehaviour.GetInstance().TimeLimit += TimeSum;
            });
        }
    }

}
