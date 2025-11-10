using TMPro;
using UnityEngine;

public class TextBasedEntityPresenter : EntityDataPresenter
{
    [Header("Text objects")]
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text xpText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text nameText;
    protected override void UpdateDisplay()
    {
        if (hpText != null) hpText.text = $"{boundEntity.CurrentHP} / {boundEntity.maxHP}";
        if (xpText != null) xpText.text = $"{boundEntity.CurrentXP} / {boundEntity.XPRequired}";
        if (levelText != null) levelText.text = $"LV {boundEntity.level}";
        if (nameText != null) nameText.text = boundEntity.entityName;
    }
}
