using UnityEngine;

public class CheckPreload : MonoBehaviour
{
    void Awake()
    {
        if (GameObject.Find("__App") == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}