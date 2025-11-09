using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace VFX
{
    public class ScreenShake : MonoBehaviour
    {
        private static ScreenShake instance;

        [SerializeField] private CinemachineBasicMultiChannelPerlin noise;
    
        private Shake currentShake;
        private Coroutine shakeCoroutine;
    
        public static Shake smallShake = new(0.5f, 0.5f, 0.1f);
        public static Shake mediumShake = new(2f, 2f, 0.3f);
        public static Shake largeShake = new(4f, 4f, 0.5f);

        private void Awake() => instance = this;

        private IEnumerator _ProcessShake(Shake s)
        {
            currentShake = s;

            noise.AmplitudeGain = s.amp;
            noise.FrequencyGain = s.frequency;
        
            yield return new WaitForSecondsRealtime(s.duration);
            noise.AmplitudeGain = 0;
            noise.FrequencyGain = 0;

            currentShake.duration = 0;
        }
    
        public static void ShakeScreen(Shake s) 
        {
            if (instance != null) instance._ShakeScreen(s);
        }
    
        private void _ShakeScreen(Shake s)
        {
            //There is already a shake going, if our amplitude is bigger we take priority,
            if (currentShake.duration != 0)
            {
                if (s.amp >= currentShake.amp)
                    StopCoroutine(shakeCoroutine);
                else return; //We don't have priority
            }
            shakeCoroutine = StartCoroutine(_ProcessShake(s));
        }
    }

    [Serializable]
    public struct Shake
    {
        public float amp;
        public float frequency;
        public float duration;

        public Shake(float amp, float frequency, float duration)
        {
            this.amp = amp;
            this.frequency = frequency;
            this.duration = duration;
        }
    }
}