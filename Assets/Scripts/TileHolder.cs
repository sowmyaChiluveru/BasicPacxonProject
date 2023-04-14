using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileData
{
    public int x, y;
    public TileData(int _x,int _y)
    {
        x = _x;
        y = _y;
    }
}

public class TileHolder : MonoBehaviour
{
    public List<Tile> _tileList;
    public GameObject tilePrefab;
    public List<int> positionsOfXTiles;
    public List<int> positionsOfYTiles;
    public List<Tile> tilesWithPositions;
    public int minX, minY, maxX, maxY;

    public void InitTileBoard()
    {
        positionsOfXTiles = new List<int>();
        positionsOfYTiles = new List<int>();

        for (int i = 0; i < 20; i++)
        {
            for(int j=0;j<20;j++)
            {
                CreateTileAt(i,j);
            }


            //CreateTileAt(i, 0);
            //CreateTileAt(i, 19);
            //if (i != 0 && i != 19)
            //{
            //    CreateTileAt(0, i);
            //    CreateTileAt(19, i);
            //}
        }
    }

    Tile CreateTileAt(int x, int y,bool setSprite=false)
    {
        bool _set = (x == 0 || x == 19 || y == 0 || y == 19) ? true : setSprite;
        Tile tileControl = GetBubbleAtGrid(x,y);
        if(tileControl==null)
        {
            GameObject tile = Instantiate(tilePrefab, transform, false) as GameObject;
            tile.transform.parent = transform;
            tile.transform.localPosition = new Vector3(x, y, 0);
            tile.name = "(" + x + "," + y + ")";
            tileControl = tile.GetComponent<Tile>() as Tile;
            if (x == 0 && y == 0)
            {
                GamePlayController.Instance._pacXonObject = Instantiate(GamePlayController.Instance.packMan, tile.transform, false) as GameObject;
            }
        }
        tileControl._positionX = x;
        tileControl._positionY = y;
        tileControl.SetTileSprite(_set);
        _tileList.Add(tileControl);
        SetBubbleLinks(tileControl);
       
        return tileControl;

    }

    void SetBubbleLinks(Tile tile)
    {
        int x = tile._positionX;
        int y = tile._positionY;
        int rx, ry;
        Tile temp = null;

        //LEFT
        temp = null;
        rx = x - 1;
        ry = y;
        temp = GetBubbleAtGrid(rx, ry);
        if (temp != null)
        {
            tile.linkedTiles.Add(temp);
            if (!temp.linkedTiles.Contains(tile))
                temp.linkedTiles.Add(tile);
        }
        // TOP
        temp = null;
        rx = x;// - (1 - (CheckLeftDisplacement(y) ? 0 : 1));
        ry = y + 1;
        temp = GetBubbleAtGrid(rx, ry);
        if (temp != null)
        {
            tile.linkedTiles.Add(temp);
            if (!temp.linkedTiles.Contains(tile))
                temp.linkedTiles.Add(tile);
        }
        // RIGHT
        temp = null;
        rx = x + 1;
        ry = y;
        temp = GetBubbleAtGrid(rx, ry);
        if (temp != null)
        {
            tile.linkedTiles.Add(temp);
            if (!temp.linkedTiles.Contains(tile))
                temp.linkedTiles.Add(tile);
        }
        // BOTTOM
        temp = null;
        rx = x;// - (1 - (CheckLeftDisplacement(y) ? 0 : 1));
        ry = y - 1;
        temp = GetBubbleAtGrid(rx, ry);
        if (temp != null)
        {
            tile.linkedTiles.Add(temp);
            if (!temp.linkedTiles.Contains(tile))
                temp.linkedTiles.Add(tile);
        }
    }

    Tile GetBubbleAtGrid(int x, int y)
    {
        foreach (Tile b in _tileList)
        {
            if (b == null)
                continue;
            if (b._positionX == x && b._positionY == y)
                return b;
        }
        return null;
    }

    public Tile AddTiles(float _x,float _y)
    {
        int x = (int)Mathf.Round(_x);
        int y = (int)Mathf.Round(_y);

        Tile tile = GetBubbleAtGrid(x,y);
        if(tile==null)
        {
            tile = CreateTileAt(x, y, false);            
        }
        tile.SetBackgroundColor(true);
        positionsOfXTiles.Add(x);
        positionsOfYTiles.Add(y);
        tilesWithPositions.Add(tile);
        return tile;
    }

