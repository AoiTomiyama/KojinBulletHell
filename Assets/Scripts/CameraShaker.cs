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
    /// ƒJƒƒ‰‚ğ—h‚ç‚·
    /// </summary>
    /// <param name="force">U“®‚Ì‹­‚³</param>
    /// <param name="shakeFadeInTime">U“®‚Ì‘‰ÁŠÔ</param>
    /// <param name="shakeDuration">U“®‚ÌŒp‘±ŠÔ</param>
    /// <param name="shakeFadeOutTime">U“®‚ÌŒ¸ŠŠÔ</param>
    public void Shake(float force, float shakeFadeInTime, float shakeDuration, float shakeFadeOutTime)
    {
        _source.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = shakeFadeInTime;
        _source.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = shakeDuration;
        _source.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = shakeFadeOutTime;
        _source.GenerateImpulse(force);
    }
}
