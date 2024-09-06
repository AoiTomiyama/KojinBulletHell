using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    private bool _isPaused;
    /// <summary>スクリプトを使わないオブジェクトにボーズ機能を実装するためのUnityEvent</summary>
    public UnityEvent OnPause, OnResume;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (_isPaused)
            {
                ResumeAll();
            }
            else
            {
                PauseAll();
            }
            _isPaused = !_isPaused;
        }
    }

    /// <summary>ポーズ状態にする。</summary>
    private void PauseAll()
    {
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Pause());
        FindObjectsOfType<ParticleSystem>().ToList().ForEach(p => p.Pause());
        OnPause?.Invoke();
    }


    /// <summary>ポーズ状態を終了させる。</summary>
    public void ResumeAll()
    {
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Resume());
        FindObjectsOfType<ParticleSystem>().ToList().ForEach(p => p.Play());
        OnResume?.Invoke();
    }
}
public interface IPausable
{
    public void Pause();
    public void Resume();
}

