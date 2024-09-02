using System.Collections;
using UnityEngine;

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
