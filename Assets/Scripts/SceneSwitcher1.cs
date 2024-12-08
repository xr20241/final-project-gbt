using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SceneSwitcher1 : MonoBehaviour
{
    public PlayableDirector director;
    public string nextSceneName = "Dimana Aku"; 

    private void Start()
    {
        // Ensure the PlayableDirector reference is set
        if (director == null)
        {
            director = GetComponent<PlayableDirector>();
        }

        // Register a callback for when the Timeline finishes
        if (director != null)
        {
            director.stopped += OnTimelineFinished;
        }
    }

    private void OnTimelineFinished(PlayableDirector obj)
    {
        // Load the next scene when the Timeline finishes
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnDestroy()
    {
        // Unregister the callback to prevent memory leaks
        if (director != null)
        {
            director.stopped -= OnTimelineFinished;
        }
    }
}
