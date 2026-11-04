using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Loading : MonoBehaviour
{
    // allowSceneActivation이 false면 progress는 0.9에서 멈추므로 이 값을 로딩 완료로 본다.
    private const float LoadCompleteProgress = 0.9f;

    [SerializeField] private Slider _progressSlider;
    [SerializeField] private TextMeshProUGUI _progressText;

    private void Start()
    {
        RefreshProgress(0f);
        StartCoroutine(Load_Coroutine());
    }

    private IEnumerator Load_Coroutine()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        // Async Scene Load
        // return : Object that can check async progress of Scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneLoader.NextSceneName);
        //ready for create each objects one by one
        operation.allowSceneActivation = false;

        float value;
        while (!operation.isDone)
        {
            // interpolation in 0~1
            value = Mathf.Clamp01(operation.progress / LoadCompleteProgress);
            RefreshProgress(value);

            Debug.Log(Time.frameCount);
            Debug.Log(operation.progress);
            if (operation.progress >= LoadCompleteProgress)
            {
                operation.allowSceneActivation = true;
                yield break;
            }

            yield return null;
        }
    }

    private void RefreshProgress(float value)
    {
        _progressSlider.value = value;
        _progressText.text = $"Load Progress...{Mathf.RoundToInt(value * 100f)}%";
    }
}