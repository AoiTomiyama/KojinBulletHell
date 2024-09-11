using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveDateManager : MonoBehaviour
{
    private static string _filePath;
    public static SaveDateManager Instance { get; private set; }
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
    /// <summary>
    /// セーブデータをJSON形式にしてローカルに保存する。
    /// </summary>
    /// <param name="playerList">保存するリスト</param>
    public void SaveData(List<Record> playerList)
    {
        var wrapper = new PlayerDataWrapper { playerDataList = playerList };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(_filePath, json);
        Debug.Log($"<color=lightblue>[SaveDataManager]</color> Save data successfully saved! : {_filePath}");
    }
    /// <summary>
    /// ローカルからセーブデータを取得する
    /// </summary>
    /// <returns>ローカルから取得したセーブデータ。取得できなかった場合は空のリストが返される。</returns>
    public List<Record> LoadData()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            PlayerDataWrapper wrapper = JsonUtility.FromJson<PlayerDataWrapper>(json);
            Debug.Log("<color=lightblue>[SaveDataManager]</color> Save data successfully loaded!");
            return wrapper.playerDataList;
        }
        else
        {
            Debug.LogWarning("<color=lightblue>[SaveDataManager]</color> Couldn't find save data!");
            return new List<Record>();
        }
    }
    /// <summary>
    /// セーブデータに新しく記録を追加する。
    /// </summary>
    /// <param name="record">追加するデータ。</param>
    public void AddData(Record record)
    {
        var recordList = LoadData();
        Debug.Log("<color=lightblue>[SaveDataManager]</color> Adding new data...");
        recordList.Add(record);
        if (recordList.Where(data => data._stage == record._stage && data._difficulty == record._difficulty).Count() >= 4)
        {
            Debug.LogWarning("<color=lightblue>[SaveDataManager]</color> Deleting worst record in save data...");

            recordList = recordList.Where(data => data._stage == record._stage && data._difficulty == record._difficulty)
                .OrderBy(data => data._time)
                .Take(3)
                .Concat(recordList.Where(data => data._stage != record._stage || data._difficulty != record._difficulty).OrderBy(data => data._time))
                .ToList();
        }
        Debug.Log("<color=lightblue>[SaveDataManager]</color> Data successfully added to save data!");
        SaveData(recordList);
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
