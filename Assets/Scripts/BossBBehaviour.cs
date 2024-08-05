using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossBBehaviour : BossBase
{
    [Header("ÉåÅ[ÉUÅ[ÇÃPrefab")]
    [SerializeField]
    private GameObject _laserLarge;
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
            OnStart(() => _bossCube.transform.DOPlay()).
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

    public override void PhaseSecondStart()
    {
        Debug.Log("Phase 2 Start");
        StartCoroutine(Flash());
        _seq?.Kill();
        _bossCube.transform.DOPause();
        this.transform.DOKill();

        if (_particlePattern != null)
        {
            Destroy(_particlePattern.gameObject);
        }
        _particlePattern = Instantiate(_particles[3]).GetComponent<ParticleSystem>();
        var emission = _particlePattern.emission;

        _shield.SetActive(true);
        var tw = _shield.transform.DORotate(new Vector3(0, 0, -360), 3, RotateMode.FastBeyond360).
            SetLoops(-1, LoopType.Incremental).
            SetEase(Ease.Linear);
        _tweens.Add(tw);
        this.transform.position = _pos[4].position;
        _bossCube.transform.rotation = Quaternion.Euler(0, 0, 90);

        float shieldDuration = 20f;
        if (_difficulty == "expert")
        {
            shieldDuration = 25f;
        }
        else if (_difficulty == "ruthless")
        {
            shieldDuration = 30f;
        }

        float laserInterval = 4f;
        if (_difficulty == "expert")
        {
            laserInterval = 3.5f;
        }
        else if (_difficulty == "ruthless")
        {
            laserInterval = 3f;
        }

        StartCoroutine(ShootLaser(shieldDuration, laserInterval, true));

        _tweens.Add(_bossCube.transform.DORotate(new Vector3(Random.Range(-3600, 3600), Random.Range(-3600, 3600), Random.Range(-3600, 3600)), shieldDuration, RotateMode.FastBeyond360).
            SetEase(Ease.Linear).
            OnComplete(() =>
            {
                emission.enabled = false;
                foreach (var go in transform.GetComponentsInChildren<LaserBeam>())
                {
                    Destroy(go.gameObject);
                }
                StartCoroutine(Flash());
                tw.Kill();
                _shield.SetActive(false);
                _bossCube.transform.DOPlay();
                this.transform.DOMove(_startPos, 0.5f).OnComplete(() => WanderingMove());
            }
            ));
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
        StartCoroutine(Flash());
        _bossCube.transform.DORotate(360 * Random.Range(4.6f, 5.1f) * Vector3.one, duration, RotateMode.FastBeyond360).
            SetEase(Ease.InExpo).
            OnComplete(() => StartCoroutine(Explode()));
        StartCoroutine(FindObjectOfType<LightRay>().EmitLightRay(duration, 10));
    }
    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.2f);
        CameraShaker.Instance.Shake(3, 0, 1, 0.4f);
        StartCoroutine(Flash());
        _bossCube.SetActive(false);
        FindObjectOfType<LightRay>().gameObject.SetActive(false);
        _seAus.PlayOneShot(_deathExplodeSE);
        Instantiate(_explodePrefab, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("StageClear");
    }

    private IEnumerator Flash()
    {
        var flash = GameObject.Find("FlashPanel").GetComponent<Image>();
        flash.enabled = true;
        yield return new WaitForSeconds(0.1f);
        flash.enabled = false;
    }

    private IEnumerator ShootLaser(float duration, float interval, bool isLarseLaser = false)
    {
        for (float i = duration - interval; i > 0; i -= interval)
        {
            if (isLarseLaser)
            {
                Instantiate(_laserLarge, new Vector2(Random.Range(_startPos.x, -_startPos.x), 25), Quaternion.Euler(0, 0, 180), transform);
            }
            else
            {
                Instantiate(_laser, transform);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
