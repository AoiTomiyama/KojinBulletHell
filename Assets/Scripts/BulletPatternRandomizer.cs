using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary> �p�^�[���؂�ւ�鎞�ɕ\������p�l�� </summary>
    Image _flashPanel;
    private void Start()
    {
        _flashPanel = GameObject.Find("FlashPanel").GetComponent<Image>();
        _flashPanel.enabled = false;
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
                _flashPanel.enabled = true;
                Destroy(_curretnPattern);
                yield return new WaitForSeconds(0.1f);
                _flashPanel.enabled = false;
            }
            var pickedPattern = _patterns[Random.Range(0, _patterns.Length)];
            var spawnPos = this.transform.position;
            if (pickedPattern.name == "RainShot")
            {
                spawnPos = pickedPattern.transform.position;
            }
            _curretnPattern = Instantiate(pickedPattern, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(_waitSeconds - 0.1f);
        }
    }
}
