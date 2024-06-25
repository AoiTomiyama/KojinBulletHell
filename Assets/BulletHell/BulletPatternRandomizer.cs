using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class BulletPatternRandomizer : MonoBehaviour
{
    [Header("�L���ɂ��邩�ǂ���")]
    [SerializeField]
    bool isEnabled;

    [Header("�ҋ@�b��")]
    [SerializeField]
    float _waitSeconds = 5;

    [Header("�e���̃p�^�[��")]
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
