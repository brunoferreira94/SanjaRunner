using UnityEngine;
using DG.Tweening;
using TMPro;

public class MapBehaviour : MonoBehaviour
{
    public SectorBehaviour[] Sectors;
    public SpriteRenderer LargerMapSprite;

    private static MapBehaviour _instance;
    private int _unlockedSectors;
    private int _unlockedLevels;

    public static MapBehaviour GetInstance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _unlockedLevels = PlayerPrefs.GetInt("levels");
        _unlockedSectors = PlayerPrefs.GetInt("sectors");

        UnlockSectors();
    }

    void UnlockSectors()
    {
        for (int counter = 0; counter < Sectors.Length; counter++)
        {
            if (counter <= _unlockedSectors)
            {
                Sectors[counter].IsLocked = false;
                Sectors[counter].UnlockLevels(_unlockedLevels);
            }
            Sectors[counter].UpdateLockedStatus();
        }
    }

    public void DisableOtherSectors(SectorBehaviour sector)
    {
        for (int counter = 0; counter < Sectors.Length; counter++)
        {
            if (Sectors[counter] != sector)
            {
                Sectors[counter].Shrink();
            }
        }

        LargerMapSprite.DOFade(0, 0.25f);
    }

    public void EnableAllSectors()
    {
        for (int counter = 0; counter < Sectors.Length; counter++)
        {
            Sectors[counter].gameObject.SetActive(true);
            Sectors[counter].ResetSector();
        }

        LargerMapSprite.DOFade(1, 0.25f);
    }

}
