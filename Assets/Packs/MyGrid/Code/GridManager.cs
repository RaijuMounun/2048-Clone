using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyGrid
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public List<TileController> Tiles => listTile;
        public List<TileController> listTile;

        public List<List<TileController>> GetPriorityTile(Direction dir)
        {
            List<List<TileController>> result = new List<List<TileController>>();

            bool isMax = dir == Direction.Up || dir == Direction.Right;
            bool isVertical = dir == Direction.Up || dir == Direction.Down;

            int current = isMax ? 3 : 0;
            for (int i = 0; i < 4; i++)
            {
                List<TileController> list = new List<TileController>();
                foreach (TileController item in listTile)
                {
                    int coord = isVertical ? item.coordinate.y : item.coordinate.x;
                    if (coord == current) list.Add(item);
                }

                result.Add(list);
                current += isMax ? -1 : 1;
            }
            return result;
        }

        public TileController GetTile(Vector2Int coordinate)
        {
            return listTile.Find(item => item.coordinate == coordinate);
        }

        public List<TileController> GetEmptyTile()
        {
            List<TileController> result = new List<TileController>();
            foreach (TileController tile in listTile)
                if (!tile.MyNumberController) result.Add(tile);

            return result;
        }

        public void SetTiles(List<TileController> tiles)
        {
            listTile = tiles;
        }
    }
}