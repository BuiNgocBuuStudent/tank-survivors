using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _startBtn;
    [SerializeField] Button _continueBtn;

    private void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
            _continueBtn.interactable = false;
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
