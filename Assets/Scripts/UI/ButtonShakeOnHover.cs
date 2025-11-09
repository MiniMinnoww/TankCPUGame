using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class ButtonShakeOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const float DURATION = 0.5f;
        private const float MAX_ANGLE = 7f;
        private const int OSCILLATIONS = 3;

        public void OnPointerEnter(PointerEventData eventData) => Shake();
        public void OnPointerExit(PointerEventData eventData) => StopShake();

        private RectTransform rect;
        private void Awake() => rect = GetComponent<RectTransform>();
        private void Shake()
        {
            StopShake();

            Sequence seq = DOTween.Sequence();

            const float stepDur = DURATION / OSCILLATIONS;

            for (int i = 0; i < OSCILLATIONS; i++)
            {
                float t = (float)i / OSCILLATIONS;
                float angle = Mathf.Lerp(MAX_ANGLE, 0f, t);
                float dir = (i % 2 == 0) ? 1f : -1f;
                float targetZ = dir * angle;

                seq.Append(rect.DOLocalRotate(new Vector3(0f, 0f, targetZ), stepDur * 0.5f)
                    .SetEase(Ease.OutQuad));
                seq.Append(rect.DOLocalRotate(Vector3.zero, stepDur * 0.5f)
                    .SetEase(Ease.InQuad));
            }

            seq.OnComplete(StopShake);
        }


        private void StopShake()
        {
            transform.DOKill();
            transform.localRotation = Quaternion.identity;
        }
    }
}