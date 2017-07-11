using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is used to initiate all the necessary input variables and game objects and supply all the necessary references
/// </summary>
public class VRInputMain : MonoBehaviour {

    [SerializeField]
    private VRCursor cursorObj;

    [SerializeField]
    private LayerMask interactionLayers;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        CreateInputObjects();
        SceneManager.sceneLoaded += SceneLoaded;
	}

    private void SceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        CreateInputObjects();
    }

    void CreateInputObjects()
    {
        VRInput input = Camera.main.gameObject.AddComponent<VRInput>();
        VRCursor newCursor = Instantiate(cursorObj);
        input.SetCursor(newCursor);
        input.SetCursorEnabled(false);
        input.SetInputEnabled(false);
        input.InputMask = interactionLayers;
    }
}
