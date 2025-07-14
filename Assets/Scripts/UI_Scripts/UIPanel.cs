using UnityEngine;


public class UIPanel : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    /// <summary>
    /// Switch between show / hide panel on the screen
    /// </summary>
    public virtual void TogglePanel() 
    {
        MainMenu.ToggleCanvasGroup(canvasGroup);
    }
    /// <summary>
    /// Checks if the panel is open (showing)
    /// </summary>
    /// <returns>Is the panel open</returns>
    public bool IsPanelOpen()
    {
        return canvasGroup.alpha == 1;
    }
}
