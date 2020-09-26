using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Manager;

namespace Sword
{
    public class Frame : MonoBehaviour
    {
        public int _frameID;
        public byte[] _fragmentFileIndexs;
        public byte[] _fragmentBlockIndexs;
        public byte[] _transforms;
        public sbyte[] _centerXs;
        public sbyte[] _centerYs;

        public short _frameSize;

        public void InitWithID(int frameId)
        {
            _frameID = frameId;
        }
        
        public int GetFrameWidth(Sprite sprite)
        {
            if (null == _fragmentFileIndexs || null == sprite || 0 == sprite._fragmentsArray.Count)
            {
                return 0;
            }

            int leftX = 0;
            int rightX = 0;
            int temp = 0;
            Fragment fragment;
            for (int i = 0; i < _frameSize; i++)
            {
                fragment = sprite._fragmentsArray[_fragmentFileIndexs[i]];
                if (null == fragment)
                {
                    continue;
                }

                int halfBlockWidth = fragment.GetWidthByIndex(_fragmentBlockIndexs[i], _transforms[i]) / 2;
                temp = _centerXs[i] - halfBlockWidth;
                leftX = temp < leftX ? temp : leftX;

                temp = _centerXs[i] + halfBlockWidth;
                rightX = temp > rightX ? temp : rightX;
            }
            return Mathf.Abs(leftX - rightX);
        }

        public int GetFrameHeight(Sprite sprite)
        {
            if (null == _fragmentFileIndexs || null == sprite || 0 == sprite._fragmentsArray.Count)
            {
                return 0;
            }

            int bottomY = 0;
            int topY = 0;
            int temp = 0;
            Fragment fragment;
            for (int i = 0; i < _frameSize; i++)
            {
                fragment = sprite._fragmentsArray[_fragmentFileIndexs[i]];
                if (null == fragment)
                {
                    continue;
                }

                int halfBlockHeight = fragment.GetHeightByIndex(_fragmentBlockIndexs[i], _transforms[i]) / 2;
                temp = _centerYs[i] - halfBlockHeight;
                bottomY = bottomY < temp ? bottomY : temp;

                temp = _centerYs[i] + halfBlockHeight;
                topY = topY > temp ? topY : temp;
            }
            return Mathf.Abs(bottomY - topY);
        }

        public void FromBytes(byte[] bytes, ref int bytesIndex, Data indexIdData)
        {
            _frameSize = DataManager.Byte2Short(DataManager.ReadByte(bytes, ref bytesIndex));
            if (_frameSize <= 0)
            {
                return;
            }

            _fragmentFileIndexs = new byte[_frameSize];
            _fragmentBlockIndexs = new byte[_frameSize];
            _transforms = new byte[_frameSize];
            _centerXs = new sbyte[_frameSize << 1];
            _centerYs = new sbyte[_frameSize << 1];

            byte[] fragmentIndexIdMapping = indexIdData.Bytes;
            int mappingSize = indexIdData.Length;

            for (int i = 0; i < _frameSize; i++)
            {
                byte blockId = DataManager.ReadByte(bytes, ref bytesIndex);
                byte fragmentFileId = DataManager.ReadByte(bytes, ref bytesIndex);

                for (int j = 0; j < mappingSize; j += 2)
                {
                    if (fragmentIndexIdMapping[j + 1] == fragmentFileId)
                    {
                        _fragmentFileIndexs[i] = fragmentIndexIdMapping[j];
                        break;
                    }
                }

                _fragmentBlockIndexs[i] = blockId;
                _centerXs[i] = DataManager.Byte2SByte(DataManager.ReadByte(bytes, ref bytesIndex));
                _centerYs[i] = DataManager.Byte2SByte(DataManager.ReadByte(bytes, ref bytesIndex));
                _transforms[i] = TransferTransform (DataManager.ReadByte(bytes, ref bytesIndex));
            }
        }

        public static byte TransferTransform(byte transform)
        {
            switch (transform)
            {
                case Const.TRANSFORM_MIRROR:
                    return Const.TRANS_MIRROR;
                case Const.TRANSFORM_MIRROR_ROT180:
                    return Const.TRANS_MIRROR_ROT180;
                case Const.TRANSFORM_ROT90:
                    return Const.TRANS_ROT90;
                case Const.TRANSFORM_ROT180:
                    return Const.TRANS_ROT180;
                case Const.TRANSFORM_ROT270:
                    return Const.TRANS_ROT270;
                case Const.TRANSFORM_NONE:
                    return 0;
                default:
                    return 0;
            }
        }

        public void Draw(int screenX, int screenY, bool isFlip, Sprite sprite)
        {
            if (null == _fragmentBlockIndexs || null == _fragmentFileIndexs || sprite._fragmentsArray.Count == 0)
            {
                return;
            }

            for (int i = 0; i < sprite._fragmentsArray.Count; i++)
            {
                Fragment fragment = sprite._fragmentsArray[i];
                fragment.SetAllImageShow(false);
            }
            //Debug.Log("====1===");
            //isFlip = true;
            for (int i = 0; i < _frameSize; i++)
            {
                int index = _fragmentFileIndexs[i];
                Fragment fragment = sprite._fragmentsArray[index];
                if (null == fragment || (!fragment.IsInit))
                {
                    continue;
                }

                Vector2 offset = fragment.GetAvatarPointAfterTransform(_fragmentBlockIndexs[i], _transforms[i]);
                if (3 == _transforms[i])
                {
                    Debug.Log("offset x:" + offset.x);
                    Debug.Log("offset y:" + offset.y);
                }
                int sx = (int)(_centerXs[i] + offset.x);
                sx = screenX + (isFlip ? -sx : sx);
                int sy = (int)(screenY + _centerYs[i] + offset.y);
                int sz = i + 1;
                //Debug.Log("name:" + fragment.name + ", index:" + index + ", isFlip:" + (isFlip ? "Yes" : "No") + ", trans:" + _transforms[i]);

                fragment.Draw(_fragmentBlockIndexs[i], sx, sy, sz, isFlip, _transforms[i]);
            }
            //Debug.Log("====2===");
        }

    }
}

