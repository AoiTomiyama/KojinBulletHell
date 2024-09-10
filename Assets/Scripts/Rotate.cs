using System.Collections;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g��C�ӂ̑��x�E�p�x�ŉ�]������B
/// </summary>
public class Rotate : MonoBehaviour, IPausable
{
    [SerializeField, Header("��]���x")]
    private float _speed;

    [SerializeField, Header("�����_�������邩")]
    private bool _randomized;

    [SerializeField, Header("�����̍ő�l"), Range(0, 360)]
    private float _maxAngle = 360;

    [SerializeField, Header("�����̍ŏ��l"), Range(0, 360)]
    private float _minAngle = 0;

    /// <summary>�|�[�Y���ɒ��f����ׂ̕ϐ�</summary>
    private IEnumerator _coroutine;
    /// <summary>�|�[�Y�����ǂ���</summary>
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

            // �p�x���͈͊O�̏ꍇ�̏���
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
