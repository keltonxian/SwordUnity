using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PureMVC.Core;
using Sword;
using System.Collections.Generic;
using PureMVC.Manager;

public class SwordView : Base
{
    public SpriteActor _actor;
    private int _hairIndex;
    private int _hairColorIndex;
    private int _faceIndex;
    public Transform[] _hairIconParent;
    public Transform[] _faceIconParent;
    public Transform[] _hairColorIcon;
    public Transform[] _sexIcon;
    public Transform _selectFrameParent;
    private Transform _hairSelectFrame;
    private Transform _hairColorSelectFrame;
    private Transform _faceSelectFrame;
    public Transform _selectTabParent;
    private Transform _sexSelectTab;

    public InputField _inputSpriteName;
    public InputField _inputFeet;
    public InputField _inputWeapon;
    public InputField _inputTiara;
    public InputField _inputHand;
    public InputField _inputShoulder;
    public InputField _inputTail;
    public InputField _inputBody;
    public InputField _inputWing;

    private List<Data> _listHairData = new List<Data>();
    private List<Fragment> _listHairIcon = new List<Fragment>();
    private List<Data> _listFaceData = new List<Data>();
    private List<Fragment> _listFaceIcon = new List<Fragment>();
    private byte _sex = Const.MALE;
    private bool _isRun = false;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        byte[][] hairData = {
            new byte [] { Const.FEMALE, 4, 5, 6, 7, 29 },
            new byte [] { Const.MALE, 20, 1, 2, 3, 26 }
        };
        byte[][] faceData = {
            new byte [] { Const.FEMALE, 3, 4, 10, 8, 9 },
            new byte [] { Const.MALE, 6, 1, 2, 7, 5 } };
        for (int i = 0; i < 2; i++)
        {
            Data data = new Data(hairData[i]);
            _listHairData.Add(data);
            data = new Data(faceData[i]);
            _listFaceData.Add(data);
        }

        _inputSpriteName.text = "a_p_9";
        _inputFeet.text = "1";
        _inputHand.text = "6";
        _inputBody.text = "26";
        _inputWeapon.text = "1";
        _inputShoulder.text = "0";
        _inputWing.text = "0";
        _inputTiara.text = "0";
        _inputTail.text = "0";

        _hairIndex = 0;
        _hairColorIndex = 0;
        _faceIndex = 0;

        //// sex
        //int randomNumber = Random.Range(0, 2);
        //_sex = (randomNumber == 1 ? Const.FEMALE : Const.MALE);

        //// hair
        //int tempHairIndex;
        //randomNumber = Random.Range(1, 6);
        //switch (randomNumber)
        //{
        //    case 1:
        //        tempHairIndex = (_sex == Const.FEMALE) ? hairData[0][randomNumber] : hairData[1][randomNumber];
        //        break;
        //    case 2:
        //        tempHairIndex = (_sex == Const.FEMALE) ? hairData[0][randomNumber] : hairData[1][randomNumber];
        //        break;
        //    case 3:
        //        tempHairIndex = (_sex == Const.FEMALE) ? hairData[0][randomNumber] : hairData[1][randomNumber];
        //        break;
        //    case 4:
        //        tempHairIndex = (_sex == Const.FEMALE) ? hairData[0][randomNumber] : hairData[1][randomNumber];
        //        break;
        //    case 5:
        //        tempHairIndex = (_sex == Const.FEMALE) ? hairData[0][randomNumber] : hairData[1][randomNumber];
        //        break;
        //    default:
        //        tempHairIndex = (_sex == Const.FEMALE) ? hairData[0][1] : hairData[1][1];
        //        break;
        //}

        //// hair color
        //randomNumber = Random.Range(0, 4);
        //int tempHairColorIndex = randomNumber;

        //// face
        //int tempFaceIndex;
        //randomNumber = Random.Range(0, 5);
        //switch (randomNumber)
        //{
        //    case 0:
        //        tempFaceIndex = (_sex == Const.FEMALE) ? 3 : 6;
        //        break;
        //    case 1:
        //        tempFaceIndex = (_sex == Const.FEMALE) ? 4 : 1;
        //        break;
        //    case 2:
        //        tempFaceIndex = (_sex == Const.FEMALE) ? 10 : 2;
        //        break;
        //    case 3:
        //        tempFaceIndex = (_sex == Const.FEMALE) ? 8 : 7;
        //        break;
        //    case 4:
        //        tempFaceIndex = (_sex == Const.FEMALE) ? 9 : 5;
        //        break;
        //    default:
        //        tempFaceIndex = (_sex == Const.FEMALE) ? 3 : 6;
        //        break;
        //}

