using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownSlot : MonoBehaviour
{
    private TMP_Text countdownText;
    private Image skillIconImage;
    private float skillCooldown;
    private float skillCooldownRemaining = 0;
    private bool isSkillActive = false;

    private void Awake()
    {
        skillIconImage = GetComponent<Image>();
        countdownText = GetComponentInChildren<TMP_Text>();
        countdownText.enabled = false;
    }

    public void SetSkillSlot(float skillCooldown, Sprite skillIcon) 
    {
        this.skillCooldown = skillCooldown;
        skillIconImage.sprite = skillIcon;
    }
    public void skillActivated() 
    {
        skillCooldownRemaining = skillCooldown;// / PlayerStatsManager.Instance.SkillSpeed;
        skillIconImage.color = Color.gray;
        countdownText.enabled = true;
        isSkillActive = true;
    }

    private void Update()
    {
        if (isSkillActive)
        {
            if (skillCooldownRemaining > 0)
            {
                skillCooldownRemaining -= Time.deltaTime;

                countdownText.text = Mathf.Ceil(skillCooldownRemaining).ToString();
                
            }
            else
            {
                skillIconImage.color = Color.white;
                countdownText.enabled = false;
                isSkillActive = false;
            }
        }
    }
}
