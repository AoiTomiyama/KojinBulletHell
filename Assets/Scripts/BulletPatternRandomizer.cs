using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �e���p�^�[����z�񂩂烉���_���ɑI�o���A��������X�N���v�g�B
/// </summary>
public class BulletPatternRandomizer : MonoBehaviour
{
    [Header("�ǂ̓�Փx�ŗL���ɂ��邩�ǂ���")]
    [SerializeField]
    private bool _enableAtNormal;
    [SerializeField]
    private bool _enableAtExpert;
    [SerializeField]
    private bool _enableAtRuthless;

    [Header("�ҋ@�b��")]
    [SerializeField]
    float _waitSeconds = 5;

    [Header("�e���̃p�^�[��")]
    [SerializeField]
    private GameObject[] _patterns;

    /// <summary> ���݂̒e���p�^�[����ۑ��B </summary>
    private GameObject _curretnPattern;

    /// <summary> �p�^�[���؂�ւ�鎞�ɕ\������p�l�� </summary>
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
