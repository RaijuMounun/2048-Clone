using System.Collections.Generic;
using UnityEngine;
using MyGrid;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] NumberController numberPrefab;
    GameObject numberParent;
    [SerializeField] GameObject[] canvases;
    [SerializeField] KeyCode[] keyCodes;
    [SerializeField] Direction[] directions;


    private void Awake()
    {
        numberParent = new GameObject("NumberParent");
    }

    private void Start()
    {
        SpawnNumber();
    }


    private void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (!Input.anyKey) return;

        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                Move(directions[i]);
                break;
            }
        }
    }


    void Move(Direction dir)
    {
        foreach (TileController item in GridManager.Instance.listTile) item.isNew = false;

        List<List<TileController>> result = GridManager.Instance.GetPriorityTile(dir);
        List<GameObject> listDestroy = new List<GameObject>();

        foreach (List<TileController> listTile in result)
        {
            foreach (TileController tile in listTile)
            {
                if (!tile.MyNumberController) continue;

                bool isMerge = false;
                TileController target = tile;
                TileController next = tile;
                for (int i = 0; i < 4; i++)
                {
                    next = next.GetNeighbour(dir);
                    if (!next) break;
                    if (next.MyNumberController)
                    {
                        if (!next.isNew && next.MyNumberController.Number == tile.MyNumberController.Number)
                        {
                            isMerge = true;
                            target = next;
                        }

                        break;
                    }
                    target = next;
                }

                if (isMerge)
                {
                    int value = tile.MyNumberController.Number;
                    if (value == 11)
                    {
                        GameOver(1);
                        return;
                    }
                    listDestroy.Add(tile.MyNumberController.gameObject);
                    listDestroy.Add(target.MyNumberController.gameObject);
                    tile.MyNumberController = null;
                    target.MyNumberController = null;
                    value++;
                    Spawn(target, value);
                    continue;
                }


                if (target == tile) continue;

                target.MyNumberController = tile.MyNumberController;
                tile.MyNumberController = null;
                target.MyNumberController.transform.position = target.transform.position;
            }
        }

        foreach (GameObject item in listDestroy) Destroy(item);

        SpawnNumber();
    }

    public void Spawn(TileController tile, int numberValue)
    {
        NumberController number = Instantiate(numberPrefab);
        number.transform.parent = numberParent.transform;
        number.Number = numberValue;
        number.transform.position = tile.transform.position;
        tile.MyNumberController = number;
        tile.isNew = true;
    }



    public void SpawnNumber()
    {
        NumberController number = Instantiate(numberPrefab);
        number.transform.parent = numberParent.transform;

        bool isOne = Random.value < 0.75f;
        number.Number = isOne ? 1 : 2;

        List<TileController> listEmptyTile = GridManager.Instance.GetEmptyTile();


        if (listEmptyTile.Count == 0)
        {
            if (number.transform.position == Vector3.zero) GameOver(0);
            return;
        }
        int index = Random.Range(0, listEmptyTile.Count);
        TileController tile = listEmptyTile[index];

        tile.MyNumberController = number;

        number.transform.position = tile.transform.position;
    }


    void GameOver(int winLose)
    {
        canvases[winLose].SetActive(true);
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
