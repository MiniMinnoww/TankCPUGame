using System;
using System.Collections;
using System.Collections.Generic;
using Brains;
using Healths;
using Hurtboxes;
using Projectiles;
using States;
using States.PlayerStates;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Players
{
    public delegate void OnPlayerKilled(Player player);
    public class Player : MonoBehaviour, IPlayerBrainInterface, IDetectableTank
    {
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private SpriteRenderer sprite;

        [SerializeField] private Rigidbody2D rb;

        [SerializeField] private TankMovementStats movementStats;

        [SerializeField] private TankHealthStats healthStats;

        [SerializeField] private PlayerHurtbox hurtbox;

        [SerializeField] private Slider healthSlider;
        [SerializeField] private ParticleSystem deathParticles;
        [SerializeField] private Light2D deathLight;

        [Header("Lag Data")]
        [SerializeField] private Lag shootingLag;

        private Brain brain;
        private Health health;

        public Rigidbody2D Rb => rb;
        private BaseState CurrentState { get; set; }

        public event OnPlayerKilled OnPlayerKilledEvent;

        private readonly Dictionary<string, BaseState> states = new();


        private void Start() => Setup();

        private void Setup()
        {
            health = new PlayerHealth(healthStats.maxHealth, healthSlider);
            health.OnKillEvent += OnKilled;

            deathLight.enabled = false;

            hurtbox.Setup(health);

            SetupStates();
        }

        private void SetupStates()
        {
            states.Add("movement", new MovementState(this, brain, hurtbox, movementStats));
            states.Add("shooting", new ShootingState(this, brain, hurtbox, projectilePrefab, shootingLag.startLag, shootingLag.endLag));
            states.Add("hitstun", new HitstunState(this, brain, 0.2f));

            SwitchState("movement");
        }

        private void Update()
        {
            brain.Update();
            CurrentState.Update();
        }

        private void FixedUpdate() => CurrentState.FixedUpdate();

        private void OnKilled(Health _)
        {
            PlayDeathParticles();
            OnPlayerKilledEvent?.Invoke(this);
        }

        private void PlayDeathParticles()
        {
            if (deathParticles == null)
            {
                Debug.LogWarning("No death particles assigned");
                return;
            }

            deathParticles.transform.parent = null;
            deathLight.enabled = false;
            deathParticles.Play();
        }

        public bool SetBrain(Brain newBrain)
        {
            if (brain != null || newBrain == null) return false;
            brain = newBrain;
            return true;
        }

        public bool SwitchState(string newState)
        {
            newState = newState.ToLower();

            if (!states.ContainsKey(newState))
            {
                Debug.LogWarning($"Can't find state {newState} in states");
                return false;
            }

            if (!states[newState].CanTransitionToThisState()) return false;

            CurrentState?.OnLeaveState();
            CurrentState = states[newState];

            if (CurrentState.ResetVelocityOnEnter()) rb.linearVelocity = Vector2.zero;
            CurrentState.OnEnterState();


            return true;
        }

        #region Brain Access Methods
        public Coroutine RunCoroutine(IEnumerator enumerator) => StartCoroutine(enumerator);
        public void HaltCoroutine(Coroutine c) => StopCoroutine(c);
        public ObjectType GetObjectType() => ObjectType.Player;
        public Vector2 GetPosition() => transform.position;
        public Vector2 GetForward() => transform.up;
        public List<IDetectableObject> GetObjectsInViewCone()
        {
            return null;
        }
        #endregion
    }

    [Serializable]
    public struct Lag
    {
        public float startLag;
        public float endLag;
    }
}