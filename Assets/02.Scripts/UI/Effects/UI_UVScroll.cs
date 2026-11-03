using UnityEngine;
using UnityEngine.UI;

// RawImage의 UV를 흘려 연기/안개 레이어가 흐르는 것처럼 보이게 한다. 텍스처는 Wrap Mode가 Repeat여야 한다.
[RequireComponent(typeof(RawImage))]
public class UI_UVScroll : MonoBehaviour
{
    [SerializeField] private Vector2 _scrollSpeed = new Vector2(0.01f, 0.003f);

    private RawImage _rawImage;

    private void Awake()
    {
        _rawImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        Rect uvRect = _rawImage.uvRect;
        uvRect.position = new Vector2(Mathf.Repeat(uvRect.x + _scrollSpeed.x * Time.unscaledDeltaTime, 1f), Mathf.Repeat(uvRect.y + _scrollSpeed.y * Time.unscaledDeltaTime, 1f));
        _rawImage.uvRect = uvRect;
    }
}
