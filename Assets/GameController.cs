using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{  
    public enum MyGameState { MainMenu, Game, Paused, Over }
    public MyGameState current_state = MyGameState.MainMenu;
    public static GameController _gameController = null;
    private string loading_scene = "";

    void Awake()
    { 
        // Assert we don't already have a game controller
        Debug.Assert(_gameController == null, this.gameObject);
        // Assign our static reference to this one we just created
        _gameController = this;
    }
    // Update is called once per frame
    void Update()
    {
        switch (current_state)
        {
            case MyGameState.MainMenu:
                // start the game
                if (Input.GetKeyDown(KeyCode.P))
                {
                    ChangeGameState(MyGameState.Game);
                }
                break;
            case MyGameState.Game:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ChangeGameState(MyGameState.Paused);
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    UpdateScene("Game");
                }
                break;
            case MyGameState.Paused:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ChangeGameState(MyGameState.Game);
                }


                break;
            case MyGameState.Over:


                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // set loading scene active
        if (loading_scene != "" && SceneManager.GetSceneByName(loading_scene).isLoaded)
        {
            // Set the loaded scene as active
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(loading_scene));
            // clear the loading scene
            loading_scene = "";
        }
    }
    public void ChangeGameState(MyGameState _myGameState)
    {
        Debug.Log("#Change State from " + current_state + " to " + _myGameState);
        // do something if current state has changed
        switch (_myGameState)
        {
            case MyGameState.MainMenu:
                UpdateScene("Game", true);
                UpdateScene("MainMenu");
                Time.timeScale = 1.0f;
                break;
            case MyGameState.Game:
                if (current_state == MyGameState.MainMenu)
                {
                    UpdateScene("MainMenu", true);
                    UpdateScene("Game");
                }
                Debug.Log("Time Scale set to 1.0");
                Time.timeScale = 1.0f;
                break;
            case MyGameState.Paused:
                Debug.Log("Time Scale set to 0.0");
                Time.timeScale = 0.0f;
                break;
            case MyGameState.Over:
                Debug.Log("Time Scale set to 0.0");
                Time.timeScale = 0.0f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // assign new state to the game
        current_state = _myGameState;
    }
    /// <summary>
    /// Restarts scene
    /// </summary>
    /// <param name="name"> scene name </param>
    /// <param name="stop"> if true scene would be unload an wouldn't be restart</param>
    private void UpdateScene(string name, bool stop = false)
    {
        if (SceneManager.GetSceneByName(name).isLoaded)
        {
            Debug.Log("Unloading " + name + " scene...");
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(name));
            
        }
        // load new scene
        if(!stop)
        {
            // Unload current scene
            Debug.Log("Loading a new instance of " + name + " scene...");
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
            loading_scene = name;
        }
    }
}
