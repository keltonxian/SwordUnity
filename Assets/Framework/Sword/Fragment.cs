using System;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Manager;
using UnityEngine;

namespace Sword
{
    public class Fragment : MonoBehaviour
    {
        public GameImage _gameImage;
        private Transform _nodeImage;

        public byte[] _ids;
        public byte[] _xs;
        public byte[] _ys;
        public byte[] _widths;
        public byte[] _heights;
        public sbyte[] _avatarX;
        public sbyte[] _avatarY;
        private SpriteRenderer[] _images;

        public int _fragmentSize;

        public bool IsInit
        {
            get
            {
                return (null != _gameImage && null != _gameImage.Image);
            }
        }

        public void InitWithName(string name, int plIndex)
        {
            Transform tf = new GameObject("GameImage").transform;
            tf.SetParent(transform);
            tf.localPosition = Vector3.zero;
            tf.localScale = Vector3.one;
            _gameImage = tf.gameObject.AddComponent<GameImage>();
            _gameImage.InitWithImageName(string.Format("{0}.png", name), string.Format("{0}_{1}.pl", name, plIndex));
            Transform tf2 = new GameObject("Image").transform;
            tf2.SetParent(transform);
            tf2.localPosition = Vector3.zero;
            tf2.localScale = Vector3.one;
            _nodeImage = tf2;
        }

        public void InitDatasAndImage()
        {
            string key = string.Format("{0}{1}", GetName(), GetPlIndex());
            //Debug.Log(string.Format("-- initDatasAndImage : {0}----{1}", GetName(), GetPlIndex()));
            Fragment temp = Model.GetCacheFragment(key);
            if (null != temp)
            {
                _fragmentSize = temp._fragmentSize;
                _ids = new byte[_fragmentSize];
                _xs = new byte[_fragmentSize];
                _ys = new byte[_fragmentSize];
                _widths = new byte[_fragmentSize];
                _heights = new byte[_fragmentSize];
                _avatarX = new sbyte[_fragmentSize];
                _avatarY = new sbyte[_fragmentSize];
                _images = new SpriteRenderer[_fragmentSize];

                for (int i = 0; i < _fragmentSize; i++)
                {
                    _ids[i] = temp._ids[i];
                    _xs[i] = temp._xs[i];
                    _ys[i] = temp._ys[i];
                    _widths[i] = temp._widths[i];
                    _heights[i] = temp._heights[i];
                    _avatarX[i] = temp._avatarX[i];
                    _avatarY[i] = temp._avatarY[i];
                    //Debug.Log(string.Format("=======t==1=[{0}]=========", i));
                    //Debug.Log(string.Format("id[{0}] x[{1}] y[{2}] width[{3}] height[{4}] avatarX[{5}] avatarY[{6}]", _ids[i], _xs[i], _ys[i], _widths[i], _heights[i], _avatarX[i], _avatarY[i]));
                    //Debug.Log(string.Format("=======t==2=[{0}]=========", i));
                }

                _gameImage.Image = temp._gameImage.Image;
                InitImage();
                SetupImage();
            }
            else
            {
                Data data = GetDataInputStream(DataManager.GetFileName(_gameImage.ImageName));
                if (null == data)
                {
                    return;
                }
                int bytesIndex = 0;
                byte[] bytes = data.Bytes;
                FromBytes(bytes, ref bytesIndex);
                if (null != _gameImage.Image)
                {
                    return;
                }
                GameImage tempGameImage = Model.FindGameImageInGame(_gameImage.ImageName, GetPlIndex());
                if (null != tempGameImage)
                {
                    _gameImage = tempGameImage;
                    return;
                }
                LoadImage(bytes, ref bytesIndex);
                SetupImage();

                Model.AddCacheFragment(key, this);
            }
        }

        private void InitImage()
        {
            if (null != _gameImage.Image)
            {
                return;
            }
            GameImage temp = Model.FindGameImageInGame(_gameImage.ImageName, GetPlIndex());
            if (null != temp)
            {
                _gameImage = temp;
                return;
            }
            InitImageFromfile();
        }

        private void InitImageFromfile()
        {
            Data data = GetDataInputStream(GetName());
            if (null == data)
            {
                return;
            }

            byte[] bytes = data.Bytes;
            int bytesIndex = 0;
            int len = DataManager.ReadInt(bytes, ref bytesIndex);
            DataManager.ReadSkip(bytes, len, ref bytesIndex);
            LoadImage(bytes, ref bytesIndex);
        }

