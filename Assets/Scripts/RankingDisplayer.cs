using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RankingDisplayer : MonoBehaviour
{
    [SerializeField, Header("�L�^��\��������ꏊ")]
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
    /// �\��������������B
    /// </summary>
    public void ResetDisplay()
    {
        DisplayRecord(-1);
    }
    /// <summary>
    /// �����L���O��UI�ɕ\������B
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
