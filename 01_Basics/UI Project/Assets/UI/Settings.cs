using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class Settings : MonoBehaviour
{
    [SerializeField] UIDocument UiDoc;
    [SerializeField] AudioMixer AudioMixer;
    VisualElement m_root;

    private const string VolumeSliderName = "VolumeSlider";
    private const string QualityDropdownName = "QualityDropdown";
    private const string ResolutionDropdownName = "ResolutionDropdown";
    private const string FullscreenToggleName = "FullscreenToggle";

    Slider m_VolumeSlider;
    DropdownField m_ResolutionDropdown;
    EnumField m_QualityDropdown;
    Toggle m_FullscreenToggle;

    void OnEnable()
    {
        m_root = UiDoc.rootVisualElement;
        
        m_VolumeSlider = m_root.Q<Slider>(VolumeSliderName);
        m_ResolutionDropdown = m_root.Q<DropdownField>(ResolutionDropdownName);
        m_QualityDropdown = m_root.Q<EnumField>(QualityDropdownName);
        m_FullscreenToggle = m_root.Q<Toggle>(FullscreenToggleName);

        var resolutions = Screen.resolutions
            .Select(r => $"{r.width} x {r.height}")
            .ToList();

        m_ResolutionDropdown.choices.Clear();
        m_ResolutionDropdown.choices.AddRange(resolutions);


        m_ResolutionDropdown.value = "1920 x 1080";
        m_QualityDropdown.Init(GraphicsQuality.High);

        m_FullscreenToggle.RegisterValueChangedCallback<bool>(ToggleFullscreen);
        m_ResolutionDropdown.RegisterValueChangedCallback<string>(ChangeResolution);
        m_QualityDropdown.RegisterValueChangedCallback(ChangeGraphicsQuality);
        m_VolumeSlider.RegisterValueChangedCallback<float>(ChangeVolume);
    }

    private void ChangeVolume(ChangeEvent<float> evt)
    {
        AudioMixer.SetFloat("Volume",evt.newValue);
    }

    private void ChangeResolution(ChangeEvent<string> evt)
    {
        string[] parts = evt.newValue.Split(" x ");

       

        Screen.SetResolution(
            int.Parse(parts[0]),
            int.Parse(parts[1]),
            Screen.fullScreenMode
        );
        
    }

    private void ToggleFullscreen(ChangeEvent<bool> evt)
    {
        Screen.fullScreen = evt.newValue;
    }
    private void ChangeGraphicsQuality(ChangeEvent<Enum> evt)
    {
        GraphicsQuality quality = (GraphicsQuality)evt.newValue;

        int qualityIndex = 0;

        if(quality == GraphicsQuality.High)
        {
            qualityIndex = 2;
        }
        else if(quality == GraphicsQuality.Medium)
        {
            qualityIndex = 1;
        }


        QualitySettings.SetQualityLevel(qualityIndex);

    }

    private void OnDisable()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenSettings(VisualElement container, string className)
    {
        container.RemoveFromClassList(className);
    }
    public void CloseSettings(VisualElement container, string className)
    {
        container.AddToClassList(className);
    }
}

public enum GraphicsQuality
{
    Low,
    Medium,
    High
}