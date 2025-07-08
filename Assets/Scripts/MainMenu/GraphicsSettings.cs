using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown QualityDropDown;
    [SerializeField] private TMP_Dropdown resolutionsDropDown;
    [SerializeField] private OptionsMenu optionsMenu;
    private Resolution[] resolutions;
    private CanvasGroup graphicsCG;

    private void Start()
    {
        graphicsCG = GetComponent<CanvasGroup>();
        updateQualityDropBox();
        updateResolutionsDropBox();
    }
    //Updates the quality options to the available ones
    private void updateQualityDropBox()
    {
        //Gets all the available quality levels
        string[] qualityNames = QualitySettings.names;
        //clears the previous options from the dropbox
        QualityDropDown.ClearOptions();

        //Save the original quality
        int currentResolutionIndex = QualitySettings.GetQualityLevel();

        List<string> qualityOptions = new List<string>();
        foreach (var qualityName in qualityNames)
        {
            qualityOptions.Add(qualityName);
        }
        //Updates the options
        QualityDropDown.AddOptions(qualityOptions);

        //set the current option to the original one
        QualityDropDown.value = currentResolutionIndex;
        QualityDropDown.RefreshShownValue();
    }
    //Updates the resulutions options to the available ones
    private void updateResolutionsDropBox()
    {
        //Gets all the available resolutions
        resolutions = Screen.resolutions;
        //clears the previous options from the dropbox
        resolutionsDropDown.ClearOptions();

        List<string> resolutionsOptions = new List<string>();

        int currentResolutionIndex = 0;
        bool foundcurrentResolution = false;

        foreach (var resolution in resolutions)
        {
            string option = resolution.width + " x " + resolution.height;
            resolutionsOptions.Add(option);

            //coninues throw the resolution options until the original one if found
            if (Screen.currentResolution.height == resolution.height && Screen.currentResolution.width == resolution.width)
            {
                foundcurrentResolution = true;
            }
            if (!foundcurrentResolution)
            {
                currentResolutionIndex++;
            }
        }
        //Updates the options
        resolutionsDropDown.AddOptions(resolutionsOptions);

        //set the current option to the original one
        resolutionsDropDown.value = currentResolutionIndex;
        resolutionsDropDown.RefreshShownValue();
    }

    
    public void SetResolution()
    {
        //Gets the selected resulution from the user
        Resolution resolution = resolutions[resolutionsDropDown.value];
        //Updates the resulution
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen) 
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality() 
    {
        //Gets the selected quality from the user and updates it
        QualitySettings.SetQualityLevel(QualityDropDown.value);
    }

    public void BackToOptions() 
    {
        optionsMenu.BackToOptions(graphicsCG);
    }
}
