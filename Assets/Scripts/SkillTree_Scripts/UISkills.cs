using UnityEngine;

public class UISkills : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    private bool skillTreeOpen;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("ToggleSkills")) 
        {
            // If alpha = 0: timscale = 0, alpha is 1 and viceversa
            Time.timeScale = canvasGroup.alpha;
            skillTreeOpen = canvasGroup.alpha == 1;
            canvasGroup.blocksRaycasts = skillTreeOpen;
            canvasGroup.interactable = skillTreeOpen;
            canvasGroup.alpha = (canvasGroup.alpha - 1) * (-1);

        }
    }
}
