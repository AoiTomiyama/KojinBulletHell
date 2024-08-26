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
            StartCoroutine(RamdomizedRotation());
        }
    }
    void FixedUpdate()
    {
        if (!_randomized)
        {
            if (transform.eulerAngles.z + _speed < _minAngle)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - _minAngle + _maxAngle);
            }
            else if (transform.eulerAngles.z + _speed > _maxAngle)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z % _maxAngle + _minAngle);
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + _speed);
            Debug.Log(transform.eulerAngles.z);
        }
    }
    IEnumerator RamdomizedRotation()
    {
        while (true)
        {
            float randomZRotation = Random.Range(_minAngle, _maxAngle);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, randomZRotation);
            yield return new WaitForSeconds(_speed);
        }
    }
}
