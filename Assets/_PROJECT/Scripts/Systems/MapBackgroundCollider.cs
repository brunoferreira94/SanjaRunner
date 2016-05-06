using UnityEngine;
using System.Collections;

public class MapBackgroundCollider : MonoBehaviour
{

    private MapBehaviour _mb;

    void Start()
    {
        _mb = MapBehaviour.GetInstance();
    }

    void OnMouseDown()
    {
        _mb.EnableAllSectors();
    }
}
