using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }
    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _fileDataHandler;

    [Header("Debug")]
    [SerializeField] bool _initializeDataIfNull;

    [Header("File Storage Config")]
    [SerializeField] string _fileName;
    [SerializeField] bool _useEncryption;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        this._fileDataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mdoe)
    {
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnLoaded(Scene scene)
    {
        SaveGame();
    }
    public void NewGame()
    {
        this._gameData = new GameData();
    }
    public void LoadGame()
    {
        this._gameData = _fileDataHandler.Load();

        if(this._gameData == null && _initializeDataIfNull)
            NewGame();

        if (this._gameData == null)
        {
            Debug.LogWarning("No data was found. Need to start NEW GAME before load data");
            return;
        }

        foreach (IDataPersistence dataPersistenceObject in this._dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        if (_gameData == null)
        {
            Debug.LogWarning("No data was found. Need to start NEW GAME before save data");
            return;
        }

        foreach (IDataPersistence dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(_gameData);
        }

        _fileDataHandler.Save(_gameData);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return _gameData != null;
    }

    public void RegisterAndLoad(IDataPersistence dataPersistenceObject)
    {
        _dataPersistenceObjects.Add(dataPersistenceObject);
        dataPersistenceObject.LoadData(_gameData);
    }
}
