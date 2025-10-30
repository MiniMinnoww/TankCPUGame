using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ButtonShakeOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const float duration = 0.5f;
    private const float maxAngle = 7f;
    private const int oscillations = 3;

    public void OnPointerEnter(PointerEventData eventData) => Shake();
    public void OnPointerExit(PointerEventData eventData) => StopShake();

    private RectTransform rect;
    private void Awake() => rect = GetComponent<RectTransform>();
    private void Shake()
    {
        StopShake();

        Sequence seq = DOTween.Sequence();

        float stepDur = duration / oscillations;

        for (int i = 0; i < oscillations; i++)
        {
            float t = (float)i / oscillations;
            float angle = Mathf.Lerp(maxAngle, 0f, t);
            float dir = (i % 2 == 0) ? 1f : -1f;
            float targetZ = dir * angle;

            seq.Append(rect.DOLocalRotate(new Vector3(0f, 0f, targetZ), stepDur * 0.5f)
                .SetEase(Ease.OutQuad));
            seq.Append(rect.DOLocalRotate(Vector3.zero, stepDur * 0.5f)
                .SetEase(Ease.InQuad));
        }

        seq.OnComplete(() => StopShake());
    }


    private void StopShake()
    {
        transform.DOKill();
        transform.localRotation = Quaternion.identity;
    }
}