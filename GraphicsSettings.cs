using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class GraphicsSettings : MonoBehaviour {
    //Post Processing profile asset
    public PostProcessingProfile Pprofile;

    //Menu UI dropdowns and toggles
    public Dropdown AntiAliasingTypeDropDown;
    public Dropdown AntiAliasingQualityDropDown;
    public Dropdown ResolutionDropDown;
    public Dropdown TexQualityDropDown;
    public Dropdown ShadowTypeDropDown;
    public Dropdown ShadowResDropDown;
    public Dropdown VSyncDropDown;
    public Toggle AmbientOcclusionToggle;
    public Toggle MotionBlurToggle;
    public Toggle BloomToggle;
    public Toggle ColorGradingToggle;
    public Toggle ChromaticAberrationToggle;
    public Toggle DitheringToggle;
    public Light[] AllLights;
    public Text ShadowResDropDownText;
    public AudioSource clip2;
    //Stores all resolutions
    Resolution[] SupportedResolutions;

    //class containing all graphics parameters
    public class SettingVars
    {
        public int AntiAliasingType;
        public int AntiAliasingQuality;
        public int Resolutions;
        public int TexQuality;
        public int ShadowType;
        public int ShadowRes;
        public int VSyncs;
    }

    //SettingsObject
    SettingVars vals;

    public void OnEnable()
    {
        //initialize object
        vals = new SettingVars();

        //assigning delegates [functions]
        AntiAliasingTypeDropDown.onValueChanged.AddListener(delegate { OnAntiAliasingTypeChange(); });
        AntiAliasingQualityDropDown.onValueChanged.AddListener(delegate { OnAntiAliasingQualityChange(); });
        ResolutionDropDown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        TexQualityDropDown.onValueChanged.AddListener(delegate { OnTexQualityChange(); });
        ShadowTypeDropDown.onValueChanged.AddListener(delegate { OnShadowTypeChange(); });
        ShadowResDropDown.onValueChanged.AddListener(delegate { OnShadowResChange(); });
        VSyncDropDown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        AmbientOcclusionToggle.onValueChanged.AddListener(delegate { OnAmbientOcclusionChange(); });
        MotionBlurToggle.onValueChanged.AddListener(delegate { OnMotionBlurChange(); });
        BloomToggle.onValueChanged.AddListener(delegate { OnBloomChange(); });
        ColorGradingToggle.onValueChanged.AddListener(delegate { OnColorGradingChange(); });
        ChromaticAberrationToggle.onValueChanged.AddListener(delegate { OnChromaticAberrationChange(); });
        DitheringToggle.onValueChanged.AddListener(delegate { OnDitheringChange(); });

        //Add all resolutions supported by the screen in the dropdown
        SupportedResolutions = Screen.resolutions;
        foreach (Resolution resolution in SupportedResolutions)
        {
            ResolutionDropDown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        //Loading the settings
        LoadSettings();
    }
	
    public void OnAntiAliasingTypeChange()
    {
        vals.AntiAliasingType = AntiAliasingTypeDropDown.value;

        //Antialiasing variable
        var AAsettings = Pprofile.antialiasing.settings;

        switch (AntiAliasingTypeDropDown.value)
        {
            case 0:
                AAsettings.method = AntialiasingModel.Method.Fxaa;
                AAsettings.fxaaSettings.preset = FXAAPresetReturner(AntiAliasingQuality());
                AntiAliasingQualityDropDown.value = AntiAliasingQuality();
                break;
            case 1:
                AAsettings.method = AntialiasingModel.Method.Taa;
                AAsettings.taaSettings = TAASettingsReturner(AntiAliasingQuality());
                AntiAliasingQualityDropDown.value = AntiAliasingQuality();
                break;
        }

        //Apply to asset
        Pprofile.antialiasing.settings = AAsettings;
        SaveSettings();
        clip2.Play();
    }

    public AntialiasingModel.FxaaPreset FXAAPresetReturner(int val)
    {
        switch (val)
        {
            case 0:
                return AntialiasingModel.FxaaPreset.ExtremePerformance;
            case 1:
                return AntialiasingModel.FxaaPreset.Performance;
            case 2:
                return AntialiasingModel.FxaaPreset.Default;
            case 3:
                return AntialiasingModel.FxaaPreset.Quality;
            case 4:
                return AntialiasingModel.FxaaPreset.ExtremePerformance;
        }
        return AntialiasingModel.FxaaPreset.Default;
    }

    public AntialiasingModel.TaaSettings TAASettingsReturner(int val)
    {
        var TAASettingsSaver = Pprofile.antialiasing.settings.taaSettings;
        switch (val)
        {
            case 0:
                TAASettingsSaver.jitterSpread = 0f;
                TAASettingsSaver.motionBlending = 0f;
                TAASettingsSaver.stationaryBlending = 0f;
                return TAASettingsSaver;
            case 1:
                TAASettingsSaver.jitterSpread = 0.25f;
                TAASettingsSaver.motionBlending = 0.25f;
                TAASettingsSaver.stationaryBlending = 0.25f;
                return TAASettingsSaver;
            case 2:
                TAASettingsSaver.jitterSpread = 0.5f;
                TAASettingsSaver.motionBlending = 0.5f;
                TAASettingsSaver.stationaryBlending = 0.5f;
                return TAASettingsSaver;
            case 3:
                TAASettingsSaver.jitterSpread = 0.75f;
                TAASettingsSaver.motionBlending = 0.75f;
                TAASettingsSaver.stationaryBlending = 0.75f;
                return TAASettingsSaver;
            case 4:
                TAASettingsSaver.jitterSpread = 0.99f;
                TAASettingsSaver.motionBlending = 0.99f;
                TAASettingsSaver.stationaryBlending = 0.99f;
                return TAASettingsSaver;
        }
        return TAASettingsSaver;
    }

    public void OnAntiAliasingQualityChange()
    {
        vals.AntiAliasingQuality = AntiAliasingQualityDropDown.value;

        //Antialiasing Variable
        var AAsettings = Pprofile.antialiasing.settings;

        switch (AntiAliasingQualityDropDown.value)
        {
            case 0:
                if(AAsettings.method == AntialiasingModel.Method.Fxaa)
                {
                    AAsettings.fxaaSettings.preset = AntialiasingModel.FxaaPreset.ExtremePerformance;
                }
                else
                {
                    AAsettings.taaSettings.jitterSpread = 0f;
                    AAsettings.taaSettings.motionBlending = 0f;
                    AAsettings.taaSettings.stationaryBlending = 0f;
                }
                break;
            case 1:
                if(AAsettings.method == AntialiasingModel.Method.Fxaa)
                {
                    AAsettings.fxaaSettings.preset = AntialiasingModel.FxaaPreset.Performance;
                }
                else
                {
                    AAsettings.taaSettings.jitterSpread = 0.25f;
                    AAsettings.taaSettings.motionBlending = 0.25f;
                    AAsettings.taaSettings.stationaryBlending = 0.25f;
                }
                break;
            case 2:
                if(AAsettings.method == AntialiasingModel.Method.Fxaa)
                {
                    AAsettings.fxaaSettings.preset = AntialiasingModel.FxaaPreset.Default;
                }
                else
                {
                    AAsettings.taaSettings.jitterSpread = 0.5f;
                    AAsettings.taaSettings.motionBlending = 0.5f;
                    AAsettings.taaSettings.stationaryBlending = 0.5f;
                }
                break;
            case 3:
                if(AAsettings.method == AntialiasingModel.Method.Fxaa)
                {
                    AAsettings.fxaaSettings.preset = AntialiasingModel.FxaaPreset.Quality;
                }
                else
                {
                    AAsettings.taaSettings.jitterSpread = 0.75f;
                    AAsettings.taaSettings.motionBlending = 0.75f;
                    AAsettings.taaSettings.stationaryBlending = 0.75f;
                }
                break;
            case 4:
                if(AAsettings.method == AntialiasingModel.Method.Fxaa)
                {
                    AAsettings.fxaaSettings.preset = AntialiasingModel.FxaaPreset.ExtremeQuality;
                }
                else
                {
                    AAsettings.taaSettings.jitterSpread = 0.99f;
                    AAsettings.taaSettings.motionBlending = 0.99f;
                    AAsettings.taaSettings.stationaryBlending = 0.99f;
                }
                break;
        }

        //Apply To asset
        Pprofile.antialiasing.settings = AAsettings;
        SaveSettings();
        clip2.Play();
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(Screen.resolutions[ResolutionDropDown.value].width, Screen.resolutions[ResolutionDropDown.value].height, Screen.fullScreen);
        vals.Resolutions = ResolutionDropDown.value;
        SaveSettings();
        clip2.Play();
    }

    public void OnTexQualityChange()
    {
        QualitySettings.masterTextureLimit = vals.TexQuality = TexQualityDropDown.value;
        SaveSettings();
        clip2.Play();
    }

    public void OnShadowTypeChange()
    {
        vals.ShadowType = ShadowTypeDropDown.value;
        switch (ShadowTypeDropDown.value)
        {
            case 0:
                QualitySettings.shadows = ShadowQuality.Disable;
                break;
            case 1:
                QualitySettings.shadows = ShadowQuality.HardOnly;
                break;
            case 2:
                QualitySettings.shadows = ShadowQuality.All;
                break;
        }

        // Make Changes in configuration of Lights
        ConfigureLights();
        SaveSettings();
        //check if the user has disabled shadows
        if(QualitySettings.shadows == ShadowQuality.Disable)
        {
            ShadowResDropDown.interactable = false;
            ShadowResDropDownText.color = new Color32((byte)170, (byte)170, (byte)170, (byte)255);
        }
        else
        {
            ShadowResDropDown.interactable = true;
            ShadowResDropDownText.color = new Color32((byte)170, (byte)0, (byte)0, (byte)255);
        }
        clip2.Play();
        SaveSettings();
    }

    public void OnShadowResChange()
    {
        vals.ShadowRes = ShadowResDropDown.value;
        switch (ShadowResDropDown.value)
        {
            case 0:
                QualitySettings.shadowResolution = ShadowResolution.Low;
                break;
            case 1:
                QualitySettings.shadowResolution = ShadowResolution.Medium;
                break;
            case 2:
                QualitySettings.shadowResolution = ShadowResolution.High;
                break;
            case 3:
                QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                break;
        }
        SaveSettings();
        clip2.Play();
    }

    public void OnVSyncChange()
    {
        vals.VSyncs = VSyncDropDown.value;
        QualitySettings.vSyncCount = vals.VSyncs;
        SaveSettings();
        clip2.Play();
    }

    public void OnAmbientOcclusionChange()
    {
        Pprofile.ambientOcclusion.enabled = AmbientOcclusionToggle.isOn;
        clip2.Play();
    }

    public void OnMotionBlurChange()
    {
        Pprofile.motionBlur.enabled = MotionBlurToggle.isOn;
        clip2.Play();
    }

    public void OnBloomChange()
    {
        Pprofile.bloom.enabled = BloomToggle.isOn;
        clip2.Play();
    }

    public void OnColorGradingChange()
    {
        Pprofile.bloom.enabled = ColorGradingToggle.isOn;
        clip2.Play();
    }

    public void OnChromaticAberrationChange()
    {
        Pprofile.chromaticAberration.enabled = ChromaticAberrationToggle.isOn;
        clip2.Play();
    }

    public void OnDitheringChange()
    {
        Pprofile.dithering.enabled = DitheringToggle.isOn;
        clip2.Play();
    }

    public void LoadSettings()
    {
        if (Application.isEditor)
        {
            if (!File.Exists(Application.persistentDataPath + "/gamesettings.json"))
            {
                AntiAliasingTypeDropDown.value = vals.AntiAliasingType = AntiAliasingType();
                AntiAliasingQualityDropDown.value = vals.AntiAliasingQuality = AntiAliasingQuality();
                ResolutionDropDown.value = vals.Resolutions = FindCurrentResolution();
                TexQualityDropDown.value = vals.TexQuality = QualitySettings.masterTextureLimit;
                ShadowTypeDropDown.value = vals.ShadowType = ShadowType();
                ShadowResDropDown.value = vals.ShadowRes = ShadowRes();
                VSyncDropDown.value = vals.VSyncs = QualitySettings.vSyncCount;
                AmbientOcclusionToggle.isOn = Pprofile.ambientOcclusion.enabled;
                MotionBlurToggle.isOn = Pprofile.motionBlur.enabled;
                BloomToggle.isOn = Pprofile.bloom.enabled;
                ColorGradingToggle.isOn = Pprofile.colorGrading.enabled;
                ChromaticAberrationToggle.isOn = Pprofile.chromaticAberration.enabled;
                DitheringToggle.isOn = Pprofile.dithering.enabled;
                string jsonData = JsonUtility.ToJson(vals, true);
                File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
            }
            else
            {
                vals = JsonUtility.FromJson<SettingVars>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
                AntiAliasingTypeDropDown.value = vals.AntiAliasingType;
                AntiAliasingQualityDropDown.value = vals.AntiAliasingQuality;
                ResolutionDropDown.value = vals.Resolutions;
                TexQualityDropDown.value = vals.TexQuality;
                ShadowTypeDropDown.value = vals.ShadowType;
                ShadowResDropDown.value = vals.ShadowRes;
                VSyncDropDown.value = vals.VSyncs;
                AmbientOcclusionToggle.isOn = Pprofile.ambientOcclusion.enabled;
                MotionBlurToggle.isOn = Pprofile.motionBlur.enabled;
                BloomToggle.isOn = Pprofile.bloom.enabled;
                ColorGradingToggle.isOn = Pprofile.colorGrading.enabled;
                ChromaticAberrationToggle.isOn = Pprofile.chromaticAberration.enabled;
                DitheringToggle.isOn = Pprofile.dithering.enabled;
            }
        }
        else
        {
            if (!File.Exists(Application.persistentDataPath + "/actualstandalonesettings.json"))
            {
                AntiAliasingTypeDropDown.value = vals.AntiAliasingType = AntiAliasingType();
                AntiAliasingQualityDropDown.value = vals.AntiAliasingQuality = AntiAliasingQuality();
                ResolutionDropDown.value = vals.Resolutions = FindCurrentResolution();
                TexQualityDropDown.value = vals.TexQuality = QualitySettings.masterTextureLimit;
                ShadowTypeDropDown.value = vals.ShadowType = ShadowType();
                ShadowResDropDown.value = vals.ShadowRes = ShadowRes();
                VSyncDropDown.value = vals.VSyncs = QualitySettings.vSyncCount;
                AmbientOcclusionToggle.isOn = Pprofile.ambientOcclusion.enabled;
                MotionBlurToggle.isOn = Pprofile.motionBlur.enabled;
                BloomToggle.isOn = Pprofile.bloom.enabled;
                ColorGradingToggle.isOn = Pprofile.colorGrading.enabled;
                ChromaticAberrationToggle.isOn = Pprofile.chromaticAberration.enabled;
                DitheringToggle.isOn = Pprofile.dithering.enabled;
                string jsonData = JsonUtility.ToJson(vals, true);
                File.WriteAllText(Application.persistentDataPath + "/actualstandalonesettings.json", jsonData);
            }
            else
            {
                vals = JsonUtility.FromJson<SettingVars>(File.ReadAllText(Application.persistentDataPath + "/actualstandalonesettings.json"));
                AntiAliasingTypeDropDown.value = vals.AntiAliasingType;
                AntiAliasingQualityDropDown.value = vals.AntiAliasingQuality;
                ResolutionDropDown.value = vals.Resolutions;
                TexQualityDropDown.value = vals.TexQuality;
                ShadowTypeDropDown.value = vals.ShadowType;
                ShadowResDropDown.value = vals.ShadowRes;
                VSyncDropDown.value = vals.VSyncs;
                AmbientOcclusionToggle.isOn = Pprofile.ambientOcclusion.enabled;
                MotionBlurToggle.isOn = Pprofile.motionBlur.enabled;
                BloomToggle.isOn = Pprofile.bloom.enabled;
                ColorGradingToggle.isOn = Pprofile.colorGrading.enabled;
                ChromaticAberrationToggle.isOn = Pprofile.chromaticAberration.enabled;
                DitheringToggle.isOn = Pprofile.dithering.enabled;
            }
        }
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(vals, true);
        if (Application.isEditor)
        {
            File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
        }
        else
        {
            File.WriteAllText(Application.persistentDataPath + "/actualstandalonesettings.json", jsonData);
        }
    }

    public int FindCurrentResolution()
    {
        int temp = 0;
        for(int x = 0; x < SupportedResolutions.Length; x++)
        {
            if(SupportedResolutions[x].width == Screen.width && SupportedResolutions[x].height == Screen.height)
            {
                temp = x;
                break;
            }
        }
        return temp;
    }

    public int ShadowType()
    {
        int val = 0;
        if(QualitySettings.shadows == ShadowQuality.Disable)
        {
            val = 0;
        }
        else if(QualitySettings.shadows == ShadowQuality.HardOnly)
        {
            val = 1;
        }
        else if(QualitySettings.shadows == ShadowQuality.All)
        {
            val = 2;
        }
        return val;
    }

    public int AntiAliasingType()
    {
        int val = 0;
        if(Pprofile.antialiasing.settings.method == AntialiasingModel.Method.Fxaa)
        {
            val = 0;
        }
        else
        {
            val = 1;
        }
        return val;
    }

    public int AntiAliasingQuality()
    {
        int val = 0;
        if(Pprofile.antialiasing.settings.method == AntialiasingModel.Method.Fxaa)
        {
            if (Pprofile.antialiasing.settings.fxaaSettings.preset == AntialiasingModel.FxaaPreset.ExtremePerformance)
            {
                val = 0;
            }
            else if(Pprofile.antialiasing.settings.fxaaSettings.preset == AntialiasingModel.FxaaPreset.Performance)
            {
                val = 1;
            }
            else if(Pprofile.antialiasing.settings.fxaaSettings.preset == AntialiasingModel.FxaaPreset.Default)
            {
                val = 2;
            }
            else if(Pprofile.antialiasing.settings.fxaaSettings.preset == AntialiasingModel.FxaaPreset.Quality)
            {
                val = 3;
            }
            else if(Pprofile.antialiasing.settings.fxaaSettings.preset == AntialiasingModel.FxaaPreset.ExtremePerformance)
            {
                val = 4;
            }
        }
        else
        {
            if(Pprofile.antialiasing.settings.taaSettings.jitterSpread == 0f)
            {
                val = 0;
            }
            else if(Pprofile.antialiasing.settings.taaSettings.jitterSpread == 0.25f)
            {
                val = 1;
            }
            else if(Pprofile.antialiasing.settings.taaSettings.jitterSpread == 0.5f)
            {
                val = 2;
            }
            else if (Pprofile.antialiasing.settings.taaSettings.jitterSpread == 0.75f)
            {
                val = 3;
            }
            else if (Pprofile.antialiasing.settings.taaSettings.jitterSpread == 0.99f)
            {
                val = 4;
            }
        }
        return val;
    }

    public int ShadowRes()
    {
        int val = 0;
        if(QualitySettings.shadowResolution == ShadowResolution.Low)
        {
            val = 0;
        }
        else if(QualitySettings.shadowResolution == ShadowResolution.Medium)
        {
            val = 1;
        }
        else if(QualitySettings.shadowResolution == ShadowResolution.High)
        {
            val = 2;
        }
        else if(QualitySettings.shadowResolution == ShadowResolution.VeryHigh)
        {
            val = 3;
        }
        return val;
    }

    public void ConfigureLights()
    {
        foreach(Light light in AllLights)
        {
            if(ShadowType() == 0)
            {
                light.shadows = LightShadows.None;
            }
            else if(ShadowType() == 1)
            {
                light.shadows = LightShadows.Hard;
            }
            else if(ShadowType() == 2)
            {
                light.shadows = LightShadows.Soft;
            }
        }
    }

}
