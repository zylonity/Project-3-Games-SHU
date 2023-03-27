using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    private GameController gm = null;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameController._gameController;
        Debug.Assert(gm != null);
    }
    public void ButtonPressed()
    {
        gm.ChangeGameState(GameController.MyGameState.Game);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
