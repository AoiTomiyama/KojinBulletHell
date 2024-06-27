using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定秒数毎に向きをランダムな方向へ変える。
/// ParticleManagerに統合予定。
/// </summary>
public class RandomDirection : MonoBehaviour
{
    [Header("方向を変える周期")]
    [SerializeField]
    float _waitTime;

    [Header("方向の最大値")]
    [SerializeField]
    float _maxAngular = 360;

    [Header("方向の最小値")]
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
