using System.Collections;
using UnityEngine;

public class SceneManagerScript : MonoBehaviour
{
    // instance
    public static SceneManagerScript instance;

    private CanvasGroup fadeCanvasGroup;
    public const float FADE_DURATION = SceneFlag.FADE_DURATION;
    public const float FADE_INCREMENT = 0.04f;
    public const float ANIMATION_SPEED = 2f;
    public bool hasFadingEnded;

    private void Awake()
    {
        // instance
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }   

        DontDestroyOnLoad(gameObject);

        // components
        fadeCanvasGroup = GameObject.Find("FadeCanvas").GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut()
    {
        hasFadingEnded = false;
        fadeCanvasGroup.alpha = 0f;
        float fadeIncrement = FADE_INCREMENT;

        do
        {
            fadeCanvasGroup.alpha += fadeIncrement;
            yield return new WaitForSeconds(fadeIncrement);
        } while (fadeCanvasGroup.alpha != 1f);

        hasFadingEnded = true;
    }

    public IEnumerator FadeIn()
    {
        hasFadingEnded = false;
        fadeCanvasGroup.alpha = 1f;
        float fadeIncrement = FADE_INCREMENT;

        do
        {
            fadeCanvasGroup.alpha -= fadeIncrement;
            yield return new WaitForSeconds(fadeIncrement);
        } while (fadeCanvasGroup.alpha != 0f);

        hasFadingEnded = true;
    }
}
