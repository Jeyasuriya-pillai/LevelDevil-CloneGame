using System.Collections; // Required for Coroutines
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    [Header("Transition Settings")]
    [SerializeField] private Animator transitionAnimator; // Drag your FadeImage here
    [SerializeField] private float transitionTime = 1f;    // Time your animation takes

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        StartCoroutine(LoadLevelWithTransition(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadLevelWithTransition(sceneName));
    }

    // Coroutine to handle the level load delay for Index integers
    private IEnumerator LoadLevelWithTransition(int levelIndex)
    {
        // 1. Play the FadeIn animation (Screen becomes Black)
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("End"); // Standard trigger name based on your references
        }

        // 2. Wait for the animation to completely finish fading to black
        yield return new WaitForSeconds(transitionTime);

        // 3. Load the scene safely in the background
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        
        // Wait until the new scene completely finishes loading
        while (!operation.isDone)
        {
            yield return null;
        }

        // 4. Play the FadeOut animation (Screen becomes clear again in the new level)
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Start");
        }
    }

    // Coroutine wrapper overload to handle string scene names
    private IEnumerator LoadLevelWithTransition(string sceneName)
    {
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("End");
        }

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        while (!operation.isDone)
        {
            yield return null;
        }

        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Start");
        }
    }
}