using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    private bool _enablePause = true;
    private bool _isPaused;
    /// <summary>スクリプトを伴わないオブジェクトにボーズ機能を実装するためのUnityEvent</summary>
    public UnityEvent OnPause, OnResume;

    public bool EnablePause { get => _enablePause; set => _enablePause = value; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && _enablePause)
        {
            if (_isPaused)
            {
                ResumeAll();
            }
            else
            {
                PauseAll();
            }
        }
    }

    /// <summary>ポーズ状態にする。</summary>
    private void PauseAll()
    {
        //IPausableインターフェイスを継承しているオブジェクトを対象に処理。
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Pause());

        //背景などのパーティクルもまとめて止めるためParticleSystemは別途処理する。
        FindObjectsOfType<ParticleSystem>().ToList().ForEach(p => p.Pause());

        OnPause?.Invoke();

        _isPaused = !_isPaused;
    }


    /// <summary>ポーズ状態を終了させる。</summary>
    public void ResumeAll()
    {
        //IPausableインターフェイスを継承しているオブジェクトを対象に処理。
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Resume());

        //背景などのパーティクルもまとめて止めるためParticleSystemは別途処理する。
        FindObjectsOfType<ParticleSystem>().ToList().ForEach(p => p.Play());

        OnResume?.Invoke();

        _isPaused = !_isPaused;
    }
}
public interface IPausable
{
    public void Pause();
    public void Resume();
}

