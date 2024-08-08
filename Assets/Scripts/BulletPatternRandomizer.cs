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

    /// <summary> パターン切り替わる時に表示するパネル </summary>
    private Image _flashPanel;
    private void Start()
    {
        _flashPanel = GameObject.Find("FlashPanel").GetComponent<Image>();
        _flashPanel.enabled = false;
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
            _flashPanel.enabled = true;
            Destroy(_curretnPattern);
            yield return new WaitForSeconds(0.1f);
            _flashPanel.enabled = false;
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
