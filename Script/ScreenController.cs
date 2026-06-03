using System.Collections; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    [Header("Transition Settings")]
    [SerializeField] private Animator transitionAnimator; 
    [SerializeField] private float transitionTime = 1f;    

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

    private IEnumerator LoadLevelWithTransition(int levelIndex)
    {
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("End"); 
        }

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        
        while (!operation.isDone)
        {
            yield return null;
        }

        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Start");
        }
    }

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