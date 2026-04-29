using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gắn lên mỗi Skill Item prefab trong Scroll View.
/// Hiển thị: tên skill, mô tả, tier, chi phí, trạng thái (locked/available/unlocked).
/// </summary>
public class SkillItemUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] TextMeshProUGUI _skillNameText;
    [SerializeField] TextMeshProUGUI _descriptionText;
    [SerializeField] TextMeshProUGUI _costText;
    //[SerializeField] Image _icon;
    [SerializeField] Image _lockImage;
    [SerializeField] Button _unlockBtn;

    private UpgradeManager _upgradeManager;
    private int _skillIndex;

    /// <summary>
    /// Được gọi bởi SkillController khi populate list.
    /// </summary>
    public void Init(UpgradeManager upgradeManager, SkillUpgradeConfig config, int skillIndex)
    {
        _upgradeManager = upgradeManager;
        _skillIndex = skillIndex;

        _skillNameText.text = $"Tier {config.tier}: {config.skillName}";
        _descriptionText.text = config.description;

        RefreshState();
    }

    /// <summary>
    /// Gắn vào Button OnClick trong prefab.
    /// </summary>
    public void OnUnlockButtonClicked()
    {
        _upgradeManager.TryUnlockSkill(_skillIndex);
        // UI sẽ refresh qua SkillController lắng nghe RefreshSkills
    }

    /// <summary>
    /// Cập nhật visual state: Unlocked / Available / Locked.
    /// </summary>
    public void RefreshState()
    {
        if (_upgradeManager == null) return;

        SkillUpgradeConfig config = _upgradeManager.SkillConfigs[_skillIndex];
        bool isUnlocked = _upgradeManager.IsSkillUnlocked(_skillIndex);
        int highestTier = _upgradeManager.GetHighestUnlockedTier(config.tankId);
        bool isNextTier = config.tier == highestTier + 1;
        bool tankUnlocked = _upgradeManager.IsTankUnlocked(_upgradeManager.PreviewTankId);

        if (isUnlocked)
        {
            _unlockBtn.gameObject.SetActive(false);
            _lockImage.gameObject.SetActive(false);
        }
        else if (isNextTier && tankUnlocked)
        {
            _costText.text = config.cost.ToString();
            _unlockBtn.interactable = _upgradeManager.PlayCoins >= config.cost;
            _lockImage.gameObject.SetActive(false);
        }
        else
        {
            _lockImage.gameObject.SetActive(true);
        }
    }
}
