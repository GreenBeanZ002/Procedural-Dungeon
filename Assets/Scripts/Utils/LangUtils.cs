using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class LangUtils : MonoBehaviour
{
    public static LangUtils Instance;
    private Dictionary<string, string> localizedText;
    public string langKey = "en_us";

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
            LoadLang(langKey);
        }
    }

    public void LoadLang(string key) 
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, $"lang_{key}.json");

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            localizedText = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
        }
        else
        {
            Debug.LogError("Lang file not found:" + filePath);
        }
    }

    public string GetText(string key) 
    {
        return localizedText.ContainsKey(key) ? localizedText[key] : key;
    }

    public string GetLanguagekey(){return langKey;}

    public void SetLanguageKey(string newKey){langKey = newKey;LoadLang(newKey);}
}
