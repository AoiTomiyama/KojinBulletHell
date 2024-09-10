using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour, IPausable
{

    private CinemachineImpulseSource _source;
    private float _duration;
    private float _timer;

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
        _duration = shakeFadeInTime + shakeDuration + shakeFadeOutTime;
        StartCoroutine(Wait());
        _source.GenerateImpulse(force);
    }
    private IEnumerator Wait()
    {
        _timer = 0f;
        while (_timer < _duration)
        {
            yield return null;
            _timer += Time.deltaTime;
        }
    }

    public void Pause()
    {
        CinemachineImpulseManager.Instance.Clear();
    }

    public void Resume() { }
}
