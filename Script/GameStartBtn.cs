using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameStartBtn : MonoBehaviour
{
    public Image fadeImage; // fade �̹���
    public float fadeDuration = 2f; // 2�� ���� fade in/out
    public Slider slider;
    public TextMeshProUGUI LoadTxt;

    public void StartGame()
    {
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // Fade in: 2�� ���� ���İ� 0 -> 1
        yield return Fade(1);

        yield return new WaitForSeconds(3f);

        slider.gameObject.SetActive(true);
        LoadTxt.gameObject.SetActive(true);

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime; // ��� �ð� ����
            slider.value = Mathf.Lerp(0f, 1f, time / fadeDuration); // 0���� 1�� ���� ����
            yield return null; // ���� ������ ���
        }

        slider.gameObject.SetActive(false);
        LoadTxt.gameObject.SetActive(false);

        // �񵿱������� ���� �� �ε�
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");
        while (!asyncLoad.isDone)
        {
            yield return null; // �� �ε尡 �Ϸ�� ������ ���
        }

        // Fade out: 2�� ���� ���İ� 1 -> 0
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
            fadeImage.color = new Color(0, 0, 0, alpha); // ���İ��� ����
            yield return null; // ���� ������ ���
        }
    }
}
