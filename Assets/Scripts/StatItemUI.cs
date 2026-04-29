using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _statValueText;
    [SerializeField] TextMeshProUGUI _upgradeCostText;
    [SerializeField] Button _upgradeBtn;
    [SerializeField] string _statName;

    private UpgradeManager _upgradeManager;

    void Start()
    {
        _upgradeManager = UpgradeManager.Instance;

        _upgradeManager.OnUpgradeChanged += RefreshUI;
        _upgradeManager.OnCoinsChanged += OnCoinsChanged;
        RefreshUI();
    }

    private void OnDisable()
    {
        if (_upgradeManager != null)
        {
            _upgradeManager.OnUpgradeChanged -= RefreshUI;
            _upgradeManager.OnCoinsChanged -= OnCoinsChanged;
        }
    }


    public void OnStatUpgradeButtonClicked()
    {
        _upgradeManager.UpgradeStat(_statName);
    }


    private void RefreshUI()
    {
        _statValueText.text = _upgradeManager.GetStatValue(_statName).ToString();

        if (_upgradeManager.IsStatMaxLevel(_statName))
        {
            _upgradeCostText.text = "Max";
            _upgradeBtn.interactable = false;
            return;
        }
        else
        {
            int cost = _upgradeManager.GetStatUpgradeCosts(_statName);
            _upgradeBtn.interactable = _upgradeManager.PlayCoins >= cost;
            _upgradeCostText.text = cost.ToString();
        }
    }

    private void OnCoinsChanged(int coins)
    {
        RefreshUI();
    }
}
