using System.IO;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class FileDataHandler
{
    private string _dataDirPath = "";
    private string _dataFileName = "";
    private bool _useEncryption = false;
    private readonly string _encryptionCodeWord = "code";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this._dataDirPath = dataDirPath;
        this._dataFileName = dataFileName;
        this._useEncryption = useEncryption;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                //đọc dữ liệu từ file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //mã hóa hay không??
                if (_useEncryption)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                //deserialize từ JSON sang C# data game
                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error when load data: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        //
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        try
        {
            //tạo thư mục lưu file nếu chưa tồn tại
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));


            //serialize C# data game sang JSON
            string dataToSave = JsonConvert.SerializeObject(data, Formatting.Indented);

            //mã hóa hay không??
            if (_useEncryption)
               dataToSave = EncryptDecrypt(dataToSave);

            //ghi dữ liệu vào file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToSave);
                }
            }

        }
        catch (Exception e)
        {
            Debug.LogError("Error when save data: " + fullPath + "\n" + e);
        }
    }

    //Dùng toán tử XOR (^) để mã hóa dữ liệu
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
        }

        return modifiedData;
    }
}
