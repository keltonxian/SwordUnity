using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Manager;

namespace Sword
{
    public class Animation : MonoBehaviour
    {
        public int _animationID;

        private sbyte[] _xSpeed;
        private sbyte[] _ySpeed;
        private byte[] _lastTime;
        private byte[] _frameIds;

        private byte[] _isChecks;

        private short _xSpeedSize;
        private short _ySpeedSize;
        private short _lastTimeSize;
        private short _frameIdsSize;
        private short _isChecksSize;

        public int _loopNum;
        private int _x;
        private int _y;
        public int _width;
        public int _height;
        public int _frameIndex;
        private int _currentFrameLastTime;
        private int _loopIndex;

        public Sprite _sprite;

        private bool _isFlipX;
        public bool IsFlipX
        {
            get
            {
                return _isFlipX;
            }
            set
            {
                _isFlipX = value;
            }
        }

        public void Init()
        {
            _animationID = -1;
            _frameIndex = 0;
            _loopIndex = 0;
        }

        public static Animation Clone(Animation cloneTarget, Transform parent)
        {
            Transform tf = Instantiate(cloneTarget.gameObject).transform;
            tf.gameObject.name = cloneTarget.gameObject.name;
            tf.SetParent(parent);
            tf.localPosition = cloneTarget.transform.localPosition;
            tf.localScale = cloneTarget.transform.localScale;
            tf.localEulerAngles = cloneTarget.transform.localEulerAngles;
            Animation animation = tf.GetComponent<Animation>();
            animation._animationID = cloneTarget._animationID;
            animation._loopNum = cloneTarget._loopNum;

            animation._lastTimeSize = cloneTarget._lastTimeSize;
            if (cloneTarget._lastTimeSize > 0)
            {
                animation._lastTime = (byte[])cloneTarget._lastTime.Clone();
            }

            animation._x = cloneTarget._x;
            animation._y = cloneTarget._y;
            animation._width = cloneTarget._width;
            animation._height = cloneTarget._height;
            animation._frameIndex = cloneTarget._frameIndex;

            animation._xSpeedSize = cloneTarget._xSpeedSize;
            if (cloneTarget._xSpeedSize > 0)
            {
                animation._xSpeed = (sbyte[])cloneTarget._xSpeed.Clone();
            }

            animation._ySpeedSize = cloneTarget._ySpeedSize;
            if (cloneTarget._ySpeedSize > 0)
            {
                animation._ySpeed = (sbyte[])cloneTarget._ySpeed.Clone();
            }

            animation._loopIndex = cloneTarget._loopIndex;
            animation._currentFrameLastTime = cloneTarget._currentFrameLastTime;
            animation.IsFlipX = cloneTarget.IsFlipX;

            animation._frameIdsSize = cloneTarget._frameIdsSize;
            if (cloneTarget._frameIdsSize > 0)
            {
                animation._frameIds = (byte[])cloneTarget._frameIds.Clone();
            }

            animation._isChecksSize = cloneTarget._isChecksSize;
            if (cloneTarget._isChecksSize > 0)
            {
                animation._isChecks = (byte[])cloneTarget._isChecks.Clone();
            }

            animation._sprite = cloneTarget._sprite;

            return animation;
        }

        public int GetWidth()
        {
            if (null == _sprite || null == _frameIds)
            {
                return 0;
            }
            int maxWidth = 0;
            int temp = 0;
            for (int i = 0; i < _frameIdsSize; i++)
            {
                temp = _sprite._framesArray[_frameIds[i]].GetFrameWidth(_sprite);
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            return maxWidth;
        }

        public int GetHeight()
        {
            if (null == _sprite || null == _frameIds)
            {
                return 0;
            }
            int maxHeight = 0;
            int temp = 0;
            for (int i = 0; i < _frameIdsSize; i++)
            {
                temp = _sprite._framesArray[_frameIds[i]].GetFrameHeight(_sprite);
                if (temp > maxHeight)
                {
                    maxHeight = temp;
                }
            }
            return maxHeight;
        }

        public void FromBytes(byte[] bytes, ref int bytesIndex)
        {
            _animationID = DataManager.ReadByte(bytes, ref bytesIndex);
            short frameNum = DataManager.ReadShort(bytes, ref bytesIndex);
            _loopNum = -1;

            _lastTime = new byte[frameNum];
            _frameIds = new byte[frameNum];
            _xSpeed = new sbyte[frameNum];
            _ySpeed = new sbyte[frameNum];

            _lastTimeSize = frameNum;
            _frameIdsSize = frameNum;
            _xSpeedSize = frameNum;
            _ySpeedSize = frameNum;

            byte checkNum = DataManager.ReadByte(bytes, ref bytesIndex);
            if (checkNum > 0)
            {
                _isChecksSize = checkNum;
                _isChecks = new byte[checkNum];
                for (int i = 0; i < checkNum; i++)
                {
                    byte index = DataManager.ReadByte(bytes, ref bytesIndex);
                    _isChecks[i] = index;
                }
            }

            for (int i = 0; i < frameNum; i++)
            {
                _frameIds[i] = DataManager.ReadByte(bytes, ref bytesIndex);
                _lastTime[i] = DataManager.ReadByte(bytes, ref bytesIndex);
                _xSpeed[i] = DataManager.Byte2SByte(DataManager.ReadByte(bytes, ref bytesIndex));
                _ySpeed[i] = DataManager.Byte2SByte(DataManager.ReadByte(bytes, ref bytesIndex));
            }
        }

        public void UpdateAni()
        {
            if (null == _frameIds || _frameIndex < 0 || _frameIndex >= _frameIdsSize)
            {
                return;
            }

            _x += _xSpeed[_frameIndex];
            _y += _ySpeed[_frameIndex];
            _currentFrameLastTime++;

            int last = _lastTime[_frameIndex];
            if (_currentFrameLastTime >= last)
            {
                _currentFrameLastTime = 0;
                _frameIndex++;
            }
            if (_frameIndex >= _frameIdsSize)
            {
                if (_loopNum != -1 && _loopIndex >= _loopNum)
                {
                    _frameIndex = -1;
                }
                else
                {
                    _frameIndex = 0;
                    _loopIndex++;
                }
            }
        }

        public void Draw(int xPos, int yPos)
        {
            Draw(xPos, yPos, IsFlipX);
        }

        public void Draw(int xPos, int yPos, bool isFlip)
        {
            Draw(xPos, yPos, isFlip, Const.BOTTOM_HCENTER);
        }

        public void Draw(int xPos, int yPos, bool isFlip, byte anchor)
        {
            if (null == _sprite || null == _frameIds)
            {
                return;
            }
            int offsetX = IsFlipX ? -_x : _x;
            int offsetY = _y;

            Frame frame;
            if (_frameIndex < 0 || _frameIndex >= _frameIdsSize)
            {
                frame = _sprite._framesArray[_frameIds[_frameIdsSize - 1]];
            }
            else
            {
                frame = _sprite._framesArray[_frameIds[_frameIndex]];
            }

            xPos += offsetX;
            yPos += offsetY;
            if ((anchor & Const.ANCHOR_LEFT) != 0)
            {
                xPos += _width / 2;
            }
            else if ((anchor & Const.ANCHOR_RIGHT) != 0)
            {
                xPos -= _width / 2;
            }

            if ((anchor & Const.ANCHOR_TOP) != 0)
            {
                yPos += _height;
            }
            else if ((anchor & Const.ANCHOR_RIGHT) != 0)
            {
                yPos -= _height / 2;
            }

            frame.Draw(xPos, yPos, isFlip, _sprite);
        }
    }
}

