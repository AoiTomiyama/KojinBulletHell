using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// �t�F�[�h�C���ƃt�F�[�h�A�E�g���s���X�N���v�g�B
/// </summary>
public class FadeInOut : MonoBehaviour
{
    [SerializeField, Header("�J�n���Ƀt�F�[�h�A�E�g�����邩")]
    private bool _isStartFadeOut = true;
    /// <summary>�A�j���[�^�[�̃R���|�[�l���g</summary>
    private Animator _anim;
    /// <summary>�t�F�[�h�C���I����ɃV�[���J�ڂ��������߁A�ꎞ�I�ɃV�[������ۑ�</summary>
    private string _sceneName;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        if (_isStartFadeOut) _anim.Play("FadeOut");
    }
    /// <summary>
    /// �t�F�[�h�C����A�V�[�����ړ�����B
    /// </summary>
    /// <param name="sceneName">�ړ���̃V�[����</param>
    public void FadeInAndChangeScene(string sceneName)
    {
        if (_sceneName != null) return;
        _anim.Play("FadeIn");
        _sceneName = sceneName;
    }
    /// <summary>
    /// �A�j���[�V�����C�x���g�p�̃��\�b�h�B
    /// </summary>
    void ChangeScene() { SceneManager.LoadScene(_sceneName); }
}
