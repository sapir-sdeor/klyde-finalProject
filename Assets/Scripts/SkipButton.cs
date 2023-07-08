using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    [SerializeField] private float timePast;
    [SerializeField] private float cutSceneTime;
    // public string nextSceneName; // The name of the next scene to load
    private void Start()
    {
       
        // Get the Button component attached to the GameObject
        // Button button = GetComponent<Button>();
        //
        // // Add a listener to the button's onClick event
        // button.onClick.AddListener(SkipLevel);
    }

    private void Update()
    {
        timePast += Time.deltaTime;
        if (timePast >= cutSceneTime)
        {
            if (SceneManager.GetActiveScene().name == "EndScene")
            {
                GameObject obj = GameObject.FindGameObjectWithTag("gameManager");
                if(obj)
                    Destroy(obj);
                SceneManager.LoadScene("Credits");
            }
            else SceneManager.LoadScene("Levels");
        }
            
    }
    public void SkipLevel()
    {
        SceneManager.LoadScene("Levels");
    }
}