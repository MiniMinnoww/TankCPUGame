using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private SpriteRenderer sprite;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private TankMovementStats movementStats;

    [SerializeField] private TankHealthStats healthStats;

    [SerializeField] private Hurtbox hurtbox;

    [SerializeField] private Slider healthSlider;

    [Header("Lag Data")]
    [SerializeField] private Lag shootingLag;

    private Brain brain;
    private Health health;

    public Rigidbody2D RB => rb;
    public BaseState CurrentState { get; private set; }

    private Dictionary<string, BaseState> states = new();


    private void Start() => Setup();

    public void Setup()
    {
        health = new PlayerHealth(healthStats.maxHealth, healthSlider);

        hurtbox.Setup(health);

        SetupStates();
    }

    private void SetupStates()
    {
        states.Add("movement", new MovementState(this, brain, movementStats));
        states.Add("shooting", new ShootingState(this, brain, hurtbox, projectilePrefab, shootingLag.startLag, shootingLag.endLag));

        SwitchState("movement");
    }

    void Update()
    {
        brain.Update();
        CurrentState.Update();
    }

    void FixedUpdate()
    {
        CurrentState.FixedUpdate();

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