        //// school
        //randomNumber = Random.Range(0, 4);
        //int schoolTag = randomNumber;
        //switch (schoolTag)
        //{
        //    case 0:
        //        _school = Const.SCHOOL_MJ;
        //        break;
        //    case 1:
        //        _school = Const.SCHOOL_WD;
        //        break;
        //    case 2:
        //        _school = Const.SCHOOL_XY;
        //        break;
        //    case 3:
        //        _school = Const.SCHOOL_TM;
        //        break;
        //    default:
        //        _school = Const.SCHOOL_MJ;
        //        schoolTag = 0;
        //        break;
        //}

        _sex = Const.MALE;


        //InitHairAndFaceImage(_sex);
    }

    public void Enter()
    {
        SetSexIndex(_sex, true);
        UpdateActor();
    }

    public void UpdateActor()
    {
        if (null == _actor)
        {
            return;
        }
        string spriteName = _inputSpriteName.text;
        if (null == _actor._sprite)
        {
            _actor.InitWithSpriteName(spriteName);
        }
        else if (!_actor._spriteName.Equals(spriteName))
        {
            _actor.UpdateSprite(spriteName);
        }

        Sword.Sprite sprite = _actor._sprite;
        sprite.ReplaceFragment(int.Parse(_inputWing.text), Const.PART_TYPE_WING);
        sprite.ReplaceFragment(int.Parse(_inputShoulder.text), Const.PART_TYPE_SHOULDER);
        sprite.ReplaceFragment(int.Parse(_inputTiara.text), Const.PART_TYPE_TIARA);
        sprite.ReplaceFragment(int.Parse(_inputTail.text), Const.PART_TYPE_TAIL);
        sprite.ReplaceFragment(int.Parse(_inputWeapon.text), Const.PART_TYPE_WEAPON);
        sprite.ReplaceFragment(int.Parse(_inputHand.text), Const.PART_TYPE_HAND);
        sprite.ReplaceFragment(int.Parse(_inputFeet.text), Const.PART_TYPE_FEET);
        sprite.ReplaceFragment(int.Parse(_inputBody.text), Const.PART_TYPE_BODY);

        _actor.UpdateFaceAndHair(_hairIndex, _faceIndex, _sex, _hairColorIndex);
    }

    private void InitHairAndFaceImage(int sex)
    {
        InitHairImage(sex);
        InitFaceImage(sex);
    }

    private void InitHairImage(int sex)
    {
        Data byteData = _listHairData[sex];
        byte[] bytes = byteData.Bytes;
        int len = bytes.Length;

        while (_listHairIcon.Count > 0)
        {
            Fragment fragment = _listHairIcon[0];
            Destroy(fragment.gameObject);
            _listHairIcon.Remove(fragment);
        }

        for (int i = 0; i < len - 1; i++)
        {
            string fragmentName = string.Format("hair_{0}", bytes[i + 1]);
            Transform tf = new GameObject(fragmentName).transform;
            tf.SetParent(_hairIconParent[i]);
            tf.localPosition = Vector3.zero;
            tf.localScale = Vector3.one;
            Fragment fragment = tf.gameObject.AddComponent<Fragment>();
            fragment.InitWithName(fragmentName, 0);
            Image image = tf.gameObject.AddComponent<Image>();
            image.sprite = fragment.LoadUIImage();
            image.SetNativeSize();
            _listHairIcon.Add(fragment);
        }
    }

    private void InitFaceImage(int sex)
    {
        Data byteData = _listFaceData[sex];
        byte[] bytes = byteData.Bytes;
        int len = bytes.Length;

        while (_listFaceIcon.Count > 0)
        {
            Fragment fragment = _listFaceIcon[0];
            Destroy(fragment.gameObject);
            _listFaceIcon.Remove(fragment);
        }

        for (int i = 0; i < len - 1; i++)
        {
            string fragmentName = string.Format("head_{0}", bytes[i + 1]);
            Transform tf = new GameObject(fragmentName).transform;
            tf.SetParent(_faceIconParent[i]);
            tf.localPosition = Vector3.zero;
            tf.localScale = Vector3.one;
            Fragment fragment = tf.gameObject.AddComponent<Fragment>();
            fragment.InitWithName(fragmentName, 0);
            Image image = tf.gameObject.AddComponent<Image>();
            image.sprite = fragment.LoadUIImage();
            image.SetNativeSize();
            _listFaceIcon.Add(fragment);
        }
    }

    private byte GetHairIdByIndex(int index, int sex)
    {
        for (int i = 0; i < _listHairData.Count; i++)
        {
            Data data = _listHairData[i];
            byte[] bytes = data.Bytes;
            if (sex != bytes[0])
            {
                continue;
            }
            if (index < 0 || index >= bytes.Length - 1)
            {
                return bytes[1];
            }
            else
            {
                return bytes[index + 1];
            }
        }
        return (byte)(sex == Const.FEMALE ? 4 : 20);
    }

    private byte GetFaceIdByIndex(int index, int sex)
    {
        for (int i = 0; i < _listFaceData.Count; i++)
        {
            Data data = _listFaceData[i];
            byte[] bytes = data.Bytes;
            if (sex != bytes[0])
            {
                continue;
            }
            if (index < 0 || index >= bytes.Length - 1)
            {
                return bytes[1];
            }
            else
            {
                return bytes[index + 1];
            }
        }
        return (byte)(sex == Const.FEMALE ? 3 : 6);
    }

    public void OnClickSex(int sex)
    {
        SetSexIndex(sex);
        UpdateActor();
    }

    private void SetSexIndex(int index, bool isForceSet = false)
    {
        if (false == isForceSet && _sex == index)
        {
            return;
        }
        _sex = (byte)index;
        InitHairAndFaceImage(_sex);
        int hairIndex = 0;
        int hairColorIndex = 0;
        int faceIndex = 0;
        SetHairIndex(hairIndex, true);
        SetHairColorIndex(hairColorIndex, true);
        SetFaceIndex(faceIndex, true);
        Transform t = _sexIcon[index].transform;
        if (null == _sexSelectTab)
        {
            UIManager.ShowSelectTab(_selectTabParent, (GameObject obj) =>
            {
                _sexSelectTab = obj.transform;
                _sexSelectTab.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                _sexSelectTab.position = t.position;
            }, t);
        }
        else
        {
            _sexSelectTab.position = t.position;
        }
    }

    public void OnClickHairStyle(int index)
    {
        if (false == SetHairIndex(index))
        {
            return;
        }
        UpdateActor();
    }

    private bool SetHairIndex(int index, bool isForceSet = false)
    {
        int hairId = GetHairIdByIndex(index, _sex);
        if (false == isForceSet && hairId == _hairIndex)
        {
            return false;
        }
        _hairIndex = hairId;
        Transform t = _listHairIcon[index].transform;
        if (null == _hairSelectFrame)
        {
            UIManager.ShowSelectFrame(_selectFrameParent, (GameObject obj) =>
            {
                _hairSelectFrame = obj.transform;
                _hairSelectFrame.position = t.position;
            }, t);
        }
        else
        {
            _hairSelectFrame.position = t.position;
        }
        return true;
    }

    public void OnClickHairColor(int index)
    {
        if (false == SetHairColorIndex(index))
        {
            return;
        }
        UpdateActor();
    }

    private bool SetHairColorIndex(int index, bool isForceSet = false)
    {
        if (false == isForceSet && index == _hairColorIndex)
        {
            return false;
        }
        _hairColorIndex = index;
        Transform t = _hairColorIcon[index].transform;
        if (null == _hairColorSelectFrame)
        {
            UIManager.ShowSelectFrame(_selectFrameParent, (GameObject obj) =>
            {
                _hairColorSelectFrame = obj.transform;
                _hairColorSelectFrame.position = t.position;
            }, t);
        }
        else
        {
            _hairColorSelectFrame.position = t.position;
        }
        return true;
    }

    public void OnClickFace(int index)
    {
        if (false == SetFaceIndex(index))
        {
            return;
        }
        UpdateActor();
    }

    private bool SetFaceIndex(int index, bool isForceSet = false)
    {
        int faceId = GetFaceIdByIndex(index, _sex);
        if (false == isForceSet && faceId == _faceIndex)
        {
            return false;
        }
        _faceIndex = faceId;
        Transform t = _listFaceIcon[index].transform;
        if (null == _faceSelectFrame)
        {
            UIManager.ShowSelectFrame(_selectFrameParent, (GameObject obj) =>
            {
                _faceSelectFrame = obj.transform;
                _faceSelectFrame.position = t.position;
            }, t);
        }
        else
        {
            _faceSelectFrame.position = t.position;
        }
        return true;
    }

    public void OnClickRefresh()
    {
        UpdateActor();
    }

    public void OnClickRun()
    {
        _isRun = !_isRun;
        //UpdateActor();
        if(null == _actor)
        {
            return;
        }
        _actor.SetAnimationByDir(Const.DIR_LEFT, _isRun);
    }
}
