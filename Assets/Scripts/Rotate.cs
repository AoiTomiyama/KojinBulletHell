using System.Collections;
using UnityEngine;

/// <summary>
/// オブジェクトを任意の速度・角度で回転させる。
/// </summary>
public class Rotate : MonoBehaviour
{
    [SerializeField, Header("回転速度")]
    private float _speed;

    [SerializeField, Header("ランダム化するか")]
    private bool _randomized;

    [SerializeField, Header("方向の最大値"),Range(0, 360)]
    private float _maxAngle = 360;

    [SerializeField, Header("方向の最小値"),Range(0, 360)]
    private float _minAngle = 0;

    private void Start()
    {
        if (_randomized)
        {
            StartCoroutine(RandomizedRotation());
        }
    }
    void FixedUpdate()
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
        Debug.Log(transform.eulerAngles.z);
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
}
