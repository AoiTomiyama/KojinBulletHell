using System.Collections;
using UnityEngine;

public class LightRay : MonoBehaviour
{
    [Header("����")]
    [SerializeField]
    GameObject _ray;
    /// <summary>
    /// �G�����񂾂Ƃ��Ȃǂɏo����̘R�o�𔭐�������
    /// </summary>
    /// <param name="duration">�����o���؂�܂łɂ����鎞�ԁB</param>
    /// <param name="count">�o�����̖{��</param>
    public IEnumerator EmitLightRay(float duration, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(_ray, this.transform.position, Quaternion.Euler(Vector3.forward * Random.Range(0, 360)), transform);
            yield return new WaitForSeconds(duration / count);
        }
    }
    private void FixedUpdate()
    {
        this.transform.rotation = Quaternion.Euler(10 * Time.time * Vector3.forward);
    }
}
