using Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{

    private CinemachineImpulseSource _source;

    //このスクリプトをインスタンス化
    private void Awake()
    {
        _source = FindObjectOfType<CinemachineImpulseSource>();
    }

    /// <summary>
    /// カメラを揺らす
    /// </summary>
    /// <param name="force">振動の強さ</param>
    /// <param name="shakeFadeInTime">振動の増加時間</param>
    /// <param name="shakeDurationfloat">振動の継続時間</param>
    /// <param name="shakeFadeOutTime">振動の減衰時間</param>
    public void Shake(float force, float shakeFadeInTime, float shakeDurationfloat, float shakeFadeOutTime)
    {
        _source.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = shakeFadeInTime;
        _source.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = shakeDurationfloat;
        _source.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = shakeFadeOutTime;
        _source.GenerateImpulse(force);

    }
}
