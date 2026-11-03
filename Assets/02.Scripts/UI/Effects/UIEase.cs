// UI 연출에서 공통으로 사용하는 이징 함수.
public static class UIEase
{
    private const float BackOvershoot = 1.70158f;

    // 목표를 살짝 넘었다가 돌아오는 감속 곡선 (t: 0 → 1)
    public static float OutBack(float t)
    {
        float c3 = BackOvershoot + 1f;
        float x = t - 1f;
        return 1f + c3 * x * x * x + BackOvershoot * x * x;
    }

    public static float OutCubic(float t)
    {
        float x = 1f - t;
        return 1f - x * x * x;
    }
}
