using System;
using System.Collections;
using System.Collections.Generic;
using Brains;
using Healths;
using Hurtboxes;
using Managers;
using Projectiles;
using States;
using States.PlayerStates;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Players
{
    public delegate void OnPlayerKilled(Player player);
    public class Player : MonoBehaviour, IPlayerBrainInterface, IDetectableTank
    {
        [SerializeField] private Projectile projectilePrefab;
        [FormerlySerializedAs("sprite")] [SerializeField] private SpriteRenderer bodySprite;
        [SerializeField] private SpriteRenderer turretSprite;

        [SerializeField] private Rigidbody2D rb;

        [SerializeField] private TankMovementStats movementStats;

        [SerializeField] private TankHealthStats healthStats;

        [SerializeField] private PlayerHurtbox hurtbox;

        [SerializeField] private Slider healthSlider;
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private ParticleSystem deathParticles;
        [SerializeField] private Light2D deathLight;

        [SerializeField] private float viewRange = 8;
        [SerializeField] private float viewAngle = 80;
        [SerializeField] private LayerMask detectionLayers;
        [SerializeField] private LayerMask obstacleLayers;

        [Header("Lag Data")]
        [SerializeField] private Lag shootingLag;

        private Brain brain;
        private Health health;
        
        public int ColourIndex { get; private set; } // Stored so projectiles can use it

        public Rigidbody2D Rb => rb;
        private BaseState CurrentState { get; set; }

        private string playerName;
        public string PlayerName
        {
            get => playerName;
            set
            {
                playerNameText.text = value;
                playerName = value;
            }
        }

        public event OnPlayerKilled OnPlayerKilledEvent;

        private readonly Dictionary<string, BaseState> states = new();
        
        // Caching the view cone for the current frame
        private List<IDetectableObject> cachedViewCone;
        private int cachedFrame;


        private void Start() => Setup();

        private void Setup()
        {
            health = new PlayerHealth(healthStats.maxHealth, healthSlider);
            health.OnKillEvent += OnKilled;

            deathLight.enabled = false;

            hurtbox.Setup(health);

            SetupStates();
            
            brain.Start();
        }

        public void SetupSprites(int colourIndex)
        {
            ColourIndex = colourIndex;

            bodySprite.sprite = Global.Sprites[colourIndex].bodySprite;
            turretSprite.sprite = Global.Sprites[colourIndex].turretSprite;
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
        public float GetHealth()
        {
            return health.CurrentHealth / health.MaxHealth;
        }

        public Vector2 GetForward() => transform.up;
        public float GetRotation() => rb.rotation;

        public List<IDetectableObject> GetObjectsInViewCone()
        {
            List<IDetectableObject> results = new List<IDetectableObject>();

            if (Time.frameCount == cachedFrame && cachedViewCone != null)
            {
                // We already cached our results this frame, so return those instead
                return new List<IDetectableObject>(cachedViewCone);
            }
            
            // Get hits
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, viewRange, detectionLayers);

            Vector2 origin = transform.position; 
            Vector2 forward = transform.up;
            float halfAngle = viewAngle * 0.5f;

            foreach (Collider2D hitCollider in hits)
            {
                IDetectableObject obj = hitCollider.GetComponent<IDetectableObject>();
                
                // Don't detect ourselves
                if (obj == null) continue;
                if (ReferenceEquals(obj, this)) continue;
                
                // Calculations
                Vector2 pos = obj.GetPosition();
                Vector2 dir = pos - origin;
                float angle = Vector2.Angle(forward, dir);

                if (angle > halfAngle) continue;

                // Line of sight NOTE: Removed this because tanks should see through barriers
                // RaycastHit2D isBlocked = Physics2D.Raycast(origin, dir.normalized, dir.magnitude, obstacleLayers);

                // if (isBlocked) continue;

                results.Add(obj);
            }
            
            // As the results weren't cached, then cache them now
            cachedViewCone = results;
            cachedFrame = Time.frameCount;

            return results;
        }

        public IDetectableTank GetTankReference() => this;

        #endregion

        #region Debug

        private void OnDrawGizmosSelected()
        {
            Vector2 origin = transform.position;
            Vector2 forward = transform.up;

            float halfAngle = viewAngle * 0.5f;

            Vector2 leftDir = Rotate(forward, -halfAngle);
            Vector2 rightDir = Rotate(forward, halfAngle);

            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(origin, viewRange);
            Gizmos.DrawLine(origin, origin + leftDir * viewRange);
            Gizmos.DrawLine(origin, origin + rightDir * viewRange);

            int steps = 20;
            for (int i = 0; i < steps; i++)
            {
                float a0 = Mathf.Lerp(-halfAngle, halfAngle, i / (float)steps);
                float a1 = Mathf.Lerp(-halfAngle, halfAngle, (i + 1) / (float)steps);

                Vector2 d0 = Rotate(forward, a0);
                Vector2 d1 = Rotate(forward, a1);

                Gizmos.DrawLine(origin + d0 * viewRange, origin + d1 * viewRange);
            }
        }

        private static Vector2 Rotate(Vector2 v, float degrees)
        {
            float rad = degrees * Mathf.Deg2Rad;
            float s = Mathf.Sin(rad);
            float c = Mathf.Cos(rad);
            return new Vector2(c * v.x - s * v.y, s * v.x + c * v.y);
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