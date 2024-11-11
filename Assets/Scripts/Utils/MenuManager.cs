using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void NewGame(){
        SceneManager.LoadScene("Game");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
