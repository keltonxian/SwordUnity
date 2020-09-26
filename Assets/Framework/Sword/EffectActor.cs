using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sword
{
    public class EffectActor : Actor
    {
        private Texture2D _messages;
        private Texture2D _texture2d;
        private Texture2D _texture2dName;

        public int _anchor;
        public int _speedX;
        public int _speedY;
        public int _ax;
        public int _ay;
        public int _pointerX;
        public int _pointerY;
        public int _lastFrameTime;
        public int _currentFrameTime;
        public int _currentLoop;

        public int _color;
        public int _num;

        public float _parabola_a;
        public float _parabola_b;

        public bool _reverse;

        public int _tempYforDrawNum;

        public override void InitWithType(ActorType type)
        {
            base.InitWithType(type);
        }
    }
}

