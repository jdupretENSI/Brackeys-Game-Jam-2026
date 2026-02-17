using UnityEngine;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private Button _start, _settings, _credits, _exit;
    
    public void StartGame(){}
    public void Settings(){}
    public void Credits(){}

    public void ExitGame()
    {
        Application.Quit();
    }
    
}
