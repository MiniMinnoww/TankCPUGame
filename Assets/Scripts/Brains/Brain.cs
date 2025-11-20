using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brains
{
    public abstract class Brain
    {
        protected readonly IPlayerBrainInterface player;
        protected Brain() {}

        protected Brain(IPlayerBrainInterface player)
        {
            this.player = player;
        }
        public Action onShootEvent;
        private float driveInput;
        
        public float DriveInput
        {
            get => driveInput;
            protected set => driveInput = Mathf.Clamp(value, -1, 1);
        }

        private float rotationInput;

        public  float RotationInput
        {
            get => rotationInput;
            protected set => rotationInput = Mathf.Clamp(value, -1, 1);
        }
        
        public virtual void Start() { }

        public virtual void Update() { }
    
        protected void Shoot() => onShootEvent?.Invoke();

        protected Coroutine StartCoroutine(IEnumerator e) => player.RunCoroutine(e);
        protected void StopCoroutine(Coroutine c) => player.HaltCoroutine(c);
    }

    public interface IPlayerBrainInterface
    {
        public Coroutine RunCoroutine(IEnumerator enumerator);
        public void HaltCoroutine(Coroutine c);
        public Vector2 GetPosition();
        public Vector2 GetForward();
        public float GetRotation();
        public List<IDetectableObject> GetObjectsInViewCone();
        public IDetectableTank GetTankReference();
    }
}
