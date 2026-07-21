using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _startBtn;
    [SerializeField] TextMeshProUGUI _coinText;
    private UpgradeManager _upgradeManager;

    [SerializeField] GameObject _settingUI;

    [SerializeField] GameObject _controlUI;
    private void OnDisable()
    {
        _upgradeManager.OnCoinsChanged -= UpdateCoinUI;
    }
    private void Start()
    {
        _upgradeManager = UpgradeManager.Instance;
        UpdateCoinUI(_upgradeManager.PlayCoins);
        _upgradeManager.OnCoinsChanged += UpdateCoinUI;

        AudioManager.Instance.PlayMusic(MusicType.menuTheme);
    }

    private void UpdateCoinUI(int playerCoin)
    {
        _coinText.text = playerCoin.ToString();
    }
    public void OnStartBtnClicked()
    {
        AudioManager.Instance.PlaySFX(SFXType.btnPlay);
        DisableButton();
        DataPersistenceManager.Instance.NewGame();
        UpgradeManager.Instance.PrepareForGame(); // Ghi data vào SessionData trước khi đổi scene
        SceneManager.LoadSceneAsync("PlayScene");
        int index = Random.Range(0, 3);
        AudioManager.Instance.PlayMusic((MusicType)index);
        Time.timeScale = 1f;
    }

    public void OnSettingBtnClicked()
    {
        AudioManager.Instance.PlaySFX(SFXType.btnClick);
        _settingUI.SetActive(true);
    }
    public void OnControlBtnClicked()
    {
        AudioManager.Instance.PlaySFX(SFXType.btnClick);
        _controlUI.SetActive(true);
    }
    public void OnCancelBtnClicked()
    {
        AudioManager.Instance.PlaySFX(SFXType.btnClick);
        _settingUI.SetActive(false);
        _controlUI.SetActive(false);
    }
    private void DisableButton()
    {
        _startBtn.interactable = false;
    }
}
