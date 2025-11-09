using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerJoinedUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_Dropdown classDropdown;
    
        public void SetupDropdown(string[] values)
        {
            List<TMP_Dropdown.OptionData> options = values.Select(v => new TMP_Dropdown.OptionData(v)).ToList();

            classDropdown.ClearOptions();
            classDropdown.AddOptions(options);
        }
        public PlayerSetupInfo GetData()
        {
            return new PlayerSetupInfo(nameInput.text, PlayerJoinMenu.GetBrainFromName(classDropdown.captionText.text));
        }
        public void Kill() => Destroy(gameObject);
    }
}
