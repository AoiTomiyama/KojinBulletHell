using System.Collections;
using UnityEngine;
/// <summary>
/// 敵死亡時に出す光線の制御
/// </summary>
public class LightRay : MonoBehaviour
{
    [SerializeField, Header("光線")]
    private GameObject _ray;
    /// <summary>
    /// 敵が死んだときなどに出る光の漏出を発生させる
    /// </summary>
    /// <param name="duration">光を出し切るまでにかかる時間。</param>
    /// <param name="count">出す光の本数</param>
    public IEnumerator EmitLightRay(float duration, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(_ray, this.transform.position, Quaternion.Euler(Vector3.forward * Random.Range(0, 360)), transform);
            yield return new WaitForSeconds(duration / count);
        }
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 10 * Time.time);
    }
}
