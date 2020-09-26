using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Manager;
using System;

namespace Sword
{
    public class Sprite : MonoBehaviour
    {
        private string _name;
        private Transform _nodeFragment;
        private Transform _nodeFrame;
        private Transform _nodeAnimation;
        public List<Fragment> _fragmentsArray = new List<Fragment>();
        public List<Frame> _framesArray = new List<Frame>();
        public List<Animation> _animationsArray = new List<Animation>();
        private static List<IntStrPair> PARTS = new List<IntStrPair>();
        private static bool _isPARTInited = false;

        private short _fragmentTypesSize;
        private bool _isSpriteInit;

        public byte[] _fragmentTypes;

        public static void InitIntStrPair()
        {
            if (true == _isPARTInited)
            {
                return;
            }
            _isPARTInited = true;
            string[] partType = {
                Const.PART_TYPE_NAME_TIARA, Const.PART_TYPE_NAME_HAIR, Const.PART_TYPE_NAME_HEAD,
                Const.PART_TYPE_NAME_BODY, Const.PART_TYPE_NAME_HANDS, Const.PART_TYPE_NAME_FEET,
                Const.PART_TYPE_NAME_TAIL, Const.PART_TYPE_NAME_WING, Const.PART_TYPE_NAME_WEAPON,
                Const.PART_TYPE_NAME_SHADOW, Const.PART_TYPE_NAME_SHOULDER, Const.PART_TYPE_NAME_CWE,
                Const.PART_TYPE_NAME_CWNECK, Const.PART_TYPE_NAME_CW, Const.PART_TYPE_NAME_G
            };
            byte[] partTypeID = {
                Const.PART_TYPE_TIARA, Const.PART_TYPE_HAIR, Const.PART_TYPE_FACE,
                Const.PART_TYPE_BODY, Const.PART_TYPE_HAND, Const.PART_TYPE_FEET,
                Const.PART_TYPE_TAIL, Const.PART_TYPE_WING, Const.PART_TYPE_WEAPON,
                Const.PART_TYPE_SHADOW, Const.PART_TYPE_SHOULDER, Const.PART_TYPE_PET_SPEC,
                Const.PART_TYPE_PET_GEN,Const.PET_TYPE_PET_ANI,Const.PART_TYPE_RIDER
            };
            for (int i = 0; i < partType.Length; i++)
            {
                IntStrPair temp = new IntStrPair().InitWithID(partTypeID[i], partType[i]);
                PARTS.Add(temp);
            }
        }

        public static string GetPartNameByPartId(int partID)
        {
            for (int i = 0; i < PARTS.Count; i++)
            {
                IntStrPair pair = PARTS[i];
                //Debug.Log(string.Format("GetPartNameByPartId pair id[{0}] partID[{1}]", pair._id, partID));
                if (pair._id == partID)
                {
                    return pair._str;
                }
            }
            return null;
        }

        public bool IsFragmentInit
        {
            get
            {
                if (_fragmentsArray.Count == 0)
                {
                    return true;
                }
                foreach (Fragment fragment in _fragmentsArray)
                {
                    if (null != fragment && false == fragment.IsInit)
                    {
                        return false;
                    }
                }
                return true;
            } 
        }

        public bool IsDataInit
        {
            get
            {
                return _fragmentsArray.Count > 0;
            }
        }

        public bool IsInit()
        {
            return (IsDataInit & IsFragmentInit);
        }

        private void RefreshSpriteIsInit()
        {
            _isSpriteInit = IsInit();
        }

        public void InitWithName(string name)
        {
            InitIntStrPair();
            _name = name;
            _nodeFragment = AddChildNode("Fragment");
            _nodeFrame = AddChildNode("Frame");
            _nodeAnimation = AddChildNode("Animation");
            
            InitDatas();
        }

        private Transform AddChildNode(string nodeName)
        {
            Transform tf = new GameObject(nodeName).transform;
            tf.SetParent(transform);
            tf.localPosition = Vector3.zero;
            tf.localScale = Vector3.one;
            return tf;
        }

        public Animation GetAnimationById(int animationId, bool isFlipX, Transform parent)
        {
            if (_animationsArray.Count == 0)
            {
                return null;
            }
            foreach (Animation ani in _animationsArray)
            {
                if (ani._animationID == animationId)
                {
                    Animation temp = Animation.Clone(ani, parent);
                    temp.IsFlipX = isFlipX;
                    temp._sprite = this;
                    temp._width = ani.GetWidth();
                    temp._height = ani.GetHeight();
                    return temp;
                }
            }
            return null;
        }

        private void InitDatas()
        {
            if (true == IsDataInit)
            {
                return;
            }
            Sprite temp = Model.GetCacheSprite(_name);
            if (null != temp && temp.IsDataInit && temp._fragmentsArray.Count > 0)
            {
                // TODO : not just add, need to Instantiate new obj
                _fragmentsArray.Clear();
                _fragmentsArray.AddRange(temp._fragmentsArray);
                _framesArray.Clear();
                _framesArray.AddRange(temp._framesArray);
                _animationsArray.Clear();
                _animationsArray.AddRange(temp._animationsArray);

                _fragmentTypesSize = temp._fragmentTypesSize;
                _fragmentTypes = (byte[])temp._fragmentTypes.Clone();

                _isSpriteInit = temp._isSpriteInit;
            }
            else
            {
                InitDatasFromFile();
                if (_animationsArray.Count == 0 || _framesArray.Count == 0)
                {
                    return;
                }
                Model.AddCacheSprite(_name, this);
            }
        }

