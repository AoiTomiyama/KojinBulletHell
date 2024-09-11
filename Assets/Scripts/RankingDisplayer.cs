using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RankingDisplayer : MonoBehaviour
{
    [SerializeField, Header("記録を表示させる場所")]
    private TextMeshProUGUI[] _texts;
    private List<SaveDateManager.Record> _recordList;
    private TitleManager _titleManager;
    private void Start()
    {
        _recordList = SaveDateManager.Instance.LoadData();
        _titleManager = FindObjectOfType<TitleManager>();
        ResetDisplay();
    }
    /// <summary>
    /// 表示を初期化する。
    /// </summary>
    public void ResetDisplay()
    {
        DisplayRecord(-1);
    }
    /// <summary>
    /// ランキングをUIに表示する。
    /// </summary>
    public void DisplayRecord(int difficulty)
    {
        var data = _recordList.Where(data => data._stage == _titleManager.SelectedLevel && data._difficulty == difficulty).OrderBy(data => data._time).ToList();
        for (int i = 0; i < _texts.Length; i++)
        {
            if (i < data.Count)
            {
                _texts[i].text = $"{i + 1}. {data[i]._time.ToString("000.00")}";
            }
            else
            {
                _texts[i].text = $"{i + 1}. ----------";
            }
        }
    }
}
