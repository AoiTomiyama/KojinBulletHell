using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDateManager : MonoBehaviour
{
    private static string _filePath;
    private SaveDateManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
    }
    public static void SaveData(List<Record> playerList)
    {
        PlayerDataWrapper wrapper = new PlayerDataWrapper { playerDataList = playerList };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(_filePath, json);
        Debug.Log($"データを保存しました: {_filePath}");
    }

    public static List<Record> LoadData()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            PlayerDataWrapper wrapper = JsonUtility.FromJson<PlayerDataWrapper>(json);
            Debug.Log("データを読み込みました。");
            return wrapper.playerDataList;
        }
        else
        {
            Debug.LogWarning("ファイルが見つかりません。");
            return new List<Record>();
        }
    }

    [System.Serializable]
    public class Record
    {
        public string _stage;
        public int _difficulty;
        public float _time;

        public Record(string stage, int difficulty, float time)
        {
            _stage = stage;
            _difficulty = difficulty;
            _time = time;
        }
    }

    [System.Serializable]
    public class PlayerDataWrapper
    {
        public List<Record> playerDataList;
    }
}
