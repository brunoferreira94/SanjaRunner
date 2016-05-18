using UnityEngine;
using DG.Tweening;

public class TimeBehaviour : MonoBehaviour
{

    public static void SetTimeScale(float timeScale, float duration)
    {
        DOTween.To(GetScale, SetScale, timeScale, duration).SetUpdate(true);
    }

    private static void SetScale(float value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = 0.02f * value;
    }

    private static float GetScale()
    {
        return Time.timeScale;
    }

}
