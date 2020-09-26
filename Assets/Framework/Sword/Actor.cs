using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sword
{
    public class Actor : MonoBehaviour
    {
        public enum ActorType
        {
            NPC, PLAYER, MONSTER, PET, IMAGE, NUMBER, STRING, OTHER_PLAYER, EFFECT,
            SPRITE, NUMBER_2, BATTLE_DIALOG, BATTLE_RESULT, PLAYER_STATUS,
            PET_EFFECT_STATUS, BATTLE_MSG_STATUS,
        }

        private string _name;
        private Actor _prevActor;
        private Actor _nextActor;
        private Texture2D _nameTex;
        private Texture2D _guildNameTex;
        private Texture2D _boothNameTex;

        private int _id;
        private ActorType _type;
        protected int _x;
        protected int _y;
        public int _width;
        public int _height;
        protected byte _dir;

        public bool _isStoryActor;
        public float _depth;
        public bool _isVisible;
        public byte _lastMoveDir;
        public int _lastMoveTOGx;
        public int _lastMoveTOGy;
        public int _lastMoveBeginIndex;
        public int _lastMoveSize;

        public Vector2[] _lastMoves;
        public byte _lastMovesLenght;

        public virtual void InitWithType(ActorType type)
        {
            _type = type;
            _isVisible = true;
            _isStoryActor = false;
            _dir = Const.DIR_NONE;
            _lastMoveDir = Const.DIR_NONE;
        }
    }
}

