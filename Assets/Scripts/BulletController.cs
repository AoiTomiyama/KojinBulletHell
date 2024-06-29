using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("’e‘¬")]
    [SerializeField]
    float _bulletSpeed;
    private void FixedUpdate()
    {
        transform.position += Vector3.right * transform.localScale.x * _bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Wall"))
        {
            Debug.Log("HIT");
            FindObjectOfType<PlayerControl>().OnBulletHit(this.transform.position);
            Destroy(this.gameObject);
        }
    }
}
