using System.Collections.Generic;
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

        private readonly List<PlayerJoinedUI> uis = new();

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