using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���[�U�[�r�[����f���I�ɐ�������X�N���v�g
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class LaserGenerator : MonoBehaviour, IPausable
{
    [SerializeField, Header("�C���^�[�o��")]
    private float _interval;

    [SerializeField, Header("���������")]
    private int _count;

    [SerializeField, Header("��������ʒu�̊Ԋu")]
    private int _distance; 

    [SerializeField, Header("�������郌�[�U�[")]
    private GameObject _laser;

    /// <summary>�����͈͂����߂�BoxCollider</summary>
    private BoxCollider2D _collider;
    /// <summary>�|�[�Y���ɐ����𒆒f����ׂ̕ϐ�</summary>
    private IEnumerator _coroutine;
    /// <summary>�����ʒu���d�Ȃ�Ȃ��悤�ɁA�����ʒu���L�����Ă����ϐ�</summary>
    private readonly List<Vector2> _generatedPositions = new();
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _coroutine = ShootEnumerator();
        StartCoroutine(_coroutine);
    }
    IEnumerator ShootEnumerator()
    {
        while (true)
        {
            _generatedPositions.Clear();
            for (int i = 0; i < _count; i++)
            {
                Vector2 currentPos = default;
                if (_generatedPositions.Count > 0 && _distance > 0)
                {
                    int attempts = 0;
                    int maxAttempt = 50;
                    while (attempts < maxAttempt)
                    {
                        attempts++;
                        currentPos = new Vector2(
                            Random.Range(_collider.bounds.extents.x, -_collider.bounds.extents.x) + transform.position.x,
                            Random.Range(_collider.bounds.extents.y, -_collider.bounds.extents.y) + transform.position.y);

                        bool isValid = true;
                        foreach (Vector2 pos in _generatedPositions)
                        {
                            if (Vector2.Distance(currentPos, pos) < _distance)
                            {
                                isValid = false;
                                break;
                            }
                        }

                        if (isValid)
                        {
                            break;
                        }
                    }

                    if (attempts >= maxAttempt)
                    {
                        Debug.LogWarning("�L���Ȕ͈͂�������܂���ł����B�f�t�H���g�l���g�p���܂��B");
                        currentPos = transform.position;
                    }
                }
                else
                {
                    currentPos = new Vector2(
                        Random.Range(_collider.bounds.extents.x, -_collider.bounds.extents.x) + transform.position.x,
                        Random.Range(_collider.bounds.extents.y, -_collider.bounds.extents.y) + transform.position.y);
                }
                _generatedPositions.Add(currentPos);
                Instantiate(_laser, currentPos, transform.rotation, transform.parent);

                var timer = 0f;
                while (timer < _interval)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }

    public void Pause()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
    }

    public void Resume()
    {
        if (_coroutine != null) StartCoroutine(_coroutine);
    }
}