        public bool InitDatasFromFile()
        {
            if (_fragmentsArray.Count > 0)
            {
                return true;
            }
            string spriteName = string.Format("{0}.s", _name);
            Data cache = Model.FindCacheSpriteData(spriteName);
            if (null == cache)
            {
                cache = DataManager.GetFileData(Const.FOLDER_RES_SPRITE, _name, ".s");
            }
            if (null == cache)
            {
                return false;
            }
            Debug.Log("InitDatasFromFile cache.Bytes.Length:" + cache.Bytes.Length);
            FromBytes(cache.Bytes);
            return true;
        }

        private void FromBytes(byte[] bytes)
        {
            int bytesIndex = 0;
            Data indexIdMapping = LoadFragment(bytes, ref bytesIndex);
            LoadFrames(bytes, ref bytesIndex, indexIdMapping);
            LoadAnimations(bytes, ref bytesIndex);
            RefreshSpriteIsInit();
        }

        private Data LoadFragment(byte[] bytes, ref int bytesIndex)
        {
            short num = DataManager.ReadByte(bytes, ref bytesIndex);

            if (num <= 0)
            {
                return null;
            }

            byte[] indexIdMapping = new byte[num * 2];
            _fragmentTypes = new byte[num];
            _fragmentTypesSize = num;

            for (int i = 0; i < num; i++)
            {
                indexIdMapping[i * 2] = DataManager.Int2Byte(i);
                indexIdMapping[i * 2 + 1] = DataManager.ReadByte(bytes, ref bytesIndex);

                string fragmentName = DataManager.ReadUTF(bytes, ref bytesIndex);
                Transform tf = new GameObject(fragmentName).transform;
                tf.SetParent(_nodeFragment);
                tf.localPosition = Vector3.zero;
                tf.localScale = Vector3.one;
                Fragment fragment = tf.gameObject.AddComponent<Fragment>();
                fragment.InitWithName(fragmentName, 0);
                _fragmentsArray.Add(fragment);

                _fragmentTypes[i] = GetPartIdByPartName(Fragment.GetTypeByString(fragmentName));
            }

            return new Data(indexIdMapping);
        }

        public static byte GetPartIdByPartName(string partName)
        {
            if (string.IsNullOrEmpty(partName))
            {
                return byte.MaxValue;
            }
            for (int i = 0; i < PARTS.Count; i++)
            {
                IntStrPair pair = PARTS[i];
                if (pair._str.Equals(partName))
                {
                    return (pair._id);
                }
            }
            return byte.MaxValue;
        }

        private void LoadFrames(byte[] bytes, ref int bytesIndex, Data indexIdData)
        {
            short num = DataManager.ReadByte(bytes, ref bytesIndex);
            if (num <= 0)
            {
                return;
            }
            for (int i = 0; i < num; i++)
            {
                int frameID = DataManager.Byte2Int(DataManager.ReadByte(bytes, ref bytesIndex));
                Transform tf = new GameObject(string.Format("Frame[{0}]", frameID)).transform;
                tf.SetParent(_nodeFrame);
                tf.localPosition = Vector3.zero;
                tf.localScale = Vector3.one;
                Frame frame = tf.gameObject.AddComponent<Frame>();
                frame.InitWithID(frameID);
                frame.FromBytes(bytes, ref bytesIndex, indexIdData);
                _framesArray.Add(frame);
            }
        }

        private void LoadAnimations(byte[] bytes, ref int bytesIndex)
        {
            short num = DataManager.ReadByte(bytes, ref bytesIndex);
            if (num <= 0)
            {
                return;
            }
            if (_animationsArray.Count != 0)
            {
                _animationsArray.Clear();
            }
            for (int i = 0; i < num; i++)
            {
                Transform tf = new GameObject(string.Format("Animation[{0}]", i)).transform;
                tf.SetParent(_nodeAnimation);
                tf.localPosition = Vector3.zero;
                tf.localScale = Vector3.one;
                Animation ani = tf.gameObject.AddComponent<Animation>();
                ani.Init();
                ani.FromBytes(bytes, ref bytesIndex);
                _animationsArray.Add(ani);
            }
        }

        public void ReplaceFragment(int fragmentID, string partName)
        {
            byte partID = GetPartIdByPartName(partName);
            if (byte.MaxValue == partID)
            {
                return;
            }
            ReplaceFragment(fragmentID, partID);
        }

        public void ReplaceFragment(int fragmentID, byte partID)
        {
            ReplaceFragment(fragmentID, 0, partID);
        }

