using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quản lý Scroll View hiển thị 5 skills của tank đang preview.
/// Khi switch tank → xóa list cũ → tạo lại 5 items mới.
/// 
/// Init trong Inspector:
/// - _skillItemPrefab: prefab có component SkillItemUI
/// - _contentParent: Transform Content của Scroll View
/// </summary>
public class SkillController : MonoBehaviour
{
    [SerializeField] GameObject _skillItemPrefab;
    [SerializeField] Transform _contentParent;

    private UpgradeManager _upgradeManager;
    private List<SkillItemUI> _currentItems = new List<SkillItemUI>();
    private int _lastPreviewTankId = -1;

    void Start()
    {
        _upgradeManager = UpgradeManager.Instance;

        _upgradeManager.OnUnlockSkillChanged += RefreshSkills;
        _upgradeManager.OnCoinsChanged += OnCoinsChanged;

        LoadSkills();
    }

    private void OnDisable()
    {
        if (_upgradeManager != null)
        {
            _upgradeManager.OnUnlockSkillChanged -= RefreshSkills;
            _upgradeManager.OnCoinsChanged -= OnCoinsChanged;
        }
    }

    /// <summary>
    /// Khi stat/skill/tank thay đổi:
    /// - Nếu tank preview đổi → rebuild toàn bộ list
    /// - Nếu cùng tank → chỉ refresh trạng thái các items
    /// </summary>
    private void RefreshSkills()
    {
        Debug.Log("Test unlock skill event");
        if (_upgradeManager.PreviewTankId != _lastPreviewTankId)
            LoadSkills();
        else
            RefreshAllItems();
    }

    private void OnCoinsChanged(int coins)
    {
        RefreshAllItems();
    }

    /// <summary>
    /// Xóa items cũ → tạo 5 items mới cho tank đang preview.
    /// </summary>
    private void LoadSkills()
    {
        // Xóa items cũ
        ClearItems();

        _lastPreviewTankId = _upgradeManager.PreviewTankId;

        // tankId format: "Tank01", "Tank02", ...
        string tankId = $"Tank{_lastPreviewTankId + 1:D2}";

        List<SkillUpgradeConfig> skills = _upgradeManager.GetSkillsForTank(tankId);

        foreach (SkillUpgradeConfig skill in skills)
        {
            GameObject itemObj = Instantiate(_skillItemPrefab, _contentParent);
            SkillItemUI itemUI = itemObj.GetComponent<SkillItemUI>();

            int skillIndex = _upgradeManager.GetSkillIndex(skill);
            itemUI.Init(_upgradeManager, skill, skillIndex);

            _currentItems.Add(itemUI);
        }
    }

    private void RefreshAllItems()
    {
        foreach (SkillItemUI item in _currentItems)
        {
            item.RefreshState();
        }
    }

    private void ClearItems()
    {
        foreach (SkillItemUI item in _currentItems)
        {
            Destroy(item.gameObject);
        }
        _currentItems.Clear();
    }
}
