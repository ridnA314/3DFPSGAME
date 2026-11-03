using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 타이틀 글리치(UI_GlitchText.BurstStarted)에 맞춰 벼락을 떨어뜨린다.
// 번개 줄기는 카메라 로컬 좌표의 LineRenderer로 매번 새로 생성하고, 배경 위 플래시 이미지를 두 번 번쩍이게 한다.
public class UI_LightningStrike : MonoBehaviour
{
    [SerializeField] private UI_GlitchText _glitchText;
    [SerializeField] private bool _strikeOnIntro = true;

    [Header("Flash")]
    [SerializeField] private Image _flash;
    [SerializeField] private float _flashMaxAlpha = 0.4f;
    [SerializeField]
    private AnimationCurve _intensity = new AnimationCurve(
        new Keyframe(0f, 0f), new Keyframe(0.03f, 1f), new Keyframe(0.08f, 0.15f),
        new Keyframe(0.14f, 0.9f), new Keyframe(0.4f, 0f));

    [Header("Bolt")]
    [SerializeField] private LineRenderer _bolt;
    [SerializeField] private LineRenderer _boltGlow;
    [SerializeField] private LineRenderer[] _branches;
    [SerializeField] private Vector2 _startXRange = new Vector2(-6f, 6f);
    [SerializeField] private float _startY = 5.2f;
    [SerializeField] private Vector2 _endYRange = new Vector2(-0.6f, 0.8f);
    [SerializeField] private float _endXSpread = 1.5f;
    [SerializeField] private int _subdivisions = 5;
    [SerializeField] private float _displacement = 1.2f;
    [SerializeField] private int _branchSubdivisions = 3;
    [SerializeField] private float _branchLength = 1.6f;
    [SerializeField] private float _branchAngle = 40f;
    [SerializeField, Range(0f, 1f)] private float _branchDisplacementRatio = 0.4f;
    [SerializeField] private Color _boltColor = new Color(0.85f, 0.9f, 1f, 1f);
    [SerializeField, Range(0f, 1f)] private float _glowAlpha = 0.35f;

    [Header("Thunder")]
    [SerializeField] private AudioSource _thunderSource;
    [SerializeField] private AudioClip[] _thunderClips;
    [SerializeField] private Vector2 _thunderDelayRange = new Vector2(0.15f, 0.6f);
    [SerializeField] private Vector2 _thunderPitchRange = new Vector2(0.85f, 1.05f);
    [SerializeField, Range(0f, 1f)] private float _thunderVolume = 0.9f;

    private Vector3[] _boltPoints;
    private Vector3[] _branchPoints;
    private Color _flashColor;
    private Coroutine _strikeRoutine;
    private Coroutine _thunderRoutine;

    private void Awake()
    {
        _boltPoints = new Vector3[(1 << _subdivisions) + 1];
        _branchPoints = new Vector3[(1 << _branchSubdivisions) + 1];
        _flashColor = _flash.color;
        SetVisible(false);
    }

    private void OnEnable()
    {
        _glitchText.BurstStarted += OnBurstStarted;
    }

    private void OnDisable()
    {
        _glitchText.BurstStarted -= OnBurstStarted;

        if (_strikeRoutine != null)
        {
            StopCoroutine(_strikeRoutine);
            _strikeRoutine = null;
        }

        if (_thunderRoutine != null)
        {
            StopCoroutine(_thunderRoutine);
            _thunderRoutine = null;
        }

        SetVisible(false);
    }

    public void Strike()
    {
        if (_strikeRoutine != null)
        {
            StopCoroutine(_strikeRoutine);
        }

        _strikeRoutine = StartCoroutine(Strike_Coroutine());

        if (_thunderSource != null && _thunderClips.Length > 0)
        {
            if (_thunderRoutine != null)
            {
                StopCoroutine(_thunderRoutine);
            }

            _thunderRoutine = StartCoroutine(Thunder_Coroutine());
        }
    }

