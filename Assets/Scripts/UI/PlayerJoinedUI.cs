using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class PlayerJoinedUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_Dropdown classDropdown;
        [SerializeField] private Image colourImage;

        private int oldValue = 0;
        
        public int Colour { get; private set; }

        public void SetupDropdown(string[] values)
        {
            List<TMP_Dropdown.OptionData> options = values.Select(v => new TMP_Dropdown.OptionData(v)).ToList();

            classDropdown.ClearOptions();
            classDropdown.AddOptions(options);

            classDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

            nameInput.text = classDropdown.captionText.text;
            oldValue = 0;
        }

        private void OnDropdownValueChanged(int newValue)
        {
            if (classDropdown.options[oldValue].text == nameInput.text)
            {
                // Name hasn't been edited, so change when we change the CPU
                nameInput.text = classDropdown.captionText.text;
                oldValue = newValue;
            }

            Colour = PlayerJoinMenu.Brains.brains.First(b => b.brainName == classDropdown.options[newValue].text).brainColourIndex;
        }
        public PlayerSetupInfo GetData()
        {
            return new PlayerSetupInfo(nameInput.text, Colour, PlayerJoinMenu.GetBrainFromName(classDropdown.captionText.text));
        }
        public void Kill() => Destroy(gameObject);
    }
}
