using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EqippedToolUI: MonoBehaviour
{
    [SerializeField] private PlayerEquipment playerEquipment;
    [SerializeField] private Image toolIcon;
    [SerializeField] private TMP_Text toolNameText;

    private void Start()
    {
        if (playerEquipment == null)
        {
            playerEquipment = FindAnyObjectByType<PlayerEquipment>();
        }

        playerEquipment.EquippedToolChanged += RefreshTool;

        RefreshTool(playerEquipment.EquippedTool);
    }

    private void RefreshTool(ToolDefinition tool)
    {
        if (tool == null)
        {
            toolIcon.sprite = null;
            toolIcon.enabled = false;

            toolNameText.text = "";

            return;
        }

        toolIcon.sprite = tool.Icon;
        toolIcon.enabled = tool.Icon != null;

        toolNameText.text = tool.DisplayName;
    }

    private void OnDestroy()
    {
        if (playerEquipment != null)
        {
            playerEquipment.EquippedToolChanged -= RefreshTool;
        }
    }
}
