using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Cognitive3D;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int heart=5;
    public float fadeDuration = 1f;
    [SerializeField] private List<Image> heartsImg;

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

        heart = PlayerPrefs.GetInt("Score", 5);
        SetHeartUI();
    }

    private void SetHeartUI(){
        for(int i = 5; i > heart; i--){
            Color color = heartsImg[i-1].color;
            heartsImg[i-1].color = new Color(color.r, color.g, color.b, 0f);
        }
    }

    public void InputScoreHandler(string input){

        switch (input)
        {
            case "good":
                IncrementHeart();
                break;
            case "Good":
                IncrementHeart();
                break;
            case "bad":
                DecrementHeart();
                break;
            case "Bad":
                DecrementHeart();
                break;
            default:
                break;
        }
    }

    private void IncrementHeart() {
        if (heart+1 > 5){
            new Cognitive3D.CustomEvent("Increment score but maximum")
                .SetProperty("score", heart)
                .Send();

            return;
        }
        heart += 1;

        PlayerPrefs.SetInt("Score", heart);

        new Cognitive3D.CustomEvent("Increment score")
            .SetProperty("score", heart)
            .Send();

        StartCoroutine(FadeIn(heart-1));
    }

    private void DecrementHeart() {
        if (heart-1< 0){
            new Cognitive3D.CustomEvent("Decrement score but minimum")
                .SetProperty("score", heart)
                .Send();

            return;
        }

        int prevIdx = heart;
        heart -= 1;

        PlayerPrefs.SetInt("Score", heart);

        new Cognitive3D.CustomEvent("Decrement score")
            .SetProperty("score", heart)
            .Send();

        StartCoroutine(FadeOut(prevIdx-1));
    }

    public IEnumerator FadeOut(int index)
    {
        Color color = heartsImg[index].color;
        float startAlpha = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            heartsImg[index].color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null; // Wait for the next frame
        }

        // Ensure the alpha is set to 0 at the end
        heartsImg[index].color = new Color(color.r, color.g, color.b, 0f);
    }

    // Function to fade the Image alpha to 1 (255 in byte scale)
    public IEnumerator FadeIn(int index)
    {
        Color color = heartsImg[index].color;
        float startAlpha = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / fadeDuration);
            heartsImg[index].color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null; // Wait for the next frame
        }

        // Ensure the alpha is set to 1 at the end
        heartsImg[index].color = new Color(color.r, color.g, color.b, 1f);
    }
}