        private void LoadImage(byte[] bytes, ref int bytesIndex)
        {
            int imageLen = DataManager.ReadInt(bytes, ref bytesIndex);
            int plIndex = GetPlIndex();
            //Debug.Log("imageLen:" + imageLen + ", plIndex:" + plIndex);
            if (0 == plIndex)
            {
                byte[] temp = new byte[imageLen];
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = bytes[bytesIndex + i];
                }
                bytesIndex += temp.Length;
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(temp);
                _gameImage.Image = tex;
            }
            else
            {
                _gameImage.ImageDatas = new Data(bytes, ref bytesIndex, imageLen);
                //Debug.Log("imageLen:" + imageLen);

                int totalPlDataSize = DataManager.ReadInt(bytes, ref bytesIndex);
                if (totalPlDataSize > 0 && plIndex > 0)
                {
                    //Debug.Log("totalPlDataSize:" + totalPlDataSize);
                    byte plFileNum = DataManager.ReadByte(bytes, ref bytesIndex);
                    for (int i = 0; i < plFileNum; i++)
                    {
                        byte filePlIndex = DataManager.ReadByte(bytes, ref bytesIndex);
                        short plDataSize = DataManager.ReadShort(bytes, ref bytesIndex);
                        if (plIndex == filePlIndex)
                        {
                            _gameImage.PlDatas = new Data(bytes, ref bytesIndex, plDataSize);
                            break;
                        }
                        bytesIndex += plDataSize;
                    }
                }
                _gameImage.InitImage();
            }
        }

        private void SetupImage()
        {
            if (null == _gameImage)
            {
                return;
            }
            Texture2D tex = _gameImage.Image;
            int texHeight = tex.height;
            //Debug.Log("=======t==1==========");
            //Debug.Log(string.Format("Tex width[{0}] height[{1}]", tex.width, tex.height));
            for (int i = 0; i < _fragmentSize; i++)
            {
                byte id = _ids[i];
                byte x = _xs[i];
                byte y = _ys[i];
                byte width = _widths[i];
                byte height = _heights[i];
                sbyte avatarX = _avatarX[i];
                sbyte avatarY = _avatarY[i];

                _gameImage.AddRenderer(id, x, y, width, height, avatarX, avatarY, Const.TRANS_NONE);

                //Transform tf = new GameObject(string.Format("Image[{0}]", id)).transform;
                //tf.SetParent(_nodeImage);
                //tf.localPosition = Vector3.zero;
                //tf.localScale = Vector3.one;
                //SpriteRenderer sr = tf.gameObject.AddComponent<SpriteRenderer>();
                //sr.sprite = UnityEngine.Sprite.Create(tex, new Rect(x, texHeight - y - height, width, height), new Vector2(0, 1), Const.SPRITE_PIXEL_PER_UNIT, 0, SpriteMeshType.FullRect);
                //sr.sprite = UnityEngine.Sprite.Create(tex, new Rect(x, texHeight - y - height, width, height), new Vector2(0.5f, 0.5f), Const.SPRITE_PIXEL_PER_UNIT, 0, SpriteMeshType.FullRect);

                //_images[i] = sr;

                //Debug.Log(string.Format("id[{0}] x[{1}] y[{2}] width[{3}] height[{4}] avatarX[{5}] avatarY[{6}]", _ids[i], _xs[i], _ys[i], _widths[i], _heights[i], _avatarX[i], _avatarY[i]));
            }
            //Debug.Log("=======t==2==========");
        }

        public void SetAllImageShow(bool isShow)
        {
            _gameImage.SetAllRendererShow(isShow);
            //for (int i = 0; i < _images.Length; i++)
            //{
            //    _images[i].enabled = isShow;
            //}
        }

        public static Data GetDataInputStream(string fileName)
        {
            Data cache = Model.FindCacheSpriteData(string.Format("{0}.f", fileName));
            if (null != cache)
            {
                return cache;
            }
            return DataManager.GetFileData(Const.FOLDER_RES_SPRITE, fileName, ".f");
        }

