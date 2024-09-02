using System.Collections;
using UnityEngine;

/// <summary>
/// 弾幕パターンを配列からランダムに選出し、生成するスクリプト。
/// </summary>
public class BulletPatternRandomizer : MonoBehaviour
{
    [SerializeField, Header("どの難易度で有効にするかどうか")]
    private bool _enableAtNormal;
    [SerializeField]
    private bool _enableAtExpert;
    [SerializeField]
    private bool _enableAtRuthless;

    [SerializeField, Header("待機秒数")]
    float _waitSeconds = 5;

    [SerializeField, Header("弾幕のパターン")]
    private GameObject[] _patterns;

    /// <summary> 現在の弾幕パターンを保存。 </summary>
    private GameObject _currentPattern;

    private void Start()
    {
        var difficulty = (Enums.Difficulties)PlayerPrefs.GetInt("DIFF_INT");
        if (_patterns != null && _patterns.Length > 0)
        {
            if ((difficulty == Enums.Difficulties.Normal && _enableAtNormal) ||
                (difficulty == Enums.Difficulties.Expert && _enableAtExpert) ||
                (difficulty == Enums.Difficulties.Ruthless && _enableAtRuthless))
            {
                StartCoroutine(PatternSwitcher());
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator PatternSwitcher()
    {
        while (true)
        {
            yield return new WaitForSeconds(_waitSeconds - 0.1f);
            Destroy(_currentPattern);
            FindObjectOfType<FlashEffect>().Flash();
            var pickedPattern = _patterns[Random.Range(0, _patterns.Length)];
            var spawnPos = this.transform.position;
            if (pickedPattern.name == "RainShot")
            {
                spawnPos = pickedPattern.transform.position;
            }
            _currentPattern = Instantiate(pickedPattern, spawnPos, Quaternion.identity);
        }
    }
}
