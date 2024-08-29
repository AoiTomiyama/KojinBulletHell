using Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{

    private CinemachineImpulseSource _source;

    private void Awake()
    {
        _source = FindObjectOfType<CinemachineImpulseSource>();
    }

    /// <summary>
    /// �J������h�炷
    /// </summary>
    /// <param name="force">�U���̋���</param>
    /// <param name="shakeFadeInTime">�U���̑�������</param>
    /// <param name="shakeDurationfloat">�U���̌p������</param>
    /// <param name="shakeFadeOutTime">�U���̌�������</param>
    public void Shake(float force, float shakeFadeInTime, float shakeDurationfloat, float shakeFadeOutTime)
    {
        _source.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = shakeFadeInTime;
        _source.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = shakeDurationfloat;
        _source.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = shakeFadeOutTime;
        _source.GenerateImpulse(force);

    }
}
