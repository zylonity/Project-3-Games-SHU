using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverButtons : MonoBehaviour
{
    private Canvas _canvas = null;
    private GameController gm = null;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameController._gameController;
        Debug.Assert(gm != null, "Game controller wasn't found", this);
        _canvas = gameObject.GetComponent<Canvas>();
        Debug.Assert(_canvas != null, "Canvas isn't assigned", this);
    }
    public void PlayAgainButtonPressed() 
    {
        gm.ChangeGameState(GameController.MyGameState.Game);
    }
    public void MainMenuButtonPressed()
    {
        gm.ChangeGameState(GameController.MyGameState.MainMenu);
    }
    // Update is called once per frame
    void Update()
    {
        if (gm.current_state == GameController.MyGameState.Over)
            _canvas.enabled = true;
    }
}
