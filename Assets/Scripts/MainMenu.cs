using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _startBtn;
    [SerializeField] Button _continueBtn;
    [SerializeField] TextMeshProUGUI _coinText;
    private UpgradeManager _upgradeManager;

    private void OnDisable()
    {
        _upgradeManager.OnCoinsChanged -= UpdateCoinUI;
    }
    private void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
            _continueBtn.interactable = false;

        _upgradeManager = UpgradeManager.Instance;
        UpdateCoinUI(_upgradeManager.PlayCoins);
        _upgradeManager.OnCoinsChanged += UpdateCoinUI;
    }

    private void UpdateCoinUI(int playerCoin)
    {
        _coinText.text = playerCoin.ToString();
    }
    public void OnStartButtonClicked()
    {
        DisableButton();
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync("PlayScene");
    }

    public void OnContinueButtonClicked()
    {
        DisableButton();
        SceneManager.LoadSceneAsync("PlayScene");
    }

    private void DisableButton()
    {
        _startBtn.interactable = false;
        _continueBtn.interactable = false;
    }
}
