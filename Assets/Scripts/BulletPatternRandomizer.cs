using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �e���p�^�[����z�񂩂烉���_���ɑI�o���A��������X�N���v�g�B
/// </summary>
public class BulletPatternRandomizer : MonoBehaviour
{
    [SerializeField, Header("�ǂ̓�Փx�ŗL���ɂ��邩�ǂ���")]
    private bool _enableAtNormal;
    [SerializeField]
    private bool _enableAtExpert;
    [SerializeField]
    private bool _enableAtRuthless;

    [SerializeField, Header("�ҋ@�b��")]
    float _waitSeconds = 5;

    [SerializeField, Header("�e���̃p�^�[��")]
    private GameObject[] _patterns;

    /// <summary> ���݂̒e���p�^�[����ۑ��B </summary>
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
