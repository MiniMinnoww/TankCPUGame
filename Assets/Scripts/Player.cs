using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering.Universal;

public delegate void OnPlayerKilled(Player player);
public class Player : MonoBehaviour
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

    public Rigidbody2D RB => rb;
    public BaseState CurrentState { get; private set; }

    public event OnPlayerKilled OnPlayerKilledEvent;

    private Dictionary<string, BaseState> states = new();


    private void Start() => Setup();

    public void Setup()
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

    void Update()
    {
        brain.Update();
        CurrentState.Update();
    }

    void FixedUpdate() => CurrentState.FixedUpdate();

    public void OnKilled(Health health)
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
        if (brain == null && newBrain != null)
        {
            brain = newBrain;
            return true;
        }
        return false;
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
}

[Serializable]
public struct Lag
{
    public float startLag;
    public float endLag;
}
