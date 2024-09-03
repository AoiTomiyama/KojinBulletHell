using DG.Tweening;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// ボスAの動き
/// </summary>
public class BossABehaviour : BossBase
{
    private void Start()
    {
        _flashEffector = FindObjectOfType<FlashEffect>();
        _difficulty = (Enums.Difficulties)PlayerPrefs.GetInt("DIFF_INT");
        _shield = GameObject.FindWithTag("Shield");
        _shield.SetActive(false);
        _startPos = this.transform.position;
        _particleTr = this.transform.Find("ParticlePosition").transform;
        _bossCube = this.transform.Find("BossCube").gameObject;
        _pos = GameObject.Find("Positions").transform.GetComponentsInChildren<Transform>();
        _seAus = GetComponent<AudioSource>();
        _seAus.volume *= PlayerPrefs.GetFloat("SEVolume");
        _tweens.Add(
            _bossCube.transform.DORotate(new Vector3(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200)), 1.5f, RotateMode.FastBeyond360).
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
            })
        );
    }
    public override void AttackPatternOne()
    {
        float duration = 6.5f;
        _particlePattern = Instantiate(_particles[0], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        _particlePattern.Stop();
        var emission = _particlePattern.emission;
        var main = _particlePattern.main;

        if (_difficulty == Enums.Difficulties.Expert)
        {
            main.duration = 0.6f;
        }
        else if (_difficulty == Enums.Difficulties.Expert)
        {
            main.duration = 0.5f;
        }
        else
        {
            main.duration = 0.75f;
        }

        const float moveToPosTime = 0.5f;
        _seq = DOTween.Sequence();
        _seq.Append(
            _bossCube.transform.DORotate(new Vector3(0, 720, 0), moveToPosTime, RotateMode.FastBeyond360).
            OnStart(() => _tweens.Add(this.transform.DOMove(_pos[1].position, moveToPosTime)))
        );
        _seq.Append(
            this.transform.DOMove(new Vector2(-_pos[1].position.x, _pos[1].position.y), duration).
            SetEase(Ease.InOutSine).
            OnStart(() =>
            {
                _particlePattern.Play();
                _tweens.Add(_bossCube.transform.DORotate(new Vector3(0, 360 * 5, 360), duration, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutSine));
            }).
            OnComplete(() =>
            {
                emission.enabled = false;
                _particleTr.DetachChildren();
                Destroy(_particlePattern.gameObject, duration);
                _bossCube.transform.DOPlay();
            })
        );
        _seq.Append(this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove()));
    }

    public override void AttackPatternTwo()
    {
        int burstCount = 4;
        if (_difficulty == Enums.Difficulties.Expert)
        {
            burstCount += 2;
        }
        else if (_difficulty == Enums.Difficulties.Ruthless)
        {
            burstCount += 4;
        }
        float moveToPosTime = 0.5f;
        _particlePattern = Instantiate(_particles[1], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        var emission = _particlePattern.emission;
        float emitCount = emission.GetBurst(0).count.constant;
        emission.enabled = false;
        _seq = DOTween.Sequence();
        _seq.Append(
            _bossCube.transform.DORotate(new Vector3(720, 0, 0), moveToPosTime, RotateMode.FastBeyond360).
            OnStart(() => _tweens.Add(this.transform.DOMove(_pos[2].position, moveToPosTime)))
        );
        for (int i = burstCount; i > 0; i--)
        {
            if (i % 2 == 0)
            {
                _seq.Append(
                    transform.DOMoveX(-_pos[2].position.x, 1).
                    OnStart(() =>
                    {
                        _particlePattern.Emit((int)emitCount);
                        _tweens.Add(_bossCube.transform.DORotate(new Vector3(0, 720, 720), 1, RotateMode.FastBeyond360).
                        SetEase(Ease.OutQuad));
                    })
                );
            }
            else
            {
                _seq.Append(
                    transform.DOMoveX(_pos[2].position.x, 1).
                    OnStart(() =>
                    {
                        _particlePattern.Emit((int)emitCount);
                        _tweens.Add(_bossCube.transform.DORotate(new Vector3(0, -720, -720), 1, RotateMode.FastBeyond360).
                        SetEase(Ease.OutQuad));
                    })
                );
            }
        }
        _seq.Append(
            this.transform.DOMove(_startPos, moveToPosTime).
            OnStart(() =>
            {
                _particleTr.DetachChildren();
                Destroy(_particlePattern.gameObject, burstCount);
                _bossCube.transform.DOPlay();
            }).
            OnComplete(() => WanderingMove())
        );
    }
    public override void AttackPatternThree()
    {
        float duration = 7f;
        _particlePattern = Instantiate(_particles[2], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        _particlePattern.Stop();
        var emission = _particlePattern.emission;
        if (_difficulty == Enums.Difficulties.Expert)
        {
            emission.rateOverTimeMultiplier *= 2.5f;
        }
        else if (_difficulty == Enums.Difficulties.Ruthless)
        {
            emission.rateOverTimeMultiplier *= 3f;
        }
        else
        {
            emission.rateOverTimeMultiplier *= 2f;
        }
        float moveToPosTime = 0.5f;
        _seq = DOTween.Sequence();
        _seq.Append(
            _bossCube.transform.DORotate(new Vector3(0, 0, 720), moveToPosTime, RotateMode.FastBeyond360).
            OnStart(() => _tweens.Add(this.transform.DOMove(_pos[3].position, moveToPosTime)))
        );
        _seq.Append(
            _bossCube.transform.DORotate(18000 * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InOutSine).
            OnStart(() => _particlePattern.Play())
        );
        _seq.Append(
            this.transform.DOMove(_startPos, moveToPosTime).
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
        this.transform.position = _pos[3].position;
        _bossCube.transform.rotation = Quaternion.Euler(0, 0, 90);

        //既存の弾幕を破棄
        if (_particlePattern != null)
        {
            Destroy(_particlePattern.gameObject);
        }

        //第二形態の攻撃を生成
        _particlePattern = Instantiate(_particles[3], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        var emission = _particlePattern.emission;

        //シールド展開時間
        float shieldDuration = 10f;
        if (_difficulty == Enums.Difficulties.Expert)
        {
            shieldDuration *= 1.5f;
        }
        else if (_difficulty == Enums.Difficulties.Ruthless)
        {
            shieldDuration *= 2f;
        }

        //レーザーの生成
        float laserInterval = 1.2f;
        if (_difficulty == Enums.Difficulties.Expert)
        {
            laserInterval = 1f;
        }
        else if (_difficulty == Enums.Difficulties.Ruthless)
        {
            laserInterval = 0.8f;
        }
        StartCoroutine(ShootLaser(shieldDuration, laserInterval));

        //展開時間だけ回転し、完了したら攻撃を破棄して通常周期に戻る。
        _tweens.Add(
            _bossCube.transform.DORotate(new Vector3(Random.Range(-3600, 3600), Random.Range(-3600, 3600), Random.Range(-3600, 3600)), shieldDuration, RotateMode.FastBeyond360).
            SetEase(Ease.Linear).
            OnComplete(() =>
            {
                emission.enabled = false;
                _effectImage.enabled = false;
                foreach (var go in transform.GetComponentsInChildren<LaserBeam>())
                {
                    Destroy(go.gameObject);
                }
                _flashEffector.Flash();
                _shield.SetActive(false);
                _bossCube.transform.DOPlay();
                this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove());
            })
        );
    }
    public override void FinalAttack()
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

        var finalAttack = Instantiate(_finalAttack);

        //シールド展開時間
        float shieldDuration = 30f;
        FindObjectOfType<CameraShaker>().Shake(0.5f, 0, shieldDuration, 0);

        //展開時間だけ回転する。
        _tweens.Add(
            _bossCube.transform.DORotate(new Vector3(0, 3600, 3600), shieldDuration, RotateMode.FastBeyond360).
            SetEase(Ease.Linear).
            OnComplete(() =>
            {
                Destroy(finalAttack);
                _effectImage.enabled = false;
                _shield.SetActive(false);
                _flashEffector.Flash();
                _tweens.Add(
                    _bossCube.transform.DORotate(new Vector3(0, 180, 180), 5, RotateMode.FastBeyond360).
                    SetEase(Ease.Linear).
                    SetLoops(-1, LoopType.Incremental)
                );
            })
        );
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
        _tweens.Add(
            _bossCube.transform.DORotate(360 * Random.Range(4.6f, 5.1f) * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InExpo).
            OnComplete(() => StartCoroutine(Explode())
            )
        );
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
    private IEnumerator ShootLaser(float duration, float interval)
    {
        for (float i = duration; i > 0; i -= interval)
        {
            Instantiate(_laser, transform);
            yield return new WaitForSeconds(interval);
        }
    }
}
