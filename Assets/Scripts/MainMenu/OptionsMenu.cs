using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private MainMenu optionsCG;
    [SerializeField] private CanvasGroup grapicsCG;

    /// <summary>
    /// Exit the current menu and opens the options menu
    /// </summary>
    /// <param name="gc">The current open menu</param>
    public void BackToOptions(CanvasGroup gc) 
    {
        optionsCG.ToggleOptionsMenuCG();
        MainMenu.ToggleCanvasGroup(gc);
    }
    /// <summary>
    /// Exit the options menu and opens the graphics menu
    /// </summary>
    public void GoToGrapicsOptions()
    {
        optionsCG.ToggleOptionsMenuCG();
        MainMenu.ToggleCanvasGroup(grapicsCG);
    }
    /// <summary>
    /// Exit the options menu and opens the main menu
    /// </summary>
    public void BackToMenu()
    {
        optionsCG.ToggleMainMenuCG();
        optionsCG.ToggleOptionsMenuCG();
    }
}
