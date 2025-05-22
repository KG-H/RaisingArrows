using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameStartBtn : MonoBehaviour
{
    public Image fadeImage; // fade 이미지
    public float fadeDuration = 2f; // 2초 동안 fade in/out
    public Slider slider;
    public TextMeshProUGUI LoadTxt;

    public void StartGame()
    {
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // Fade in: 2초 동안 알파값 0 -> 1
        yield return Fade(1);

        yield return new WaitForSeconds(3f);

        slider.gameObject.SetActive(true);
        LoadTxt.gameObject.SetActive(true);

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime; // 경과 시간 증가
            slider.value = Mathf.Lerp(0f, 1f, time / fadeDuration); // 0에서 1로 점차 증가
            yield return null; // 다음 프레임 대기
        }

        slider.gameObject.SetActive(false);
        LoadTxt.gameObject.SetActive(false);

        // 비동기적으로 메인 씬 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");
        while (!asyncLoad.isDone)
        {
            yield return null; // 씬 로드가 완료될 때까지 대기
        }

        // Fade out: 2초 동안 알파값 1 -> 0
        yield return Fade(0);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        fadeImage.gameObject.SetActive(true);
        float startAlpha = fadeImage.color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha); // 알파값만 변경
            yield return null; // 다음 프레임 대기
        }
    }
}
