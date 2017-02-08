using UnityEngine;
using System.Xml;
using System.IO;

public class LanguageManager : ScriptableObject {

    private static LanguageManager instance = null;

    public static LanguageManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = CreateInstance<LanguageManager>();
            }
            return instance;
        }
    }

    private XmlDocument mainDoc = null;
    private XmlElement root = null;
    private string languagePath = string.Empty;
    private string[] languageFiles = null;

    void Awake()
    {
        languagePath = "Languages/";
        //CollectLanguages();
    }

    void CollectLanguages()
    {
        try
        {
            DirectoryInfo langDir = new DirectoryInfo(languagePath);
            FileInfo[] files = langDir.GetFiles("*.xml");

            languageFiles = new string[files.Length];

            int i = 0;

            foreach(FileInfo f in files)
            {
                languageFiles[i] = f.FullName;
                i++;
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    string GetLanguageFile(string language)
    {
        foreach(string langGo in languageFiles)
        {
            if (langGo.EndsWith(language + ".xml"))
            {
                return langGo;
            }
        }
        return string.Empty;
    }

    public void LoadLanguage(string language)
    {
        /*try
        {
            string loadFile = GetLanguageFile(language);
            mainDoc = new XmlDocument();
            StreamReader sr = new StreamReader(loadFile);
            mainDoc.Load(sr);
            root = mainDoc.DocumentElement;
            sr.Close();
        }
        catch(System.Exception e)
        {
            Debug.Log(e.Message);
        }*/
        TextAsset xmlText = Resources.Load<TextAsset>(languagePath + language);
        if (xmlText == null)
        {
            Debug.Log("cant find language " + language + ", switching to default French");
            xmlText = Resources.Load<TextAsset>(languagePath + "French");
        }
        mainDoc = new XmlDocument();
        mainDoc.Load(new StringReader(xmlText.text));
        root = mainDoc.DocumentElement;
    }

    public string Get(string path)
    {
        path = "Strings/"+path;
        try {
            XmlNode node = root.SelectSingleNode(path);
            if (node == null)
            {
                return path;
            }
            else
            {
                string val = node.InnerText;
                val = val.Replace("\\n", "\n");
                return val;
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e.Message);
            return path;
        }
    }

    public void ChangeLanguage(string lang)
    {
        LoadLanguage(lang);
        LocalizedText[] texts = FindObjectsOfType<LocalizedText>();
        if (texts != null)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].RefreshText();
            }
        }
    }
}
