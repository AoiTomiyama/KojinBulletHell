using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �w��b�����Ɍ����������_���ȕ����֕ς���B
/// ParticleManager�ɓ����\��B
/// </summary>
public class RandomDirection : MonoBehaviour
{
    [Header("������ς������")]
    [SerializeField]
    float _waitTime;

    [Header("�����̍ő�l")]
    [SerializeField]
    float _maxAngular = 360;

    [Header("�����̍ŏ��l")]
    [SerializeField]
    float _minAngular = 0;

    private void Start()
    {
        StartCoroutine(RandomRotateRepeat());
    }

    IEnumerator RandomRotateRepeat()
    {
        while (true)
        {
            float randomZRotation = Random.Range(_minAngular, _maxAngular);
            transform.rotation = Quaternion.Euler(0f, 0f, randomZRotation);
            yield return new WaitForSeconds(_waitTime);
        }
    }
}
