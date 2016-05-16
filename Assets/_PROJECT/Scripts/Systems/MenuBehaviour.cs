using UnityEngine;
using System.Collections;

public class MenuBehaviour : MonoBehaviour
{

    private PersistentBehaviour _pb;

    void Start()
    {
        _pb = PersistentBehaviour.GetInstance();
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("sectors", 0);
        PlayerPrefs.SetInt("levels", 0);
        _pb.ChangeLevel(2, 0.75f);
    }

    public void ContinueGame()
    {
        _pb.ChangeLevel(2, 0.75f);
    }

    public void Quit()
    {
        
    }

}
