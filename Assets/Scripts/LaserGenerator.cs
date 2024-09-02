using System.Collections;
using UnityEngine;

public class LaserGenerator : MonoBehaviour
{
    [SerializeField, Header("�C���^�[�o��")]
    private float _interval;

    [SerializeField, Header("���������")]
    private int _count;

    [SerializeField, Header("�������郌�[�U�[")]
    private GameObject _laser;

    private BoxCollider2D _collider;
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        StartCoroutine(ShootEnumerator());
    }
    IEnumerator ShootEnumerator()
    {
        while (true)
        {
            for (int i = 0; i < _count; i++)
            {
                var pos = new Vector2(
                    Random.Range(_collider.bounds.extents.x, -_collider.bounds.extents.x) + transform.position.x,
                    Random.Range(_collider.bounds.extents.y, -_collider.bounds.extents.y) + transform.position.y);
                Instantiate(_laser, pos, transform.rotation, transform.parent);
            }
            yield return new WaitForSeconds(_interval);
        }
    }
}
