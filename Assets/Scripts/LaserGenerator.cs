using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LaserGenerator : MonoBehaviour
{
    [SerializeField, Header("インターバル")]
    private float _interval;

    [SerializeField, Header("生成する個数")]
    private int _count;

    [SerializeField, Header("生成する位置の間隔")]
    private int _distance; 

    [SerializeField, Header("生成するレーザー")]
    private GameObject _laser;

    private BoxCollider2D _collider;
    private List<Vector2> _generatedPositions = new();
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        StartCoroutine(ShootEnumerator());
    }
    IEnumerator ShootEnumerator()
    {
        while (true)
        {
            _generatedPositions.Clear();
            for (int i = 0; i < _count; i++)
            {
                Vector2 currentPos = default;
                if (_generatedPositions.Count > 0)
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
                        Debug.LogWarning("有効な範囲が見つかりませんでした。デフォルト値を使用します。");
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
            }
            yield return new WaitForSeconds(_interval);
        }
    }
}
