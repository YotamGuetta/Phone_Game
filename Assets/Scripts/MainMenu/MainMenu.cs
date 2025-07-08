using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup optionsCG;
    private CanvasGroup MainMenuCG;

    private void Start()
    {
        MainMenuCG = GetComponent<CanvasGroup>();
    }
    /// <summary>
    /// Starts the game
    /// </summary>
    public void LoadGame() 
    {
        SceneManager.LoadScene(sceneName: "Level1");
    }
    /// <summary>
    /// Exits the main menu and opens the options menu
    /// </summary>
    public void LoadOptions()
    {
        if (optionsCG != null && MainMenuCG != null) 
        {
            ToggleOptionsMenuCG();
            ToggleMainMenuCG();
        }
    }
    /// <summary>
    /// Toggle main menu off/on
    /// </summary>
    public void ToggleMainMenuCG() 
    {
        ToggleCanvasGroup(MainMenuCG);
    }
    /// <summary>
    /// Toggle options menu off/on
    /// </summary>
    public void ToggleOptionsMenuCG()
    {
        ToggleCanvasGroup(optionsCG);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game");
        Application.Quit();
    }
    /// <summary>
    /// Gets a group canvas of a menu and toggle it off/on
    /// </summary>
    /// <param name="cg">Te menu to toggle</param>
    public static void ToggleCanvasGroup(CanvasGroup cg) 
    {
        float newAlpha = Mathf.Abs(cg.alpha - 1);
        cg.alpha = newAlpha;
        cg.interactable = newAlpha == 1;
        cg.blocksRaycasts = newAlpha == 1;
    }
}
