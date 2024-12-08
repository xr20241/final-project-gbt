using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cognitive3D;

public class SceneSwitcher : MonoBehaviour
{
    
    public static SceneSwitcher Instance { get; private set; }
    [SerializeField] private bool isLinear;
    
    [SerializeField] private string goodRoute;
    [SerializeField] private string badRoute;

    [SerializeField] private float changeDelay = 3f;
    [SerializeField] private float initialDelay = 6f;

    [SerializeField] public Image fadeImage; // The image that covers the screen, black in color
    [SerializeField] public Image initialPanel;
    [SerializeField] public GameObject initialCanvas;
    public TextMeshProUGUI initialBriefTxt;
    public float fadeDuration = 1f; // Duration of the fade in seconds


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.canvasRenderer.SetAlpha(0);
        }

        if (initialPanel != null && initialBriefTxt != null)
        {
            initialPanel.canvasRenderer.SetAlpha(1); // Set initial alpha to 1 (opaque)
            initialBriefTxt.alpha = 1;

            StartCoroutine(FadeOutAfterDelay());  // Start coroutine to fade out after delay
        }
    }

    private IEnumerator FadeOutAfterDelay()
    {
        // Wait for the initial delay before starting fade out
        yield return new WaitForSeconds(initialDelay);

        initialPanel.CrossFadeAlpha(0f, fadeDuration, false);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            if (initialBriefTxt != null)
            {
                initialBriefTxt.alpha = alpha;
            }
            yield return null;
        }

        initialCanvas.SetActive(false);
    }

    public void SwitchSceneEntry() {
        if (isLinear) SwitchScene(goodRoute);

        else {
            SwitchScene((ScoreManager.Instance.heart >= 3) ? goodRoute : badRoute);
        }
    }

    public void SwitchScene(string sceneName)
    {
        StartCoroutine(FadeToBlackAndLoad(sceneName));
    }

    private IEnumerator FadeToBlackAndLoad(string sceneName)
    {
        yield return new WaitForSeconds(changeDelay);

        FadeTo(1f);

        // Wait until the fade-in is complete
        yield return new WaitForSeconds(fadeDuration);



        if (isLinear){ 
            new Cognitive3D.CustomEvent("Change scene")
                .SetProperty("target scene", goodRoute)
                .Send();

            SceneManager.LoadScene(goodRoute, LoadSceneMode.Single);
        }

        else
        {
            Debug.Log("masuk target scene: " + ((ScoreManager.Instance.heart >= 3)? goodRoute : badRoute));

            new Cognitive3D.CustomEvent("Change scene")
                .SetProperty("target scene", (ScoreManager.Instance.heart >= 3)? goodRoute : badRoute)
                .Send();
            SceneManager.LoadScene((ScoreManager.Instance.heart >= 3) ? goodRoute : badRoute, LoadSceneMode.Single);
        }

        // Wait for a frame before fading out (optional, allows the new scene to load)
        yield return null;

        // Fade back to transparent
        FadeTo(0f);

        // Wait until the fade-out is complete
        yield return new WaitForSeconds(fadeDuration);
    }

    private void FadeTo(float targetAlpha)
    {
        fadeImage.CrossFadeAlpha(targetAlpha, fadeDuration, false);
    }
}
