using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadMultiplayerLobbyScene()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