   public void MinMaxValues()
    {
        if (positionsOfXTiles.Count > 0 && positionsOfYTiles.Count > 0)
        {
            minX = positionsOfXTiles.Min();
            minY = positionsOfYTiles.Min();
            maxX = positionsOfXTiles.Max();
            maxY = positionsOfYTiles.Max();

            for(int i=0;i<tilesWithPositions.Count;i++)
            {
                List<Tile> _t = tilesWithPositions[i].linkedTiles;
                if(_t.Count>0)
                {
                    for(int k=0;k<_t.Count;k++)
                    {
                        if(!_t[k].isColored &&(_t[k]._positionX>=minX&& _t[k]._positionX <= maxX)&& (_t[k]._positionY >= minY && _t[k]._positionY <= maxY))
                        {
                            Debug.Log("The tile initial:::" + _t[k]._positionX + "::" + _t[k]._positionY);
                            FloodFill(readTexture: null, sourceColor: new Color(1, 1, 1), 0f, _t[k]);
                            positionsOfXTiles.Clear();
                            positionsOfYTiles.Clear();
                            tilesWithPositions.Clear();
                            return;
                        }
                    }
                }
            }


            //for (int i = minX; i <= maxX; i++)
            //{
            //    for (int j = minY; j <= maxY; j++)
            //    {
            //        Tile _tile = GetBubbleAtGrid(i,j);
            //        if (_tile != null && !_tile.isColored)
            //            FloodFill(readTexture:null,sourceColor:new Color(1,1,1),0f,_tile);
            //        break;
            //    }
            //}
            positionsOfXTiles.Clear();
            positionsOfYTiles.Clear();
            tilesWithPositions.Clear();
        }
    }

    List<TileData> GetSameColumnCornerTiles(int x,int y)
    {
        List<TileData> list = new List<TileData>();
       // list = tilesWithPositions.FindAll(t => (int)t.x==x);
        return list;
    }

    public  void FloodFill(Texture2D readTexture, Color sourceColor, float tollerance,Tile _tile)
    {
        var targetColor = Color.red;
        var q = new Queue<Tile>();
        q.Enqueue(_tile);
        int iterations = 0;

        var width = 19;// readTexture.width;
        var height = 19;// readTexture.height;
        while (q.Count > 0)
        {
            var _t = q.Dequeue();
            var x1 = _t._positionX;
            var y1 = _t._positionY;
            if (q.Count > width * height)
            {
                throw new System.Exception("The algorithm is probably looping. Queue size: " + q.Count);
            }

            if (_t.isColored)// writeTexture.GetPixel(x1, y1) == targetColor)
            {
                continue;
            }
            _t.SetBackgroundColor(true);
            _t.SetTileSprite(true);
          //  writeTexture.SetPixel(x1, y1, targetColor);
          for(int i=0;i<_t.linkedTiles.Count;i++)
            {
                Tile tile = _t.linkedTiles[i];
                if (CheckValidity( width, height, tile, sourceColor, tollerance))
                    q.Enqueue(tile);
            }

            //var newPoint = new Point(x1 + 1, y1);
            //if (CheckValidity(readTexture, readTexture.width, readTexture.height, newPoint, sourceColor, tollerance))
            //    q.Enqueue(newPoint);

            //newPoint = new Point(x1 - 1, y1);
            //if (CheckValidity(readTexture, readTexture.width, readTexture.height, newPoint, sourceColor, tollerance))
            //    q.Enqueue(newPoint);

            //newPoint = new Point(x1, y1 + 1);
            //if (CheckValidity(readTexture, readTexture.width, readTexture.height, newPoint, sourceColor, tollerance))
            //    q.Enqueue(newPoint);

            //newPoint = new Point(x1, y1 - 1);
            //if (CheckValidity(readTexture, readTexture.width, readTexture.height, newPoint, sourceColor, tollerance))
            //    q.Enqueue(newPoint);

            iterations++;
        }
    }

     bool CheckValidity(int width, int height, Tile p, Color sourceColor, float tollerance)
    {
        if (p._positionX < minX || p._positionX > maxX)
        {
            return false;
        }
        if (p._positionY < minY || p._positionY > maxY)
        {
            return false;
        }

       // var color = texture.GetPixel(p.x, p.y);

        //var distance = Mathf.Abs(color.r - sourceColor.r) + Mathf.Abs(color.g - sourceColor.g) + Mathf.Abs(color.b - sourceColor.b);
        return !p.isColored;
    }


}
