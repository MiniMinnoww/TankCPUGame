using System.Collections;
using Brains;
using Players;
using UnityEngine;

namespace States
{
    public abstract class BaseState
    {
        private readonly float startLag;
        private readonly float endLag;

        protected Player Owner { get; private set; }
        protected Brain Brain { get; private set; }

        private bool InStartLag { get; set; }
        private bool InEndLag { get; set; }
        private bool InLag => InStartLag || InEndLag;

        private Coroutine startLagCoroutine;
        private Coroutine endLagCoroutine;

        public virtual bool CanTransitionToThisState() => true;
        public virtual bool ResetVelocityOnEnter() => true;

        protected BaseState(Player owner, Brain brain, float startLag=0, float endLag=0)
        {
            Owner = owner;
            Brain = brain;

            this.startLag = startLag;
            this.endLag = endLag;
        }

        public void OnEnterState()
        {
            InStartLag = false;
            InEndLag = false;

            RegisterEvents();

            if (startLagCoroutine != null) Owner.StopCoroutine(startLagCoroutine);
            startLagCoroutine = Owner.StartCoroutine(StartLagCoroutine());
        }

        private IEnumerator StartLagCoroutine()
        {
            BeforeStartLag();
            InStartLag = true;
            yield return new WaitForSeconds(startLag);
            InStartLag = false;
            AfterStartLag();
        }

        protected virtual void BeforeStartLag() { }
        protected virtual void AfterStartLag() { }

        public void Update() 
        {
            if (!InLag) OnUpdate();
        }
        public void FixedUpdate() 
        {
            if (!InLag) OnFixedUpdate();
        }

        protected virtual void OnUpdate() { }
        protected virtual void OnFixedUpdate() { }

        protected void StartEndLag()
        {
            if (endLagCoroutine != null) Owner.StopCoroutine(endLagCoroutine);
            endLagCoroutine = Owner.StartCoroutine(EndLagCoroutine());
        }
        private IEnumerator EndLagCoroutine()
        {
            BeforeEndLag();
            InEndLag = true;
            yield return new WaitForSeconds(endLag);
            InEndLag = false;
            AfterEndLag();
            OnLeaveState();
        }
        protected virtual void BeforeEndLag() { }
        protected virtual void AfterEndLag() { }

        // Could be called forcefully (externally) or by this class after end lag.
        public void OnLeaveState()
        {
            if (startLagCoroutine != null) Owner.StopCoroutine(startLagCoroutine);
            if (endLagCoroutine != null) Owner.StopCoroutine(endLagCoroutine);

            InStartLag = false;
            InEndLag = false;

            UnregisterEvents();

            OnStateEnd();
        }
        protected virtual void OnStateEnd() { }

        protected virtual void RegisterEvents() { }
        protected virtual void UnregisterEvents() { }
    }
}