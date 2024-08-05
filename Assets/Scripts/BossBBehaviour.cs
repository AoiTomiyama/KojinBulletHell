using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossBBehaviour : BossBase
{
    [Header("ÉåÅ[ÉUÅ[ÇÃPrefab")]
    [SerializeField]
    protected private GameObject _laserLarge;
    private void Start()
    {
        _difficulty = PlayerPrefs.GetString("DIFF");
        _shield = transform.Find("Shield").gameObject;
        _shield.SetActive(false);
        _startPos = this.transform.position;
        _particleTr = this.transform.Find("ParticlePosition").transform;
        _bossCube = this.transform.Find("BossCube").gameObject;
        _pos = GameObject.Find("Positions").transform.GetComponentsInChildren<Transform>();
        _seAus = GetComponent<AudioSource>();
        _seAus.volume *= PlayerPrefs.GetFloat("SEVolume");
        _tweens.Add(
            _bossCube.transform.DORotate(new Vector3(Random.Range(-100, -200), Random.Range(300, 600), Random.Range(-100, -200)), 1.5f, RotateMode.FastBeyond360).
                SetLoops(-1, LoopType.Incremental).
                SetEase(Ease.Linear)
        );

        WanderingMove();
    }
    private void WanderingMove()
    {
        _tweens.Add(
            this.transform.DOMove(new Vector2(-_startPos.x, _startPos.y), 3).
                SetLoops(2, LoopType.Yoyo).
                SetEase(Ease.InOutQuad).
                OnComplete(() =>
                {
                    _bossCube.transform.DOPause();
                    Attack();
                }
                )
        );
    }
    public override void AttackPatternOne()
    {
        float duration = 0.55f;
        float count;
        _particlePattern = Instantiate(_particles[0], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        var emission = _particlePattern.emission;
        emission.enabled = false;
        var main = _particlePattern.main;
        int burstCount = (int)emission.GetBurst(0).count.constant;

        if (_difficulty == "expert")
        {
            count = 6;
        }
        else if (_difficulty == "ruthless")
        {
            count = 8;
        }
        else
        {
            count = 5;
        }
        _seq = DOTween.Sequence();
        _seq.Append(
            _bossCube.transform.DORotate(new Vector3(0, 720, 0), 0.5f, RotateMode.FastBeyond360).
            OnComplete(() => _particlePattern.Play())
            );
        while (count > 0)
        {
            _seq.Append(_bossCube.transform.DORotate(Vector3.up * 720, duration, RotateMode.FastBeyond360).
                SetEase(Ease.Linear).
                OnStart(() =>
                {
                    StartCoroutine(Flash());
                    transform.position = new Vector3(Random.Range(_pos[1].position.x, -_pos[1].position.x), _pos[1].position.y, this.transform.position.z);
                    _particlePattern.Emit(burstCount);
                }
                ));
            count--;
        }
        _seq.Append(this.transform.DOMove(_startPos, 0.5f).
            OnStart( () => _bossCube.transform.DOPlay()).
            OnComplete(() =>
            {
                emission.enabled = false;
                _particleTr.DetachChildren();
                Destroy(_particlePattern.gameObject, 3f);
                WanderingMove();
            }
        ));
    }
    public override void AttackPatternTwo()
    {
        float duration = 5f;
        _particlePattern = Instantiate(_particles[1], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        _particlePattern.Stop();
        var emission = _particlePattern.emission;
        float laserInterval = 1.2f;
        if (_difficulty == "expert")
        {
            laserInterval = 0.9f;
        }
        else if (_difficulty == "ruthless")
        {
            laserInterval = 0.5f;
        }
        float moveToPosTime = 0.5f;
        StartCoroutine(Flash());
        this.transform.position = _pos[2].position;
        _seq = DOTween.Sequence();
        _seq.Append(
            _bossCube.transform.DORotate(new Vector3(0, 0, 720), moveToPosTime, RotateMode.FastBeyond360)
            );
        _seq.Append(
            _bossCube.transform.DORotate(1800 * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InOutSine).
            OnStart(() => 
            {
                StartCoroutine(ShootLaser(duration, laserInterval));
                _particlePattern.Play();
            })
            );
        _seq.Append(this.transform.DOMove(_startPos, moveToPosTime).
            OnStart(() =>
            {
                emission.enabled = false;
                _particleTr.DetachChildren();
                Destroy(_particlePattern.gameObject, duration);
                _bossCube.transform.DOPlay();
            }
            ));
        _seq.Append(this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove()));
    }

    public override void AttackPatternThree()
    {
        float duration = 5f;
        _particlePattern = Instantiate(_particles[2], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        _particlePattern.Stop();
        var main = _particlePattern.main;
        var emission = _particlePattern.emission;
        if (_difficulty == "expert")
        {
            main.duration -= 0.1f;
        }
        else if (_difficulty == "ruthless")
        {
            main.duration -= 0.25f;
        }
        float moveToPosTime = 0.5f;

        StartCoroutine(Flash());
        this.transform.position = _pos[3].position;
        _seq = DOTween.Sequence();
        _seq.Append(_bossCube.transform.DORotate(new Vector3(0, 0, 720), moveToPosTime, RotateMode.FastBeyond360));
        _seq.Append(
            _bossCube.transform.DORotate(1800 * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InOutSine).
            OnStart(() => 
            {
                _tweens.Add(
                    this.transform.DOMove(new Vector2(-_pos[3].position.x, _pos[3].position.y), duration / 4).
                    SetLoops(4, LoopType.Yoyo).
                    SetEase(Ease.InOutQuad));
                _particlePattern.Play();
            })
            );
        _seq.Append(this.transform.DOMove(_startPos, moveToPosTime).
            OnStart(() =>
            {
                emission.enabled = false;
                _particleTr.DetachChildren();
                Destroy(_particlePattern.gameObject, duration);
                _bossCube.transform.DOPlay();
            }
            ));
        _seq.Append(this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove()));
    }


    public override void Death()
    {
        throw new System.NotImplementedException();
    }

    public override void PhaseSecondStart()
    {
        throw new System.NotImplementedException();
    }
    private IEnumerator Flash()
    {
        var flash = GameObject.Find("FlashPanel").GetComponent<Image>();
        flash.enabled = true;
        yield return new WaitForSeconds(0.1f);
        flash.enabled = false;
    }

    private IEnumerator ShootLaser(float duration, float interval)
    {
        for (float i = duration - interval; i > 0; i -= interval)
        {
            Instantiate(_laser, transform);
            yield return new WaitForSeconds(interval);
        }
    }
}
