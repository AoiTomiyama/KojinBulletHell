using System.Collections;
using UnityEngine;

/// <summary>
/// オブジェクトを任意の速度・角度で回転させる。
/// </summary>
public class Rotate : MonoBehaviour, IPausable
{
    [SerializeField, Header("回転速度")]
    private float _speed;

    [SerializeField, Header("ランダム化するか")]
    private bool _randomized;

    [SerializeField, Header("方向の最大値"), Range(0, 360)]
    private float _maxAngle = 360;

    [SerializeField, Header("方向の最小値"), Range(0, 360)]
    private float _minAngle = 0;

    /// <summary>ポーズ時に中断する為の変数</summary>
    private IEnumerator _coroutine;
    /// <summary>ポーズ中かどうか</summary>
    private bool _isPaused;


    private void Start()
    {
        if (_randomized)
        {
            _coroutine = RandomizedRotation();
            StartCoroutine(_coroutine);
        }
    }
    void FixedUpdate()
    {
        if (!_isPaused)
        {
            float newZAngle = transform.eulerAngles.z + _speed;

            // 角度が範囲外の場合の処理
            if (newZAngle < _minAngle)
            {
                newZAngle = _maxAngle - (_minAngle - newZAngle);
            }
            else if (newZAngle > _maxAngle)
            {
                newZAngle = _minAngle + (newZAngle - _maxAngle);
            }

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZAngle);
        }
    }
    IEnumerator RandomizedRotation()
    {
        while (true)
        {
            float randomZRotation = Random.Range(_minAngle, _maxAngle);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, randomZRotation);
            yield return new WaitForSeconds(_speed);
        }
    }

    public void Pause()
    {
        _isPaused = true;
        if (_coroutine != null) StopCoroutine(_coroutine);
    }

    public void Resume()
    {
        _isPaused = false;
        if (_coroutine != null) StartCoroutine(_coroutine);
    }
}
