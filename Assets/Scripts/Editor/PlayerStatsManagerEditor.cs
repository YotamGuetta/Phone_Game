using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;

[CustomEditor(typeof(PlayerStatsManager))]
public class PlayerStatsManagerEditor : Editor
{
    public VisualTreeAsset VisualTree;

    private PlayerStatsManager playerStatsManager;
    private UnityEngine.UIElements.Button combatDefultValues;
    private UnityEngine.UIElements.Button movementDefultValues;
    private UnityEngine.UIElements.Button healthDefultValues;

    private PropertyField currentHealthProperty;
    private PropertyField maxHealthProperty;
    private Toggle toggleDynamicHealth;

    private int oldMaxHealthValue;
    private int oldCurrHealthValue;
    private void OnEnable()
    {
        playerStatsManager = (PlayerStatsManager)target;
        oldMaxHealthValue = playerStatsManager.maxHealth;
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        //Adds all the UI builder to the editor
        VisualTree.CloneTree(root);

        //Find and assign callbacks for the buttons
        combatDefultValues = root.Q<UnityEngine.UIElements.Button>("CombatDefultValues");
        combatDefultValues.RegisterCallback<ClickEvent>(onCombatDefultValuesClick);

        movementDefultValues = root.Q<UnityEngine.UIElements.Button>("MovementDefultValues");
        movementDefultValues.RegisterCallback<ClickEvent>(onMovementDefultValuesClick);

        healthDefultValues = root.Q<UnityEngine.UIElements.Button>("HealthDefultValues");
        healthDefultValues.RegisterCallback<ClickEvent>(onHealthDefultValuesClick);



        //Keep Health assignment logic
        currentHealthProperty = root.Q<PropertyField>("CurrentHealthProperty");
        currentHealthProperty.RegisterValueChangeCallback(OntCurrentHealthChanged);

        maxHealthProperty = root.Q<PropertyField>("MaxHealthProperty");
         maxHealthProperty.RegisterValueChangeCallback(OnMaxHealthChanged);

        return root;
    }

    private void OntCurrentHealthChanged(SerializedPropertyChangeEvent evt)
    {
        playerStatsManager.AdjustCurrentHealthChangeToValid();

        oldCurrHealthValue = evt.changedProperty.intValue;
        if (playerStatsManager != null && playerStatsManager.playerHealthPoints != null)
            playerStatsManager.playerHealthPoints.updateHealthText(oldMaxHealthValue, oldCurrHealthValue);
    }

    private void OnMaxHealthChanged(SerializedPropertyChangeEvent evt)
    {
        if (playerStatsManager.ActivateDynamicHealth)
            playerStatsManager.AdjustMaxHealthChangeToValid(oldMaxHealthValue);
        oldMaxHealthValue = evt.changedProperty.intValue;

        if (playerStatsManager != null && playerStatsManager.playerHealthPoints != null)
            playerStatsManager.playerHealthPoints.updateHealthText(oldMaxHealthValue, oldCurrHealthValue);
    }

    private void onCombatDefultValuesClick(ClickEvent evt) 
    {
        playerStatsManager.SetAllCombatStatsToDefultValues();
    }
    private void onMovementDefultValuesClick(ClickEvent evt)
    {
        playerStatsManager.SetAllMovementStatsToDefultValues();
    }
    private void onHealthDefultValuesClick(ClickEvent evt)
    {
        playerStatsManager.SetAllHealthStatsToDefultValues();
    }
}
