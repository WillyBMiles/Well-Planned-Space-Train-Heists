using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] string menuScene;
    public void Go()
    {
        SceneManager.LoadScene(menuScene);
    }
}
