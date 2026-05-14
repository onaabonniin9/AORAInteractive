using UnityEngine;
using TMPro;
using System.Collections;

public class InstructionTextUI : MonoBehaviour
{
    [Header("Configuración")]
    public float visibleTime = 3f;
    public float fadeDuration = 1.5f;

    private TextMeshProUGUI textUI;

    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();

        StartCoroutine(FadeOutText());
    }

    IEnumerator FadeOutText()
    {
        yield return new WaitForSeconds(visibleTime);

        float elapsedTime = 0f;

        Color originalColor = textUI.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            textUI.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );

            yield return null;
        }

        textUI.color = new Color(
            originalColor.r,
            originalColor.g,
            originalColor.b,
            0f
        );

        gameObject.SetActive(false);
    }
}