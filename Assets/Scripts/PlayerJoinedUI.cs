using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEditor.Search;
public class PlayerJoinedUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_Dropdown classDropdown;
    
    public void SetupDropdown(string[] values)
    {
        List<TMP_Dropdown.OptionData> options = new();

        foreach (string v in values) options.Add(new TMP_Dropdown.OptionData(v));

        classDropdown.ClearOptions();
        classDropdown.AddOptions(options);
    }
    public PlayerSetupInfo GetData()
    {
        return new PlayerSetupInfo(nameInput.text, PlayerJoinMenu.GetBrainFromName(classDropdown.captionText.text));
    }
    public void Kill() => Destroy(gameObject);
}
