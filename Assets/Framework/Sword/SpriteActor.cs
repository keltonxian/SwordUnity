using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sword
{
    public class SpriteActor : EffectActor
    {
        private Animation _ani;
        public Sprite _sprite;

        public string _spriteName;

        private float _animSpeed = 20f;
        private float _animInternal = 0f;

        public byte _anim1;
        public byte _anim2;

        public short _bussIcon;

        public int _ani_id;
        public int _lastAniId;
        public int _lastFrameIndex;
        public int _lastLoopNum;

        public bool _isSingleAnimation;
        public bool _isLastAniFlipX;
        public bool _isAllFlipX;

        public bool IsFlipX
        {
            get
            {
                return null == _ani ? false : _ani.IsFlipX;
            }
        }

        public override void InitWithType(ActorType type)
        {
            base.InitWithType(type);
        }

        public void InitWithSprite(Sprite sprite)
        {
            InitWithType(ActorType.SPRITE);
            // TODO clone component
            //_sprite = Instantiate(sprite, transform);

            //InitSpriteSetting();
        }

        public void InitWithSpriteName(string spriteName)
        {
            InitWithType(ActorType.SPRITE);
            UpdateSprite(spriteName);
        }

        public void UpdateSprite(string spriteName)
        {
            _spriteName = spriteName;

            if (null != _sprite)
            {
                Destroy(_ani.gameObject);
                _ani = null;
                Destroy(_sprite.gameObject);
                _sprite = null;
            }

            Transform tf = new GameObject(spriteName).transform;
            tf.SetParent(transform);
            tf.localPosition = Vector3.zero;
            tf.localScale = Vector3.one;
            Sprite sprite = tf.gameObject.AddComponent<Sprite>();
            sprite.InitWithName(spriteName);
            _sprite = sprite;

            InitSpriteSetting();
        }

        private void InitSpriteSetting()
        {
            SetAnimation(0, true);
        }

        public void SetAnimation(int newAniId)
        {
            SetAnimation(newAniId, 0, -1, _isAllFlipX, false);
        }

        public void SetAnimation(int newAniId, bool isReset)
        {
            SetAnimation(newAniId, 0, -1, _isAllFlipX, isReset);
        }

        public void SetAnimation(int newAniId, int loopNum)
        {
            SetAnimation(newAniId, 0, loopNum, _isAllFlipX, false);
        }

        public void SetAnimation(int newAniId, int loopNum, bool isReset)
        {
            SetAnimation(newAniId, 0, loopNum, _isAllFlipX, isReset);
        }

        public void SetAnimation(int newAniId, int frameIndex, int loopNum, bool isFlipX)
        {
            SetAnimation(newAniId, frameIndex, loopNum, isFlipX, false);
        }

        public void SetAnimation(int newAniId, int frameIndex, int loopNum, bool isFlipX, bool isReset)
        {
            bool isChange = (null == _ani || _ani_id != newAniId || _ani.IsFlipX != isFlipX || isReset);
            if (null != _sprite && isChange)
            {
                if (null != _ani)
                {
                    Destroy(_ani.gameObject);
                    _ani = null;
                }
                _ani_id = newAniId;
                _ani = _sprite.GetAnimationById(newAniId, isFlipX, transform);

                if (null != _ani)
                {
                    _ani.IsFlipX = isFlipX;
                    _ani._frameIndex = frameIndex;
                    _ani._loopNum = loopNum;

                    _width = _ani.GetWidth();
                    _height = _ani.GetHeight();
                }
            }
        }

        public void SetAnimationByDir(byte dir, bool isMove)
        {
            if (null == _sprite)
            {
                return;
            }
            if (null == _ani)
            {
                bool defaultFlipX = false;
                switch (dir)
                {
                    case Const.DIR_DOWN:
                    case Const.DIR_UP:
                        break;
                    case Const.DIR_LEFT:
                    case Const.DIR_LEFT_DOWN:
                    case Const.DIR_LEFT_UP:
                        defaultFlipX = false;
                        break;
                    case Const.DIR_RIGHT:
                    case Const.DIR_RIGHT_UP:
                    case Const.DIR_RIGHT_DOWN:
                        defaultFlipX = true;
                        break;
                }
                if (true == isMove && true == _sprite.IsAnimationExistById(1))
                {
                    SetAnimation(1, 0, -1, defaultFlipX, false);
                }
                else
                {
                    SetAnimation(0, 0, -1, defaultFlipX, false);
                }
                return;
            }

            int index = 0;
            bool isFlipX = IsFlipX;
            switch (_dir)
            {
                case Const.DIR_DOWN:
                case Const.DIR_UP:
                    break;
                case Const.DIR_LEFT:
                case Const.DIR_LEFT_DOWN:
                case Const.DIR_LEFT_UP:
                    isFlipX = false;
                    break;
                case Const.DIR_RIGHT:
                case Const.DIR_RIGHT_UP:
                case Const.DIR_RIGHT_DOWN:
                    isFlipX = true;
                    break;
            }

            if (true == isMove && true == _sprite.IsAnimationExistById(1) && _sprite._animationsArray.Count > 1)
            {
                index = 1;
            }
            else
            {
                index = 0;
            }

            if (_ani_id != index || isFlipX != _ani.IsFlipX)
            {
                SetAnimation(index, 0, -1, isFlipX, false);
            }
        }

        public void SetAnimationByDir(bool isMove)
        {
            SetAnimationByDir(Const.DIR_LEFT, isMove);
        }

        public void UpdateFaceAndHair(int hair, int face, int sex, int colorIndex)
        {
            if (null == _sprite)
            {
                return;
            }
            _sprite.ReplaceFragment(hair, colorIndex, Const.PART_TYPE_HAIR);
            _sprite.ReplaceFragment(face, Const.PART_TYPE_FACE);
            _sprite.InitFragmentAll();
        }

        public void Draw(int xPos, int yPos)
        {
            if (null != _ani)
            {
                _ani.Draw(xPos, yPos);
            }
        }

        public void Draw()
        {
            Draw(_x, _y);
        }

        void Update()
        {
            _animInternal += _animSpeed * Time.deltaTime;
            if (_animInternal < 1f)
            {
                return;
            }
            _animInternal = 0;
            if (null != _ani)
            {
                _ani.UpdateAni();
            }
            Draw();
        }
    }
}

