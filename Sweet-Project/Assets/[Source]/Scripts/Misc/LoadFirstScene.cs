using UnityEngine;

public class LoadFirstScene : MonoBehaviour
{
	private void Awake ()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
