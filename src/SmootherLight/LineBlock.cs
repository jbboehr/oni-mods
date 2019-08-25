using UnityEngine;

namespace MightyVincent
{
    public struct LineBlock
    {
        private readonly Vector2 _start;
        private readonly Vector2 _end;
        private readonly float _angle; // (0, 180)
        private readonly Vector2 _direction;

        public LineBlock(Vector2 start, Vector2 end)
        {
            _start = start;
            _end = end;
            _angle = Vector2.SignedAngle(_start, _end);
            _direction = _end - _start;
        }

        public bool IsBlocking(Vector2 deltaPoint)
        {
            var deltaPointAngle = Vector2.SignedAngle(_start, deltaPoint);
            return deltaPointAngle >= 0f
                   && deltaPointAngle <= _angle
                   && Vector2.SignedAngle(_direction, deltaPoint - _start) <= 0f;
        }

        public bool JoinIfAble(LineBlock other, out LineBlock joined)
        {
            // if appendable
            var sameDirection = Vector2.Angle(_direction, other._direction) <= 0;
            if (sameDirection)
            {
                if (_start == other._end)
                {
                    joined = new LineBlock(other._start, _end);
                    return true;
                }

                if (_end == other._start)
                {
                    joined = new LineBlock(_start, other._end);
                    return true;
                }
            }

            // if totally blocked
            var isThisShorter = _angle < other._angle;
            var longer = isThisShorter ? other : this;
            var shorter = isThisShorter ? this : other;
            if (longer.IsBlocking(shorter._start) && longer.IsBlocking(shorter._end))
            {
                joined = longer;
                return true;
            }

            // not joined, useless
            joined = this;
            return false;
        }

        public static LineBlock operator +(LineBlock lineBlock, Vector2 offset)
        {
            return new LineBlock(lineBlock._start + offset, lineBlock._end + offset);
        }
    }
}