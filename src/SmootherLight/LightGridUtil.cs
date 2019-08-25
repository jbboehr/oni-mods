using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MightyVincent
{
    internal class LightGridUtil
    {
        private static readonly LineBlock[] _EMPTY = { };
        private static readonly Vector2 _LD = new Vector2(-0.5f, -0.5f);
        private static readonly Vector2 _RD = new Vector2(0.5f, -0.5f);
        private static readonly Vector2 _LU = new Vector2(-0.5f, 0.5f);
        private static readonly Vector2 _RU = new Vector2(0.5f, 0.5f);
        private static readonly LineBlock _DOWN_RL = new LineBlock(_RD, _LD);
        private static readonly LineBlock _UP_LR = new LineBlock(_LU, _RU);
        private static readonly LineBlock _LEFT_DU = new LineBlock(_LD, _LU);
        private static readonly LineBlock _RIGHT_UD = new LineBlock(_RU, _RD);
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
                    lightArea.AddBlocks(GetLineBlocksOf(deltaPoint));
                else if (!lightArea.IsBlocking(deltaPoint))
                    // 透光，不被遮挡
                    visiblePoints.Add(pointCell);
            }
        }

        private static IEnumerable<LineBlock> GetLineBlocksOf(Vector2I deltaPoint)
        {
            var dx = deltaPoint.x;
            var dy = deltaPoint.y;
            if (dx == 0) return dy == 0 ? _EMPTY : new[] {dy > 0 ? _DOWN_RL + deltaPoint : _UP_LR + deltaPoint};

            if (dx > 0)
            {
                if (dy == 0) return new[] {_LEFT_DU + deltaPoint};

                return dy > 0 ? new[] {_DOWN_RL + deltaPoint, _LEFT_DU + deltaPoint} : new[] {_LEFT_DU + deltaPoint, _UP_LR + deltaPoint};
            }

            if (dy == 0) return new[] {_RIGHT_UD + deltaPoint};

            return dy > 0 ? new[] {_RIGHT_UD + deltaPoint, _DOWN_RL + deltaPoint} : new[] {_UP_LR + deltaPoint, _RIGHT_UD + deltaPoint};
        }

        private static bool DoesOcclude(int cell)
        {
            return Grid.IsValidCell(cell) && !Grid.Transparent[cell] && Grid.Solid[cell];
        }
    }
}