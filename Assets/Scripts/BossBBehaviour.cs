using DG.Tweening;
using System.Collections;
using UnityEngine;
/// <summary>
/// ボスBの動き
/// </summary>

public class BossBBehaviour : BossBase
{
    [SerializeField, Header("レーザー（大）のPrefab")]
    private GameObject _laserLarge;

    private void Start()
    {
        OnStartSetUp();
    }
    public override void AttackPatternOne()
    {
        const float duration = 0.55f;
        float count;
        _particlePattern = Instantiate(_particles[0], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        var emission = _particlePattern.emission;
        emission.enabled = false;
        var main = _particlePattern.main;
        int burstCount = (int)emission.GetBurst(0).count.constant;

        if (_difficulty == Enums.Difficulties.Expert)
        {
            count = 6;
        }
        else if (_difficulty == Enums.Difficulties.Ruthless)
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
        for (; count > 0; count--)
        {
            _seq.Append(
                _bossCube.transform.DORotate(Vector3.up * 720, duration, RotateMode.FastBeyond360).
                SetEase(Ease.Linear).
                OnStart(() =>
                {
                    _flashEffector.Flash();
                    transform.position = new Vector3(Random.Range(_pos[1].position.x, -_pos[1].position.x), _pos[1].position.y, this.transform.position.z);
                    _particlePattern.Emit(burstCount);
                })
            );
        }
        _seq.Append(
            this.transform.DOMove(_startPos, 0.5f).
            OnStart(() => _bossCube.transform.DOPlay()).
            OnComplete(() =>
            {
                emission.enabled = false;
                _particleTr.DetachChildren();
                Destroy(_particlePattern.gameObject, 3f);
                WanderingMove();
            })
        );
    }
    public override void AttackPatternTwo()
    {
        const float duration = 7f;
        _particlePattern = Instantiate(_particles[1], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        _particlePattern.Stop();
        var emission = _particlePattern.emission;
        float laserInterval = 1.2f;
        if (_difficulty == Enums.Difficulties.Expert)
        {
            laserInterval = 0.9f;
        }
        else if (_difficulty == Enums.Difficulties.Ruthless)
        {
            laserInterval = 0.5f;
        }
        float moveToPosTime = 0.5f;
        _flashEffector.Flash();
        this.transform.position = _pos[2].position;
        _seq = DOTween.Sequence();
        _seq.Append(_bossCube.transform.DORotate(new Vector3(0, 0, 720), moveToPosTime, RotateMode.FastBeyond360));
        _seq.Append(
            _bossCube.transform.DORotate(1800 * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InOutSine).
            OnStart(() =>
            {
                StartCoroutine(ShootLaser(duration, laserInterval));
                _particlePattern.Play();
            })
        );
        _seq.Append(
            this.transform.DOMove(_startPos, moveToPosTime).
            OnStart(() =>
            {
                emission.enabled = false;
                _particleTr.DetachChildren();
                Destroy(_particlePattern.gameObject, duration);
                _bossCube.transform.DOPlay();
            })
        );
        _seq.Append(this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove()));
    }

    public override void AttackPatternThree()
    {
        const float duration = 7f;
        _particlePattern = Instantiate(_particles[2], _particleTr.position, Quaternion.identity, _particleTr).GetComponent<ParticleSystem>();
        _particlePattern.Stop();
        var main = _particlePattern.main;
        var emission = _particlePattern.emission;
        if (_difficulty == Enums.Difficulties.Expert)
        {
            main.duration -= 0.1f;
        }
        else if (_difficulty == Enums.Difficulties.Ruthless)
        {
            main.duration -= 0.25f;
        }
        float moveToPosTime = 0.5f;

        _flashEffector.Flash();
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
                    SetEase(Ease.InOutQuad)
                );
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
            })
        );
        _seq.Append(this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove()));
    }

    public override void PhaseSecondStart()
    {
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

        //第二形態の攻撃を生成
        _particlePattern = Instantiate(_particles[3]).GetComponent<ParticleSystem>();
        var emission = _particlePattern.emission;

        //シールド展開時間
        float shieldDuration = 20f;
        if (_difficulty == Enums.Difficulties.Expert)
        {
            shieldDuration = 25f;
        }
        else if (_difficulty == Enums.Difficulties.Ruthless)
        {
            shieldDuration = 30f;
        }

        //レーザーの生成
        float laserInterval = 4f;
        if (_difficulty == Enums.Difficulties.Expert)
        {
            laserInterval = 3.1f;
        }
        else if (_difficulty == Enums.Difficulties.Ruthless)
        {
            laserInterval = 2.4f;
        }
        StartCoroutine(ShootLaser(shieldDuration, laserInterval, true));

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
                _tweens.Add(this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove()));
            })
        );
    }

    private IEnumerator ShootLaser(float duration, float interval, bool isLargeLaser = false)
    {
        for (float i = duration - interval; i > 0; i -= interval)
        {
            if (isLargeLaser)
            {
                Instantiate(_laserLarge, new Vector2(Random.Range(_startPos.x, -_startPos.x), 25), Quaternion.Euler(0, 0, 180), transform);
            }
            else
            {
                Instantiate(_laser, transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
