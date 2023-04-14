using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection
{
    None,
    Right,
    Left,
    Up,
    Down
};

public class GamePlayController : MogSingletonScene<GamePlayController>
{
    [Header("Prefabs")]
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject tileBoardPrefab;
   public GameObject packMan,_pacXonObject;
    [SerializeField] float horizontal, vertical;
    Rigidbody2D pacBody;
   public float speed = 100f;
    public MoveDirection moveDirection = MoveDirection.None;

    [Header("Boundary Variables")]
    float minXBordervalue = 0f;
    float maxXBordervalue = 19f;
    float minYBordervalue = 0f;
    float maxYBordervalue = 19f;

    [HideInInspector] public TileHolder _tileBoard;

    // Start is called before the first frame update
    void Start()
    {
        GameObject board = Instantiate(tileBoardPrefab) as GameObject;
        _tileBoard = board.GetComponent<TileHolder>();
        _tileBoard.InitTileBoard();
        pacBody= _pacXonObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveInDirection(MoveDirection.Left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveInDirection(MoveDirection.Right);
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveInDirection(MoveDirection.Up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveInDirection(MoveDirection.Down);
        }
        MovePacXon();
    }

    Vector2 direction = Vector2.zero;
    public void MoveInDirection(MoveDirection mov)
    {
        moveDirection = mov;
        switch (mov)
        {
            case MoveDirection.Right:
                direction = Vector2.right;
                _pacXonObject.transform.localScale = new Vector3(1, 1, 1);
                _pacXonObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case MoveDirection.Left:
                direction = Vector2.left;
                _pacXonObject.transform.localScale = new Vector3(-1, 1, 1);
                _pacXonObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case MoveDirection.Up:
                direction = Vector2.up;
                _pacXonObject.transform.localScale = new Vector3(1, 1, 1);
                _pacXonObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
            case MoveDirection.Down:
                direction = Vector2.down;
                _pacXonObject.transform.localScale = new Vector3(1, -1, 1);
                _pacXonObject.transform.localRotation = Quaternion.Euler(0, 0, 270);
                break;
        }
    }

    void MovePacXon()
    {
        if (moveDirection != MoveDirection.None)
        {
            Vector3 pos = _pacXonObject.transform.localPosition;
            pos += (Vector3)(direction);
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            float step = speed * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, minXBordervalue, maxXBordervalue);
            pos.y = Mathf.Clamp(pos.y, minYBordervalue, maxYBordervalue);
            _pacXonObject.transform.localPosition = Vector3.MoveTowards(_pacXonObject.transform.localPosition, pos, Time.deltaTime * speed);
            pos = _pacXonObject.transform.localPosition;
            Tile tile = _tileBoard.AddTiles(Mathf.Round(pos.x), Mathf.Round(pos.y));
            if (tile && tile.tileSprite.enabled )//&& moveDirection != MoveDirection.None)
            {
                _pacXonObject.transform.localPosition=pos;
                Debug.Log("calleddd;;;" + tile._positionX + "::" + tile._positionY);
                moveDirection = MoveDirection.None;
                _tileBoard.MinMaxValues();
            }      
        }
    }

}
