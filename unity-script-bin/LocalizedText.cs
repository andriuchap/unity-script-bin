using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {

    [SerializeField]
    private string stringId = string.Empty;

    private Text textComponent;

	// Use this for initialization
	void Start () {
        textComponent = GetComponent<Text>();
        textComponent.text = LanguageManager.Instance.Get(stringId);
	}

    void OnEnable()
    {
        RefreshText();
    }

    public void RefreshText()
    {
        textComponent.text = LanguageManager.Instance.Get(stringId);
    }
}
