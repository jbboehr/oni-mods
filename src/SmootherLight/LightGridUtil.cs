using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace MightyVincent
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class LightGridUtil
    {
        private static readonly Vector2 RU = new Vector2(0.5f, 0.5f);
        private static readonly Vector2 LU = new Vector2(-0.5f, 0.5f);
        private static readonly Vector2 LD = new Vector2(-0.5f, -0.5f);
        private static readonly Vector2 RD = new Vector2(0.5f, -0.5f);

        public static Sector2 RectToSector(Vector2I deltaPoint)
        {
            var dx = deltaPoint.x;
            var dy = deltaPoint.y;
            Vector2 startDir, endDir;
            if (dx == 0)
            {
                if (dy == 0)
                {
                    throw new ArgumentException("Impossible to be ZERO");
                }

                if (dy > 0)
                {
                    startDir = RD;
                    endDir = LD;
                }
                else
                {
                    startDir = LU;
                    endDir = RU;
                }
            }
            else if (dx > 0)
            {
                if (dy == 0)
                {
                    startDir = LD;
                    endDir = LU;
                }
                else if (dy > 0)
                {
                    startDir = RD;
                    endDir = LU;
                }
                else
                {
                    startDir = LD;
                    endDir = RU;
                }
            }
            else
            {
                if (dy == 0)
                {
                    startDir = RU;
                    endDir = RD;
                }
                else if (dy > 0)
                {
                    startDir = RU;
                    endDir = LD;
                }
                else
                {
                    startDir = LU;
                    endDir = RD;
                }
            }

            var start = new Vector2(dx + startDir.x, dy + startDir.y);
            var end = new Vector2(dx + endDir.x, dy + endDir.y);
            return new Sector2(start, end);
        }

        public static bool DoesOcclude(int cell)
        {
            return Grid.IsValidCell(cell) && !Grid.Transparent[cell] && Grid.Solid[cell];
        }

        public static float DeltaAngle(Vector2 from, Vector2 to)
        {
            var angle = Vector2.Angle(from, to);
            if (from.x * to.y - from.y * to.x < 0)
            {
                // negative
                angle = 360f - angle;
            }

            return angle;
        }

        public static void GetVisibleCells(int cell, List<int> visiblePoints, int range, LightShape shape)
        {
            if (DoesOcclude(cell))
                return;
            visiblePoints.Add(cell);

            var xy0 = Grid.CellToXY(cell);
            var blockedSectors = new List<Sector2>();
            const float tolerance = 0.1f;
            switch (shape)
            {
                case LightShape.Circle:
                    break;
                case LightShape.Cone:
                    if (!DoesOcclude(cell - 1)) visiblePoints.Add(cell - 1);
                    if (!DoesOcclude(cell + 1)) visiblePoints.Add(cell + 1);
                    blockedSectors.Add(new Sector2(Vector2.right + RD, Vector2.left + LD));
                    break;
                default:
                    return;
            }

            var sqrRange = (range + 0.5f) * (range + 0.5f);
            blockedSectors.ForEach(sector => Debug.Log(sector.ToString()));

            for (var depth = 1; depth <= range; depth++)
            {
                Debug.Log($"depth: {depth}");
                var side = depth * 2 + 1;
                var rectBorder = new RectBorder(xy0.x - depth, xy0.y - depth, side, side);
                foreach (var point in rectBorder)
                {
                    /*
                    - 不透光，边界判断
                      - 相交，扩充已有扇区
                      - 不相交，添加新扇区
                    - 透光，中心判断
                      - 在扇区内，被遮挡
                      - 在扇区外，有光
                     */
                    var deltaPoint = point - xy0;
                    if (point == xy0 || deltaPoint.magnitudeSqr > sqrRange)
                        // origin or exceed
                        continue;

                    var pointCell = Grid.PosToCell(point);
                    if (DoesOcclude(pointCell))
                    {
                        // 不透光，更新扇区
                        var other = RectToSector(deltaPoint);

                        for (var i = 0; !other.IsClosure && i < blockedSectors.Count; i++)
                        {
                            if (!blockedSectors[i].UnionIfIntersecting(other, out var joined)) continue;
                            // 相交
                            blockedSectors.RemoveAt(i);
                            i--;
                            other = joined;
                        }

                        if (other.IsClosure)
                            return;

                        blockedSectors.Add(other);
                    }
                    else if (blockedSectors.All(blockedSector => !blockedSector.ContainsApproximately(deltaPoint, tolerance)))
                        // 透光，不在扇区
                        visiblePoints.Add(pointCell);
                }

                blockedSectors.ForEach(sector => Debug.Log(sector.ToString()));
            }
        }
    }

    public class RectBorder : IEnumerable<Vector2I>
    {
        private readonly int _minX;
        private readonly int _minY;
        private readonly int _maxX;
        private readonly int _maxY;

        public RectBorder(int minX, int minY, int width, int height)
        {
            _minX = minX;
            _minY = minY;
            _maxX = _minX - 1 + width;
            _maxY = _minY - 1 + height;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<Vector2I> GetEnumerator()
        {
            int x = _minX, y = _minY;
            do
            {
                yield return new Vector2I(x, y);

                // move
                if (y == _minY && x >= _minX && x < _maxX)
                {
                    // bottom
                    x += 1;
                }
                else if (x == _maxX && y >= _minY && y < _maxY)
                {
                    // right
                    y += 1;
                }
                else if (y == _maxY && x <= _maxX && x > _minX)
                {
                    // top
                    x -= 1;
                }
                else
                {
                    // left
                    y -= 1;
                }
            } while (!(x == _minX && y == _minY));
        }
    }

    public struct Sector2
    {
        private static readonly Sector2 _ZERO = new Sector2(Vector2.zero, Vector2.zero);
        private readonly Vector2 _start;
        private readonly Vector2 _end;
        private readonly float _angle;

        public bool IsClosure => this == _ZERO;

        public Sector2(Vector2 start, Vector2 end)
        {
            _start = start;
            _end = end;
            _angle = LightGridUtil.DeltaAngle(start, end);
        }

        public bool UnionIfIntersecting(Sector2 other, out Sector2 joined)
        {
            var isThisSmaller = _angle < other._angle;
            var big = isThisSmaller ? other : this;
            var small = isThisSmaller ? this : other;

            var containsStart = big.Contains(small._start);
            var containsEnd = big.Contains(small._end);
            if (containsStart)
            {
                if (containsEnd)
                {
                    joined = Mathf.Approximately(big._angle + small._angle - LightGridUtil.DeltaAngle(big._start, small._end) - LightGridUtil.DeltaAngle(small._start, big._end), 360f) ? _ZERO : big;
                }
                else
                {
                    joined = new Sector2(big._start, small._end);
                }

                return true;
            }

            if (containsEnd)
            {
                joined = new Sector2(small._start, big._end);
                return true;
            }

            joined = other;
            return false;
        }

        public bool Contains(Vector2 direction)
        {
            var deltaAngle = LightGridUtil.DeltaAngle(_start, direction);
            return deltaAngle >= 0 && deltaAngle <= _angle;
        }

        public bool ContainsApproximately(Vector2 direction, float tolerance = 0f)
        {
            var deltaAngle = LightGridUtil.DeltaAngle(_start, direction);
            return deltaAngle >= tolerance && deltaAngle <= _angle - tolerance;
        }

        // --------------------- generated ------------------------

        public override string ToString()
        {
            return $"start: {_start.ToString()}, end: {_end.ToString()}, angle: {_angle.ToString(CultureInfo.InvariantCulture)}";
        }

        public static bool operator ==(Sector2 o1, Sector2 o2)
        {
            return o1._start == o2._start && o1._end == o2._end;
        }

        public static bool operator !=(Sector2 o1, Sector2 o2)
        {
            return !(o1 == o2);
        }

        public bool Equals(Sector2 other)
        {
            return _start.Equals(other._start) && _end.Equals(other._end) /* && _angle.Equals(other._angle)*/;
        }

        public override bool Equals(object obj)
        {
            return obj is Sector2 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _start.GetHashCode();
                hashCode = (hashCode * 397) ^ _end.GetHashCode();
                /*hashCode = (hashCode * 397) ^ _angle.GetHashCode();*/
                return hashCode;
            }
        }
    }
}