        public int GetWidthByIndex(int index, int trans)
        {
            if (null == _widths || index < 0 || index >= _fragmentSize)
            {
                return 0;
            }
            if (trans == Const.TRANS_ROT90 || trans == Const.TRANS_ROT270)
            {
                return DataManager.Byte2Int(_heights[index]);
            }
            else
            {
                return DataManager.Byte2Int(_widths[index]);
            }
        }

        public int GetHeightByIndex(int index, int trans)
        {
            if (null == _heights || index < 0 || index >= _fragmentSize)
            {
                return 0;
            }
            if (trans == Const.TRANS_ROT90 || trans == Const.TRANS_ROT270)
            {
                return DataManager.Byte2Int(_widths[index]);
            }
            else
            {
                return DataManager.Byte2Int(_heights[index]);
            }
        }

        public static string GetTypeByString(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return null;
            }

            int index = type.IndexOf('_');
            if (index < 0)
            {
                return null;
            }
            return type.Substring(0, index);
        }

        public int GetFragmentId()
        {
            string fileName = DataManager.GetFileName(_gameImage.ImageName);
            if (string.IsNullOrEmpty(fileName))
            {
                return -1;
            }

            int beginIndex = 0;
            int len = 0;
            for (int i = fileName.Length - 2; i >= 0; i--)
            {
                if (fileName.Substring(i, 1).Equals('_'))
                {
                    beginIndex = i + 1;
                    len = fileName.Length - 1 - i;
                    break;
                }
            }

            if (len == 0)
            {
                return -1;
            }

            return int.Parse(fileName.Substring(beginIndex, len));
        }

        public int GetPlIndex()
        {
            if (string.IsNullOrEmpty(_gameImage.PlName))
            {
                return 0;
            }
            string temp = DataManager.GetFileName(_gameImage.PlName);
            int beginIndex = temp.IndexOf('_');
            if (beginIndex < 0)
            {
                return 0;
            }
            beginIndex = temp.Length - 1;
            return int.Parse(temp.Substring(beginIndex, 1));
        }

        public string GetName()
        {
            return DataManager.GetFileName(_gameImage.ImageName);
        }

        private void FromBytes(byte[] bytes, ref int bytesIndex)
        {
            int length = DataManager.ReadInt(bytes, ref bytesIndex);
            if (null != _ids)
            {
                DataManager.ReadSkip(bytes, length, ref bytesIndex);
            }
            _fragmentSize = DataManager.ReadByte(bytes, ref bytesIndex);
            if (0 == _fragmentSize)
            {
                return;
            }
            _ids = new byte[_fragmentSize];
            _xs = new byte[_fragmentSize];
            _ys = new byte[_fragmentSize];
            _widths = new byte[_fragmentSize];
            _heights = new byte[_fragmentSize];
            _avatarX = new sbyte[_fragmentSize];
            _avatarY = new sbyte[_fragmentSize];
            _images = new SpriteRenderer[_fragmentSize];

            //Debug.Log("=========1==========");
            for (int i = 0; i < _fragmentSize; i++)
            {
                _ids[i] = DataManager.ReadByte(bytes, ref bytesIndex);
                _xs[i] = DataManager.ReadByte(bytes, ref bytesIndex);
                _ys[i] = DataManager.ReadByte(bytes, ref bytesIndex);
                _widths[i] = DataManager.ReadByte(bytes, ref bytesIndex);
                _heights[i] = DataManager.ReadByte(bytes, ref bytesIndex);
                _avatarX[i] = DataManager.Byte2SByte(DataManager.ReadByte(bytes, ref bytesIndex));
                _avatarY[i] = DataManager.Byte2SByte(DataManager.ReadByte(bytes, ref bytesIndex));
                //Debug.Log(string.Format("id[{0}] x[{1}] y[{2}] width[{3}] height[{4}] avatarX[{5}] avatarY[{6}]", _ids[i], _xs[i], _ys[i], _widths[i], _heights[i], _avatarX[i], _avatarY[i]));
            }
            //Debug.Log("=========2==========");
        }

        public UnityEngine.Sprite LoadUIImage()
        {
            Data data = GetDataInputStream(GetName());
            if (null == data)
            {
                return null;
            }
            byte[] bytes = data.Bytes;
            int bytesIndex = 0;
            int len = DataManager.ReadInt(bytes, ref bytesIndex);
            DataManager.ReadSkip(bytes, len, ref bytesIndex);
            int imageLen = DataManager.ReadInt(bytes, ref bytesIndex);
            int plIndex = GetPlIndex();
            Texture2D tex = null;
            if (0 == plIndex)
            {
                byte[] temp = new byte[imageLen];
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = bytes[bytesIndex + i];
                }
                bytesIndex += temp.Length;
                tex = new Texture2D(2, 2);
                tex.LoadImage(temp);
                _gameImage.Image = tex;
            }
            else
            {
                Data imageData = new Data(bytes, ref bytesIndex, imageLen);
                string fileName = DataManager.GetFileName(_gameImage.PlName);
                Data plData = DataManager.GetFileData(Const.FOLDER_RES_SPRITE, fileName, "pl");
                tex = DataManager.LoadImageWithPalette(imageData, plData);
            }
            int x = 0;
            int y = 0;
            int texHeight = tex.height;
            int width = tex.width;
            int height = tex.height;
            UnityEngine.Sprite sprite = UnityEngine.Sprite.Create(tex, new Rect(x, texHeight - y - height, width, height), new Vector2(0, 1), Const.SPRITE_PIXEL_PER_UNIT, 0, SpriteMeshType.FullRect);
            return sprite;
        }

        public Vector2 GetAvatarPointAfterTransform(int index, int trans)
        {
            if (index < 0 || index >= _fragmentSize)
            {
                return Vector2.zero;
            }
            int avatarXPos = _avatarX[index];
            int avatarYPos = _avatarY[index];

            switch (trans)
            {
                case Const.TRANS_MIRROR:
                    return new Vector2(-avatarXPos, avatarYPos);
                case Const.TRANS_MIRROR_ROT180:
                    return new Vector2(avatarXPos, -avatarYPos);
                case Const.TRANS_ROT90:
                    return new Vector2(-avatarYPos, avatarXPos);
                case Const.TRANS_ROT180:
                    return new Vector2(-avatarXPos, -avatarYPos);
                case Const.TRANS_ROT270:
                    return new Vector2(avatarYPos, -avatarXPos);
                default:
                    return new Vector2(avatarXPos, avatarYPos);
            }
        }

        public void Draw (byte fragmentSubIndex, int screenX, int screenY, int screenZ, bool isFlip, byte trans)
        {
            if (null == _xs || fragmentSubIndex >= _fragmentSize)
            {
                return;
            }

            byte id = _ids[fragmentSubIndex];
            byte x = _xs[fragmentSubIndex];
            byte y = _ys[fragmentSubIndex];
            byte width = _widths[fragmentSubIndex];
            byte height = _heights[fragmentSubIndex];
            sbyte avatarX = _avatarX[fragmentSubIndex];
            sbyte avatarY = _avatarY[fragmentSubIndex];

            if (trans == Const.TRANS_ROT90 || trans == Const.TRANS_ROT270)
            {
                //Debug.Log("1 trans:" + trans);
                //Debug.Log("1 screenX:" + screenX);
                //Debug.Log("1 screenY:" + screenY);
                //Debug.Log("1 width:" + width);
                //Debug.Log("1 (width >> 1):" + (width >> 1));
                screenX += (width >> 1) - (height >> 1);
                screenY -= (width >> 1) - (height >> 1);
                //Debug.Log("2 screenX:" + screenX);
                //Debug.Log("2 screenY:" + screenY);
            }

            //int texHeight = _gameImage.Image.height;
            //Rect texRect = new Rect(x, texHeight - y - height, width, height);

            Vector3 drawPos;
            Vector3 drawScale = Vector3.one;
            float pixelPerUnit = Const.SPRITE_PIXEL_PER_UNIT;
            if (isFlip)
            {
                drawPos = new Vector3((screenX + (width >> 1)) / pixelPerUnit, -(screenY - (height >> 1)) / pixelPerUnit, -0.01f * screenZ);
                drawScale.x = -drawScale.x;
            }
            else
            {
                drawPos = new Vector3((screenX - (width >> 1)) / pixelPerUnit, -(screenY - (height >> 1)) / pixelPerUnit, -0.01f * screenZ);
            }
            //_gameImage.UpdateTexProp(texRect);
            //Debug.Log("fragmentSubIndex:" + fragmentSubIndex);
            _gameImage.ShowRenderer(id, x, y, width, height, avatarX, avatarY, trans, drawPos, drawScale);
        }
    }
}

