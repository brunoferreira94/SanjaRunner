using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

/*

    This class represents a persistent helper object. It is used to fade in/out and change levels with the fade.

    Despite the fact that this object is not really a singleton, as it may be duplicated within Unity, 
    it does follow a singleton-like pattern: it has a static GetInstance() method which may be called
    anywhere to reach this object.

*/

public class PersistentBehaviour : MonoBehaviour
{

    public CanvasGroup FadeGroup;

    private bool _hasInitialized;   // This bool is used so that Awake()'s LoadLevel is only called once.
    private bool _blackScreen;
    private float _currentFadeSpeed;
    private static PersistentBehaviour _instance;

    public static PersistentBehaviour GetInstance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        if (!_hasInitialized)
        {
            _hasInitialized = true;
            Invoke("Initialize", 2);
        }
    }

    void Initialize()
    {
        Application.LoadLevel(1);
    }

    // This is automatically called by Unity. As soon as we load a level, we'll fade out the black screen.
    void OnLevelWasLoaded(int level)
    {
        if (level == 0) { return; }

        FadeOut(0.5f, 1);
    }

    public void FadeOut(float time, float delay)
    {
        FadeGroup.DOFade(0, time).SetDelay(delay);
    }

    public void FadeIn(float time, float delay)
    {
        FadeGroup.DOFade(1, time).SetDelay(delay);
    }

    public void ChangeLevel(int level, float fadeTime)
    {
        StartCoroutine(ChangeLevel_Coroutine(level, fadeTime));
    }

    public void ChangeLevel(string level, float fadeTime)
    {
        StartCoroutine(ChangeLevel_Coroutine(level, fadeTime));
    }

    IEnumerator ChangeLevel_Coroutine(int level, float fadeTime)
    {
        FadeIn(fadeTime, 0.0f);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(level);
    }

    IEnumerator ChangeLevel_Coroutine(string level, float fadeTime)
    {
        FadeIn(fadeTime, 0.0f);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(level);
    }
}
