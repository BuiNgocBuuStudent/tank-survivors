using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _fileDataHandler;

    [Header("File Storage Config")]
    [SerializeField] string _fileName;
    [SerializeField] bool _useEncryption;
    
    public void Init()
    {
        this._fileDataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    public void NewGame()
    {
        this._gameData = new GameData();
    }
    public void LoadGame()
    {
        this._gameData = _fileDataHandler.Load();

        if (this._gameData == null)
        {
            Debug.LogWarning("No data was found. Need to start NEW GAME before load data");
            NewGame();
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
            dataPersistenceObject.SaveData(ref _gameData);
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
}
