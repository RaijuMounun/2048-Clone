﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyGrid
{
    [Serializable]
    public struct TileData
    {
        public TileController tile;
        public Direction direction;
    }

    public class TileController : MonoBehaviour
    {

        public NumberController MyNumberController;
        public bool isNew;

        [SerializeField] private List<TileData> neighbours;

        public Vector2Int coordinate;

        public TileController GetNeighbour(Direction direction)
        {
            var data = neighbours.Find(x => x.direction == direction);
            return data.tile;
        }

        public List<TileController> GetAllNeighbours()
        {
            var result = new List<TileController>();
            foreach (var neighbour in neighbours)
                result.Add(neighbour.tile);

            return result;
        }

        public void PrepareNeighbour(List<TileData> data)
        {
            neighbours = new List<TileData>(data);
        }
    }
}