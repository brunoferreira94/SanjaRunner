using UnityEngine;
using DG.Tweening;
using TMPro;

public class MapBehaviour : MonoBehaviour
{
    public SectorBehaviour[] Sectors;
    public SpriteRenderer LargerMapSprite;

    private static MapBehaviour _instance;

    public static MapBehaviour GetInstance()
    {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
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
