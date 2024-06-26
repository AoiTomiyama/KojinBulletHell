using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static float _time;
    public static float gameTime => _time;
    Text _text;
    private void Start()
    {
        _time = 0f;
        _text = GameObject.Find("TimeField").GetComponent<Text>();
    }
    void FixedUpdate()
    {
        _time += Time.deltaTime;
        if (_text != null)
        {
            _text.text = _time.ToString("F2");
        }
    }
}
