using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject _curretnPattern;

    private void Start()
    {
        string difficulty = PlayerPrefs.GetString("DIFF");
        if (_patterns != null)
        {
            if (difficulty == "normal" && _enableAtNormal || difficulty == "expert" && _enableAtExpert || difficulty == "ruthless" && _enableAtRuthless)
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
            Destroy(_curretnPattern);
            FindObjectOfType<FlashEffect>().Flash();
            var pickedPattern = _patterns[Random.Range(0, _patterns.Length)];
            var spawnPos = this.transform.position;
            if (pickedPattern.name == "RainShot")
            {
                spawnPos = pickedPattern.transform.position;
            }
            _curretnPattern = Instantiate(pickedPattern, spawnPos, Quaternion.identity);
        }
    }
}