    // 빛보다 소리가 늦게 도착하도록 약간 지연 후 천둥을 재생한다.
    private IEnumerator Thunder_Coroutine()
    {
        yield return UIWait.Seconds_Coroutine(Random.Range(_thunderDelayRange.x, _thunderDelayRange.y));

        AudioClip clip = _thunderClips[Random.Range(0, _thunderClips.Length)];
        _thunderSource.pitch = Random.Range(_thunderPitchRange.x, _thunderPitchRange.y);
        _thunderSource.PlayOneShot(clip, _thunderVolume);
        _thunderRoutine = null;
    }

    private void OnBurstStarted(bool isIntro)
    {
        if (isIntro && !_strikeOnIntro) return;

        Strike();
    }

    private IEnumerator Strike_Coroutine()
    {
        GenerateBolt();
        SetVisible(true);

        float duration = _intensity.keys[_intensity.length - 1].time;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            ApplyIntensity(Mathf.Clamp01(_intensity.Evaluate(elapsed)));
            yield return null;
        }

        SetVisible(false);
        _strikeRoutine = null;
    }

    private void GenerateBolt()
    {
        Vector3 start = new Vector3(Random.Range(_startXRange.x, _startXRange.y), _startY, 0f);
        Vector3 end = new Vector3(start.x + Random.Range(-_endXSpread, _endXSpread), Random.Range(_endYRange.x, _endYRange.y), 0f);
        BuildJaggedLine(_boltPoints, start, end, _displacement);

        _bolt.positionCount = _boltPoints.Length;
        _bolt.SetPositions(_boltPoints);
        _boltGlow.positionCount = _boltPoints.Length;
        _boltGlow.SetPositions(_boltPoints);

        int branchCount = Random.Range(0, _branches.Length + 1);
        for (int i = 0; i < _branches.Length; i++)
        {
            LineRenderer branch = _branches[i];
            bool isUsed = i < branchCount;
            branch.gameObject.SetActive(isUsed);
            if (!isUsed) continue;

            // 본 줄기의 중간 구간에서 아래쪽으로 비스듬히 갈라져 나온다.
            int quarter = _boltPoints.Length / 4;
            Vector3 origin = _boltPoints[Random.Range(quarter, _boltPoints.Length - quarter)];
            float angle = Random.Range(-_branchAngle, _branchAngle) * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), -Mathf.Cos(angle), 0f);
            BuildJaggedLine(_branchPoints, origin, origin + direction * _branchLength, _displacement * _branchDisplacementRatio);

            branch.positionCount = _branchPoints.Length;
            branch.SetPositions(_branchPoints);
        }
    }

    // 중점 변위로 지그재그 선을 만든다. points 길이는 2^n + 1.
    private static void BuildJaggedLine(Vector3[] points, Vector3 start, Vector3 end, float displacement)
    {
        int last = points.Length - 1;
        points[0] = start;
        points[last] = end;

        Vector3 direction = (end - start).normalized;
        Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0f);

        float offset = displacement;
        for (int step = last; step > 1; step /= 2)
        {
            int half = step / 2;
            for (int i = half; i < last; i += step)
            {
                Vector3 mid = (points[i - half] + points[i + half]) * 0.5f;
                points[i] = mid + perpendicular * Random.Range(-offset, offset);
            }

            offset *= 0.5f;
        }
    }

    private void ApplyIntensity(float intensity)
    {
        Color flash = _flashColor;
        flash.a = _flashMaxAlpha * intensity;
        _flash.color = flash;

        Color core = _boltColor;
        core.a *= intensity;
        SetLineColor(_bolt, core);
        foreach (LineRenderer branch in _branches)
        {
            SetLineColor(branch, core);
        }

        Color glow = _boltColor;
        glow.a *= intensity * _glowAlpha;
        SetLineColor(_boltGlow, glow);
    }

    private void SetVisible(bool isVisible)
    {
        _bolt.enabled = isVisible;
        _boltGlow.enabled = isVisible;
        foreach (LineRenderer branch in _branches)
        {
            branch.enabled = isVisible;
        }

        // 씬 언로드 중에는 다른 캔버스의 플래시 이미지가 먼저 파괴될 수 있다.
        if (!isVisible && _flash != null)
        {
            Color flash = _flashColor;
            flash.a = 0f;
            _flash.color = flash;
        }
    }

    private static void SetLineColor(LineRenderer line, Color color)
    {
        line.startColor = color;
        line.endColor = color;
    }
}
