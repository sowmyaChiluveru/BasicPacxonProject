using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int _positionX = -1, _positionY = -1;
    public List<Tile> linkedTiles;
    public GameObject backgroundTile;
    public SpriteRenderer tileSprite;
    public bool isColored=false;

    public void SetTileSprite(bool canShow)
    {
        tileSprite.enabled = canShow;
        isColored = canShow;
    }

    public void SetBackgroundColor(bool canShow)
    {
        backgroundTile.SetActive(canShow);
        isColored = canShow;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

}