        public void ReplaceFragment(int fragmentID, int plID, byte partID)
        {
            //Debug.Log(string.Format("ReplaceFragment fragmentID[{0}] plID[{1}] partID[{2}]", fragmentID, plID, partID));
            if (false == IsFragmentNeedReplace(fragmentID, plID, partID))
            {
                return;
            }
            string partFileName = GetPartNameByPartId(partID);
            //Debug.Log("ReplaceFragment partFileName:"+ partFileName);
            if (string.IsNullOrEmpty(partFileName))
            {
                return;
            }
            string newFragmentName;
            if (partFileName.Equals(Const.PART_TYPE_NAME_WEAPON) || partFileName.Equals(Const.PART_TYPE_NAME_CWE))
            {
                newFragmentName = string.Format("{0}_{1}_{2}", partFileName, GetWeaponType(), fragmentID);
            }
            else if (partID == Const.PART_TYPE_RIDER)
            {
                Fragment fragment = GetFragmentByPart(Const.PART_TYPE_RIDER);
                if (null == fragment)
                {
                    return;
                }
                newFragmentName = fragment.GetName();
            }
            else
            {
                newFragmentName = string.Format("{0}_{1}", partFileName, fragmentID);
            }

            //if (fragmentID <= 0)
            //{
            //    return;
            //}
            //--
            //string fragmentName = DataManager.ReadUTF(bytes, ref bytesIndex);
            Debug.Log(string.Format("ReplaceFragment-------id[{0}] name[{1}]", fragmentID, newFragmentName));
            Transform tf = new GameObject(newFragmentName).transform;
            tf.SetParent(_nodeFragment);
            tf.localPosition = Vector3.zero;
            tf.localScale = Vector3.one;
            Fragment newFragment = tf.gameObject.AddComponent<Fragment>();
            newFragment.InitWithName(newFragmentName, plID);

            //_fragmentTypes[i] = GetPartIdByPartName(Fragment.GetTypeByString(fragmentName));
            //--
            //Fragment newFragment = fragmentID <= 0 ? null : new Fragment().InitWithName(newFragmentName, plID);
            ReplaceFragmentBy(newFragment, partID);
        }

        private bool IsFragmentNeedReplace(int fragmentID, int plID, byte partID)
        {
            int fi = GetFragmentID(partID);
            int pli = GetFragmentPlID(partID);
            return !(fi == fragmentID && pli == plID);
        }

        private int GetFragmentID(byte partID)
        {
            Fragment temp = GetFragmentByPart(partID);
            if (null == temp)
            {
                return 0;
            }
            return temp.GetFragmentId();
        }

        private int GetFragmentPlID(byte partID)
        {
            Fragment temp = GetFragmentByPart(partID);
            if (null == temp)
            {
                return 0;
            }
            return temp.GetPlIndex();
        }

        private Fragment GetFragmentByPart(byte partID)
        {
            for (int i = 0; i < _fragmentTypesSize; i++)
            {
                if (_fragmentTypes[i] == partID)
                {
                    return _fragmentsArray[i];
                }
            }
            return null;
        }

        private byte GetWeaponType()
        {
            int index = _name.LastIndexOf('_');
            if (index < 0)
            {
                return byte.MaxValue;
            }
            index += 1;
            string temp = _name.Substring(index, _name.Length - index);
            int weaponType = int.Parse(temp);
            return unchecked((byte)weaponType);
            //return byte.Parse(temp);
        }

        public void ReplaceFragmentBy(Fragment newFragment, int partID)
        {
            for (int i = 0; i < _fragmentsArray.Count; i++)
            {
                if (partID == _fragmentTypes[i])
                {
                    Fragment temp = _fragmentsArray[i];
                    _fragmentsArray[i] = newFragment;
                    Destroy(temp.gameObject);
                    break;
                }
            }
        }

        public GameImage GetFragmentImageByName(string fragmentName, int plIndex)
        {
            if (string.IsNullOrEmpty(fragmentName) || _fragmentsArray.Count == 0)
            {
                return null;
            }
            string plName = plIndex <= 0 ? null : string.Format("{0}_{1}.pl", DataManager.GetFileName(fragmentName), plIndex);
            string imageName = fragmentName;
            for (int i = 0; i < _fragmentsArray.Count; i++)
            {
                Fragment temp = _fragmentsArray[i];
                if (null != temp && null != temp._gameImage.Image && temp._gameImage.Equals(imageName, plName))
                {
                    return temp._gameImage;
                }
            }
            return null;
        }

        public void InitFragmentAll()
        {
            if (IsFragmentInit || _fragmentsArray.Count == 0)
            {
                return;
            }
            foreach (Fragment fragment in _fragmentsArray)
            {
                if (null == fragment || fragment._fragmentSize > 0)
                {
                    continue;
                }
                fragment.InitDatasAndImage();
            }
            RefreshSpriteIsInit();
        }

        public bool IsAnimationExistById(int animationId)
        {
            if (0 == _animationsArray.Count)
            {
                return false;
            }
            foreach (Animation ani in _animationsArray)
            {
                return true;
            }
            return false;
        }

    }
}

