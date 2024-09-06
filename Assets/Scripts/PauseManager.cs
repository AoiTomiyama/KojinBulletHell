using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    private bool _isPaused;
    /// <summary>�X�N���v�g���g��Ȃ��I�u�W�F�N�g�Ƀ{�[�Y�@�\���������邽�߂�UnityEvent</summary>
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

    /// <summary>�|�[�Y��Ԃɂ���B</summary>
    private void PauseAll()
    {
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Pause());
        FindObjectsOfType<ParticleSystem>().ToList().ForEach(p => p.Pause());
        OnPause?.Invoke();
    }


    /// <summary>�|�[�Y��Ԃ��I��������B</summary>
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

