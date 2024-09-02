using DG.Tweening;
using System.Collections;
using UnityEngine;
/// <summary>
/// ボスCの動き
/// </summary>

public class BossCBehaviour : BossBase
{
    [SerializeField, Header("第二形態時の攻撃")]
    private GameObject _secondPhaseLaserAttack;

    private ParticleSystem _subParticlePattern;

    private void Start()
    {
        _flashEffector = FindObjectOfType<FlashEffect>();
        _difficulty = (Enums.Difficulties)PlayerPrefs.GetInt("DIFF_INT");
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
        float duration = 8.5f;
        _particlePattern = Instantiate(_particles[0], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        var emission = _particlePattern.emission;
        var main = _particlePattern.main;
        _particlePattern.Stop();

        if (_difficulty != Enums.Difficulties.Normal)
        {
            var burst = emission.GetBurst(0);
            var burstCount = burst.count;
            if (_difficulty == Enums.Difficulties.Expert)
            {
                burstCount.constant += 1;
            }
            else if (_difficulty == Enums.Difficulties.Ruthless)
            {
                burstCount.constant += 2;
            }
            burst.count = burstCount;
            emission.SetBurst(0, burst);
        }

        const float moveToPosTime = 0.5f;
        _seq = DOTween.Sequence();
        _seq.Append(
            _bossCube.transform.DORotate(new Vector3(0, 720, 0), moveToPosTime, RotateMode.FastBeyond360).
            OnComplete(() => _particlePattern.Play()).
            OnStart(() => this.transform.DOMove(_pos[1].position, moveToPosTime).
            SetEase(Ease.InOutSine))
            );
        _seq.Append(_bossCube.transform.DORotate(Vector3.up * 720, duration, RotateMode.FastBeyond360).
            SetEase(Ease.Linear)
            );
        _seq.Append(this.transform.DOMove(_startPos, moveToPosTime).
            OnStart(() =>
            {
                _bossCube.transform.DOPlay();
                emission.enabled = false;
                _particleTr.DetachChildren();
                Destroy(_particlePattern.gameObject, duration);
            }).
            OnComplete(() => WanderingMove())
            );
    }
    public override void AttackPatternTwo()
    {
        float duration = 8.5f;
        _particlePattern = Instantiate(_particles[1], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        _subParticlePattern = Instantiate(_particles[2], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();

        _particlePattern.Stop();
        _subParticlePattern.Stop();

        var emission = _particlePattern.emission;
        if (_difficulty != Enums.Difficulties.Normal)
        {
            var burst = emission.GetBurst(0);
            var burstCount = burst.count;
            if (_difficulty == Enums.Difficulties.Expert)
            {
                burstCount.constant += 2;
            }
            else if (_difficulty == Enums.Difficulties.Ruthless)
            {
                burstCount.constant += 4;
            }
            burst.count = burstCount;
            emission.SetBurst(0, burst);
        }

        float moveToPosTime = 0.5f;
        _seq = DOTween.Sequence();
        _seq.Append(
            _bossCube.transform.DORotate(new Vector3(0, 0, 720), moveToPosTime, RotateMode.FastBeyond360).
            OnStart(() => this.transform.DOMove(_pos[2].position, moveToPosTime)).
            SetEase(Ease.InOutSine)
            );
        _seq.Append(
            _bossCube.transform.DORotate(1800 * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InOutSine).
            OnStart(() =>
            {
                _particlePattern.Play();
                _subParticlePattern.Play();
            })
            );
        _seq.Append(this.transform.DOMove(_startPos, moveToPosTime).
            OnStart(() =>
            {
                var subParticleEmission = _subParticlePattern.emission;
                subParticleEmission.enabled = false;
                emission.enabled = false;
                _particleTr.DetachChildren();
                Destroy(_particlePattern.gameObject, duration);
                _bossCube.transform.DOPlay();
            }).
            OnComplete(() => WanderingMove())
            );
    }

    public override void AttackPatternThree()
    {
        float duration = 5f;
        _particlePattern = Instantiate(_particles[3], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        _particlePattern.Stop();
        var main = _particlePattern.main;

        var emission = _particlePattern.emission;
        if (_difficulty == Enums.Difficulties.Expert)
        {
            emission.rateOverTimeMultiplier *= 1.75f;
        }
        else if (_difficulty == Enums.Difficulties.Ruthless)
        {
            emission.rateOverTimeMultiplier *= 2.5f;
        }

        float moveToPosTime = 0.5f;
        _seq = DOTween.Sequence();
        _seq.Append(
            _bossCube.transform.DORotate(new Vector3(0, 0, 720), moveToPosTime, RotateMode.FastBeyond360).
            OnStart(() => this.transform.DOMove(_pos[3].position, moveToPosTime).
            SetEase(Ease.InOutSine))
            );
        _seq.Append(
            _bossCube.transform.DORotate(1800 * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InOutSine).
            OnStart(() => _particlePattern.Play())
            );

        _seq.Append(this.transform.DOMove(_startPos, moveToPosTime).
            OnStart(() =>
            {
                emission.enabled = false;
                _particleTr.DetachChildren();
                Destroy(_particlePattern.gameObject, duration);
                _bossCube.transform.DOPlay();
            }).
            OnComplete(() => WanderingMove())
            );
    }

    public override void PhaseSecondStart()
    {
        Debug.Log("Phase 2 Start");

        //画面を光らせ、全体の色を変える。
        _effectImage.enabled = true;
        _flashEffector.Flash();

        //実行中のTweenを破棄または停止
        _seq?.Kill();
        this.transform.DOKill();
        _bossCube.transform.DOPause();

        //シールドを展開し、場所移動。
        _shield.SetActive(true);
        this.transform.position = _pos[4].position;
        _bossCube.transform.rotation = Quaternion.Euler(0, 0, 90);

        //既存の弾幕を破棄
        if (_particlePattern != null)
        {
            Destroy(_particlePattern.gameObject);
        }
        if (_subParticlePattern != null)
        {
            Destroy(_subParticlePattern.gameObject);
        }

        //第二形態の攻撃を生成
        var secondAttack = Instantiate(_secondPhaseLaserAttack);
        ParticleSystem.EmissionModule emission;

        if (_difficulty == Enums.Difficulties.Normal)
        {
            secondAttack.transform.Find("InactiveOnNormal").gameObject.SetActive(false);
        }
        if (_difficulty == Enums.Difficulties.Ruthless)
        {
            _particlePattern = Instantiate(_particles[4], _pos[4].position, Quaternion.identity).GetComponent<ParticleSystem>();
            emission = _particlePattern.emission;
        }

        //シールド展開時間
        float shieldDuration = 15f;
        if (_difficulty == Enums.Difficulties.Expert)
        {
            shieldDuration = 20f;
        }
        else if (_difficulty == Enums.Difficulties.Ruthless)
        {
            shieldDuration = 25f;
        }

        //展開時間だけ回転し、完了したら攻撃を破棄して通常周期に戻る。
        _tweens.Add(_bossCube.transform.DORotate(new Vector3(Random.Range(-3600, 3600), Random.Range(-3600, 3600), Random.Range(-3600, 3600)), shieldDuration, RotateMode.FastBeyond360).
            SetEase(Ease.Linear).
            OnComplete(() =>
            {
                if (_difficulty == Enums.Difficulties.Ruthless)
                {
                    emission.enabled = false;
                    Destroy(_particlePattern.gameObject, 5);
                }

                Destroy(secondAttack);
                _effectImage.enabled = false;
                _flashEffector.Flash();
                _shield.SetActive(false);
                _bossCube.transform.DOPlay();
                this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove());
            }
            ));
    }
    public override void FinalAttack()
    {
        _effectImage.enabled = true;
        _flashEffector.Flash();
        _seq?.Kill();
        _bossCube.transform.DOPause();
        this.transform.DOKill();
        _shield.SetActive(true);
        this.transform.position = _pos[5].position;
        _bossCube.transform.rotation = Quaternion.Euler(0, 0, 90);

    }

    public override void Death()
    {
        float duration = 4f;
        GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
        FindObjectOfType<GameManager>().IsTimeStop = true;
        _seAus.PlayOneShot(_deathChargeSE);
        _seq?.Kill();
        transform.DOKill();
        _bossCube.transform.DOKill();
        this.transform.position = new Vector2(0, _startPos.y);
        Destroy(_particleTr.gameObject);
        _flashEffector.Flash();
        _bossCube.transform.DORotate(360 * Random.Range(4.6f, 5.1f) * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InExpo).
            OnComplete(() => StartCoroutine(Explode()));
        StartCoroutine(FindObjectOfType<LightRay>().EmitLightRay(duration, 10));
    }
    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.2f);
        FindObjectOfType<CameraShaker>().Shake(3, 0, 1, 0.4f);
        _flashEffector.Flash();
        _bossCube.SetActive(false);
        FindObjectOfType<LightRay>().gameObject.SetActive(false);
        _seAus.PlayOneShot(_deathExplodeSE);
        Instantiate(_explodePrefab, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2.5f);
        FindObjectOfType<FadeInOut>().FadeInAndChangeScene("StageClear");
    }
}
