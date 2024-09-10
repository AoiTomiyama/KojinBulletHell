using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    private bool _enablePause = true;
    private bool _isPaused;
    /// <summary>�X�N���v�g�𔺂�Ȃ��I�u�W�F�N�g�Ƀ{�[�Y�@�\���������邽�߂�UnityEvent</summary>
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

    /// <summary>�|�[�Y��Ԃɂ���B</summary>
    private void PauseAll()
    {
        //IPausable�C���^�[�t�F�C�X���p�����Ă���I�u�W�F�N�g��Ώۂɏ����B
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Pause());

        //�w�i�Ȃǂ̃p�[�e�B�N�����܂Ƃ߂Ď~�߂邽��ParticleSystem�͕ʓr��������B
        FindObjectsOfType<ParticleSystem>().ToList().ForEach(p => p.Pause());

        OnPause?.Invoke();

        _isPaused = !_isPaused;
    }


    /// <summary>�|�[�Y��Ԃ��I��������B</summary>
    public void ResumeAll()
    {
        //IPausable�C���^�[�t�F�C�X���p�����Ă���I�u�W�F�N�g��Ώۂɏ����B
        FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList().ForEach(p => p.Resume());

        //�w�i�Ȃǂ̃p�[�e�B�N�����܂Ƃ߂Ď~�߂邽��ParticleSystem�͕ʓr��������B
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

