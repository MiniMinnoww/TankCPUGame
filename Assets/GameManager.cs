using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player playerPrefab;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private CinemachineTargetGroup targetGroup;

    private List<Player> alivePlayers = new();

    private void Awake() => Instance = this;

    private void Start()  
    {
        SetupPlayers(PlayerJoinMenu.NextGameData.playerInfos.ToArray());
        SetCameraTargets();
    }

    private void SetupPlayers(PlayerSetupInfo[] playersToSetup) 
    {
        for (int i = 0; i < playersToSetup.Length; i++) 
        {
            // Creates a new instance of this "brain" class
            Brain playerBrain = (Brain) Activator.CreateInstance(playersToSetup[i].brain.GetType());

            Player newPlayer = Instantiate(playerPrefab, spawnPositions[i % spawnPositions.Length].position, spawnPositions[i % spawnPositions.Length].rotation);
            if (newPlayer.SetBrain(playerBrain))
            {
                alivePlayers.Add(newPlayer);
                newPlayer.OnPlayerKilledEvent += OnPlayerDeath;
            }
            else 
            {
                Debug.LogError($"Can't set player brain to '{playerBrain}'. Destroying player...");
                Destroy(newPlayer.gameObject);
            }
        }
    }

    private void SetCameraTargets()
    {
        targetGroup.Targets.Clear();

        foreach (Player player in alivePlayers)
            targetGroup.AddMember(player.transform, 1, 1);
    }

    private void OnPlayerDeath(Player player)
    {
        player.OnPlayerKilledEvent -= OnPlayerDeath;

        Destroy(player.gameObject);
        alivePlayers.Remove(player);

        CheckForWin();
    }

    private void CheckForWin()
    {
        if (alivePlayers.Count > 1) return;
        SceneManager.LoadScene("PlayerJoinMenu");
    }
}
