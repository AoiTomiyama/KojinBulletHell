using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class BulletPatternRandomizer : MonoBehaviour
{
    [Header("有効にするかどうか")]
    [SerializeField]
    bool isEnabled;

    [Header("待機秒数")]
    [SerializeField]
    float _waitSeconds = 5;

    [Header("弾幕のパターン")]
    [SerializeField]
    GameObject[] _patterns;

    GameObject _curretnPattern;
    private void Start()
    {
        if (isEnabled && _patterns != null)
        {
            StartCoroutine(PatternSwitcher());
            _curretnPattern = Instantiate(_patterns[Random.Range(0, _patterns.Length)]);
        }
    }

    IEnumerator PatternSwitcher()
    {
        while (true)
        {
            yield return new WaitForSeconds(_waitSeconds);
            var emission = _curretnPattern.GetComponent<ParticleSystem>().emission;
            emission.enabled = false;
            Destroy(_curretnPattern, 10);
            _curretnPattern = Instantiate(_patterns[Random.Range(0, _patterns.Length)]);
        }
    }
}
