using System.Collections;
using UnityEngine;

// UI 연출용 대기. WaitForSecondsRealtime과 달리 프레임의 unscaledDeltaTime을 누적하므로
// timeScale = 0인 일시정지 중에도 흐르면서, 에디터 일시정지/프레임 단위 녹화(captureFramerate)에서도 연출 타이밍이 일정하게 유지된다.
public static class UIWait
{
    public static IEnumerator Seconds_Coroutine(float seconds)
    {
        float elapsed = 0f;
        while (elapsed < seconds)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
