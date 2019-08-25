using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MightyVincent
{
    internal class LightGridUtil
    {
        private static readonly HashSet<string> _MESH_TILE_IDS = new HashSet<string> {MeshTileConfig.ID, GasPermeableMembraneConfig.ID};

        public static void GetVisibleCells(int cell, List<int> visiblePoints, int range, LightShape shape)
        {
            Predicate<int> occludeFilter;
            if (State.Config.LightThroughMeshTiles)
            {
                var meshCellLookup = Components.BuildingCompletes.Items
                    .Where(complete => _MESH_TILE_IDS.Contains(complete.PrefabID().ToString()))
                    .ToLookup(complete => complete.GetCell());
                occludeFilter = i => DoesOcclude(i) && !meshCellLookup.Contains(i);
            }
            else
            {
                occludeFilter = DoesOcclude;
            }

            if (occludeFilter.Invoke(cell))
                return;

            var origin = Grid.CellToXY(cell);
            var lightArea = LightArea.Create(range, Vector2.zero);
            ICellEnumerator enumerator;
            switch (shape)
            {
                case LightShape.Circle:
                    enumerator = new RectBorder(origin, range);
                    break;
                case LightShape.Cone:
                    enumerator = new TrapezoidLayer(origin, range);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape), shape, null);
            }

            foreach (var point in enumerator)
            {
                /*
                - 不透光，边界判断
                  - 相交，扩充已有扇区
                  - 不相交，添加新扇区
                - 透光，中心判断
                  - 在扇区内，被遮挡
                  - 在扇区外，有光
                 */
                var deltaPoint = point - origin;
                if (!lightArea.InRange(deltaPoint))
                    // exceed
                    continue;

                var pointCell = Grid.PosToCell(point);
                if (occludeFilter.Invoke(pointCell))
                    // 不透光，更新障碍
                    lightArea.AddCellBlock(deltaPoint);
                else if (!lightArea.IsBlocking(deltaPoint))
                    // 透光，不被遮挡
                    visiblePoints.Add(pointCell);
            }
        }

        private static bool DoesOcclude(int cell)
        {
            return Grid.IsValidCell(cell) && !Grid.Transparent[cell] && Grid.Solid[cell];
        }
    }
}