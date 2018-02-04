using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLevelButton : MonoBehaviour
{
    public string levelName;

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelName);
    }
}
