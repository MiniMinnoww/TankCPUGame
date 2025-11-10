using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class WinMenu : MonoBehaviour
    {
        public static WinMenu Instance { get; private set; }
        [SerializeField] private TextMeshProUGUI winnerName;
        [SerializeField] private GameObject winPanel;

        private void Awake() => Instance = this;
        private void Start() => winPanel.SetActive(false);

        public void OnWin(string winner)
        {
            winPanel.SetActive(true);
            winnerName.text = $"{winner} won!";

            StartCoroutine(BackToMainMenuInSeconds(2));
        }

        private IEnumerator BackToMainMenuInSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            SceneManager.LoadScene("PlayerJoinMenu");
        }
    }
}