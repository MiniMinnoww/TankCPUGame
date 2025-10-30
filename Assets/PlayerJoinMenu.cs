using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerJoinMenu : MonoBehaviour
{
    public static PlayerJoinMenu Instance { get; private set; }
    [SerializeField] private BrainList possibleBrains;
    [SerializeField] private PlayerJoinedUI playerJoinedUIPrefab;
    [SerializeField] private Transform playerJoinedUIParent;

    public static GameData NextGameData { get; private set; }

    private readonly List<PlayerJoinedUI> uis = new();

    private void Awake() => Instance = this;

    private void Start()
    {
        possibleBrains.ReloadCache();
        ResetUIs();
    }
    public void ResetUIs()
    {
        foreach (PlayerJoinedUI ui in uis) ui.Kill();
        uis.Clear();
    }

    public void AddNewUI()
    {
        PlayerJoinedUI newUI = Instantiate(playerJoinedUIPrefab, playerJoinedUIParent);
        newUI.SetupDropdown(possibleBrains.GetBrainNames());
        uis.Add(newUI);
    }

    public void StartGame()
    {
        NextGameData = GenerateGameData();

        SceneManager.LoadScene("MainLevel");
    }

    private GameData GenerateGameData()
    {
        List<PlayerSetupInfo> infos = new();
        foreach (PlayerJoinedUI ui in uis) infos.Add(ui.GetData());
        return new GameData(infos);
    }
    
    public static Brain GetBrainFromName(string brainName)
    {
        return Instance.possibleBrains.GetBrainFromName(brainName);
    }
}

public struct GameData
{
    public List<PlayerSetupInfo> playerInfos;

    public GameData(List<PlayerSetupInfo> infos)
    {
        playerInfos = infos;
    }
}

public struct PlayerSetupInfo
{
    public string playerName;
    public Brain brain;
    // TODO: Add things like colour, name, etc.
    
    public PlayerSetupInfo(string playerName, Brain brain)
    {
        this.playerName = playerName;
        this.brain = brain;
    }
}
