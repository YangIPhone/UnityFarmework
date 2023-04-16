using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQFramework.CQTacticsToolkit{
    public class ShapeParser
    {
        public List<OverlayTile> GetAbilityTileLocations(OverlayTile abilityPosition, TextAsset defaultAbility, Vector2Int characterPosition,int abilityHeight)
        {
            int[,] abilityMap = mapAbility(defaultAbility);

            var map = MapManager.Instance.map;
            /// /找到1，1是能力的释放点
            List<Vector2> originLocation = FindLocationsInArray(1, abilityMap, null);

            var affectedTileLocations = new List<Vector2Int>();

            //2是技能命中的瓦片
            if (SearchAbilityMapForValue(2, abilityMap))
                affectedTileLocations.AddRange(GetShapeAbilityTiles(abilityPosition, characterPosition, abilityMap, originLocation));

            //3是一行瓷砖。只要是3，它就会把那个方向距离在10以内的瓦片都拿出来
            if (SearchAbilityMapForValue(3, abilityMap))
                affectedTileLocations.AddRange(GetLineAbilityTiles(abilityPosition, characterPosition, abilityMap, originLocation));


            var affectedTiles = new List<OverlayTile>();
            Vector3Int locationWithZ = Vector3Int.zero; 
            foreach (var location in affectedTileLocations)
            {
                for(int z = -abilityHeight;z<=abilityHeight;z++){
                    locationWithZ = new Vector3Int(location.x,location.y,abilityPosition.gridLocation.z+z);
                    if (map.ContainsKey(locationWithZ)) affectedTiles.Add(map[locationWithZ]);
                }
            }

            return affectedTiles;
        }

        //convert text file to a nested array. 
        private static int[,] mapAbility(TextAsset defaultAbility)
        {
            int i = 0;
            int[,] abilityMap = new int[10, 10];
            foreach (var row in defaultAbility.ToString().Split('\n'))
            {
                int j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    abilityMap[i, j] = int.Parse(col.Trim());
                    j++;
                }
                i++;
            }

            return abilityMap;
        }

        private List<Vector2Int> GetShapeAbilityTiles(OverlayTile abilityTile, Vector2Int characterPos, int[,] abilityMap, List<Vector2> originLocation)
        {
            List<Vector2> abilityAffectedTiles = new List<Vector2>();
            var abilityPosition = abilityTile.grid2DLocation;
            if (originLocation.Count > 0)
            {
                abilityAffectedTiles = FindLocationsInArray(2, abilityMap, originLocation[0]);
            }

            List<Vector2Int> affectedTileLocations = new List<Vector2Int>();

            foreach (var tileOffset in abilityAffectedTiles)
            {
                affectedTileLocations.Add(GetAffectedTile(ref abilityPosition, Vector2Int.FloorToInt(tileOffset), characterPos));
            }

            return affectedTileLocations;
        }

        private List<Vector2Int> GetLineAbilityTiles(OverlayTile abilityTile, Vector2Int characterPosition, int[,] abilityMap, List<Vector2> originLocation)
        {
            List<Vector2> abilityAffectedTiles = new List<Vector2>();
            var abilityPosition = abilityTile.grid2DLocation;

            if (originLocation.Count > 0)
            {
                abilityAffectedTiles = FindLocationsInArray(3, abilityMap, originLocation[0]);
            }

            List<Vector2Int> affectedTileLocations = new List<Vector2Int>();

            foreach (var tileOffset in abilityAffectedTiles)
            {
                var affectedTiles = GetAffectedTilesInALine(ref abilityPosition, Vector2Int.FloorToInt(tileOffset), characterPosition);
                affectedTileLocations.AddRange(affectedTiles);
            }

            return affectedTileLocations;
        }


        public List<Vector2Int> GetAffectedTilesInALine(ref Vector2Int originPosition, Vector2Int tileOffset, Vector2Int charpos)
        {
            int lenght = 10;
            Vector2Int absDirectionFromActiveCharacter;
            Vector2Int directionFromActiveCharacter;

            List<Vector2Int> positions = new List<Vector2Int>();
            absDirectionFromActiveCharacter = new Vector2Int(Mathf.Abs(originPosition.x - charpos.x), Mathf.Abs(originPosition.y - charpos.y));
            directionFromActiveCharacter = originPosition - charpos;

            if (absDirectionFromActiveCharacter.x >= absDirectionFromActiveCharacter.y)
            {
                if (directionFromActiveCharacter.x <= 0)
                {
                    positions.Add(new Vector2Int(originPosition.x - tileOffset.x, originPosition.y - tileOffset.y));

                    for (int i = 1; i < lenght; i++)
                    {
                        positions.Add(new Vector2Int(positions[0].x - i, positions[0].y));
                    }
                }
                else
                {
                    positions.Add(new Vector2Int(originPosition.x + tileOffset.x, originPosition.y + tileOffset.y));

                    for (int i = 1; i < lenght; i++)
                    {
                        positions.Add(new Vector2Int(positions[0].x + i, positions[0].y));
                    }
                }
            }
            else
            {
                if (directionFromActiveCharacter.y <= 0)
                {
                    positions.Add(new Vector2Int(originPosition.x - tileOffset.y, originPosition.y - tileOffset.x));

                    for (int i = 1; i < lenght; i++)
                    {
                        positions.Add(new Vector2Int(positions[0].x, positions[0].y - i));
                    }
                }
                else
                {
                    positions.Add(new Vector2Int(originPosition.x + tileOffset.y, originPosition.y + tileOffset.x));

                    for (int i = 1; i < lenght; i++)
                    {
                        positions.Add(new Vector2Int(positions[0].x, positions[0].y + i));
                    }
                }
            }

            return positions;
        }


        public Vector2Int GetAffectedTile(ref Vector2Int originPosition, Vector2Int tileOffset, Vector2Int charpos)
        {
            Vector2Int absDirectionFromActiveCharacter;
            Vector2Int directionFromActiveCharacter;

            absDirectionFromActiveCharacter = new Vector2Int(Mathf.Abs(originPosition.x - charpos.x), Mathf.Abs(originPosition.y - charpos.y));
            directionFromActiveCharacter = originPosition - charpos;

            if (absDirectionFromActiveCharacter.x >= absDirectionFromActiveCharacter.y)
            {
                if (directionFromActiveCharacter.x <= 0)
                {
                    return new Vector2Int(originPosition.x - tileOffset.x, originPosition.y - tileOffset.y);
                }
                else
                {
                    return new Vector2Int(originPosition.x + tileOffset.x, originPosition.y + tileOffset.y);
                }
            }
            else
            {
                if (directionFromActiveCharacter.y <= 0)
                {
                    return new Vector2Int(originPosition.x - tileOffset.y, originPosition.y - tileOffset.x);
                }
                else
                {
                    return new Vector2Int(originPosition.x + tileOffset.y, originPosition.y + tileOffset.x);
                }
            }
        }

        public List<Vector2> FindLocationsInArray(int value, int[,] abilityMap, Vector2? offsetLocation)
        {
            List<Vector2> affectedTileLocations = new List<Vector2>();
            for (int row = 0; row < abilityMap.GetLength(0); row++)
            {
                for (int col = 0; col < abilityMap.GetLength(1); col++)
                {
                    if (abilityMap[row, col] == value)
                    {
                        affectedTileLocations.Add(new Vector2(offsetLocation.HasValue ? offsetLocation.Value.x - row : row, offsetLocation.HasValue ? offsetLocation.Value.y - col : col));
                    }
                }
            }

            return affectedTileLocations;
        }

        private bool SearchAbilityMapForValue(int value, int[,] abilityMap)
        {
            for (int row = 0; row < abilityMap.GetLength(0); row++)
            {
                for (int col = 0; col < abilityMap.GetLength(1); col++)
                {
                    if (abilityMap[row, col] == value)
                        return true;
                }
            }

            return false;
        }
    }

}
