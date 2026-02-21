using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This manager takes care of everything UI based from the start screen,
/// it can also be use for the pause, the game UI or anything else.
/// Everything works by turning panels on and off depending on the need
/// </summary>
public class HomeManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject _panelHome;
    [SerializeField] private Button _start; 
    [SerializeField] private Button _settings; 
    [SerializeField] private Button _credits; 
    [SerializeField] private Button _exit;
    
    [Header("Settings")]
    [SerializeField] private GameObject _panelSettings;
    
    [Header("Credits")]
    [SerializeField] private GameObject _panelCredits;
    
    [Header("Transition")]
    [SerializeField] private GameObject _panelTransition;
    
    [Header("Pause")]
    [SerializeField] private GameObject _panelPause;
    
    
    public void StartGame(){}
    public void Settings(){}
    public void Credits(){}

    public void ExitGame()
    {
        Application.Quit();
    }
    
}
