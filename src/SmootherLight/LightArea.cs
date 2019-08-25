using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MightyVincent
{
    public abstract class LightArea
    {
        private readonly List<LineBlock> _lineBlocks;
        protected readonly float RangeSqr;

        protected LightArea(int range)
        {
            var r = range + 0.5f;
            RangeSqr = r * r;
            _lineBlocks = new List<LineBlock>();
        }

        public static LightArea Create(int range, Vector2 direction, float angle = 360f)
        {
            if (direction == Vector2.zero) return new CircleLightArea(range);

            return new SectorLightArea(range, direction, angle);
        }

        public void AddBlocks(IEnumerable<LineBlock> lineBlocks)
        {
            foreach (var lineBlock in lineBlocks) AddBlock(lineBlock);
        }

        public void AddBlock(LineBlock lineBlock)
        {
            var one = lineBlock;
            for (var i = 0; i < _lineBlocks.Count; i++)
            {
                var block = _lineBlocks[i];
                if (!block.JoinIfAble(one, out var joined)) continue;
                _lineBlocks.RemoveAt(i);
                i--;
                one = joined;
            }

            _lineBlocks.Add(one);
        }

        public abstract bool InRange(Vector2I deltaPoint);

        public bool IsBlocking(Vector2I deltaPoint)
        {
            return _lineBlocks.Any(block => block.IsBlocking(deltaPoint));
        }
    }

    public class CircleLightArea : LightArea
    {
        public CircleLightArea(int range) : base(range)
        {
        }

        public override bool InRange(Vector2I deltaPoint)
        {
            return deltaPoint.magnitudeSqr <= RangeSqr;
        }
    }

    internal class SectorLightArea : LightArea
    {
        private readonly float _angle;
        private readonly Vector2 _direction;

        public SectorLightArea(int range, Vector2 direction, float angle) : base(range)
        {
            _direction = direction;
            _angle = angle;
        }

        public override bool InRange(Vector2I deltaPoint)
        {
            return deltaPoint.magnitudeSqr <= RangeSqr
                   && Vector2.Angle(_direction, deltaPoint) <= _angle / 2;
        }
    }
}