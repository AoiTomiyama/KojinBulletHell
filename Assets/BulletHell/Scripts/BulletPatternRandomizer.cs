using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

/// <summary>
/// �e���p�^�[����z�񂩂烉���_���ɑI�o���A��������X�N���v�g�B
/// </summary>
public class BulletPatternRandomizer : MonoBehaviour
{
    [Header("�L���ɂ��邩�ǂ���")]
    [SerializeField]
    bool _isEnabled;

    [Header("�ҋ@�b��")]
    [SerializeField]
    float _waitSeconds = 5;

    [Header("�e���̃p�^�[��")]
    [SerializeField]
    GameObject[] _patterns;

    /// <summary> ���݂̒e���p�^�[����ۑ��B </summary>
    GameObject _curretnPattern;
    private void Start()
    {
        if (_isEnabled && _patterns != null)
        {
            StartCoroutine(PatternSwitcher());
        }
    }

    IEnumerator PatternSwitcher()
    {
        while (true)
        {
            if (_curretnPattern != null)
            {
                var emission = _curretnPattern.GetComponent<ParticleSystem>().emission;
                emission.enabled = false;
                Destroy(_curretnPattern, 10);
            }
            var pickedPattern = _patterns[Random.Range(0, _patterns.Length)];
            var spawnPos = this.transform.position;
            if (pickedPattern.name == "RainShot")
            {
                spawnPos = pickedPattern.transform.position;
            }
            _curretnPattern = Instantiate(pickedPattern, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(_waitSeconds);
        }
    }
}
