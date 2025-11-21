using System.Collections.Generic;
using System.Linq;
using Brains;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PlayerJoinMenu : MonoBehaviour
    {
        private static PlayerJoinMenu Instance { get; set; }
        [SerializeField] private BrainList possibleBrains;
        [SerializeField] private PlayerJoinedUI playerJoinedUIPrefab;
        [SerializeField] private Transform playerJoinedUIParent;

        public static GameData NextGameData { get; private set; }
        public static BrainList Brains => Instance.possibleBrains;

        private readonly List<PlayerJoinedUI> uis = new();

        private const int MAX_PLAYERS = 7;

        private void Awake() => Instance = this;

        private void Start()
        {
            possibleBrains.ReloadCache();
            ResetUIs();
        }

        private void ResetUIs()
        {
            foreach (PlayerJoinedUI ui in uis) ui.Kill();
            uis.Clear();
        }

        public void AddNewUI()
        {
            if (uis.Count >= MAX_PLAYERS) return;
            
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
            List<PlayerSetupInfo> infos = uis.Select(ui => ui.GetData()).ToList();
            return new GameData(infos);
        }
    
        public static Brain GetBrainFromName(string brainName)
        {
            return Instance.possibleBrains.GetBrainFromName(brainName);
        }
    }

    public struct GameData
    {
        public readonly List<PlayerSetupInfo> playerInfos;

        public GameData(List<PlayerSetupInfo> infos)
        {
            playerInfos = infos;
        }
    }

    public struct PlayerSetupInfo
    {
        public readonly string playerName;
        public readonly Brain brain;
        public int colourIndex;
    
        public PlayerSetupInfo(string playerName, int colourIndex, Brain brain)
        {
            this.playerName = playerName;
            this.colourIndex = colourIndex;
            this.brain = brain;
        }
    }
}