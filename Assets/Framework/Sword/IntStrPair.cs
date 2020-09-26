using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sword
{
    public class IntStrPair
    {
        public byte _id;
        public string _str;

        private static List<IntStrPair> _listSuffix = new List<IntStrPair>();

        public IntStrPair InitWithID(byte id, string str)
        {
            _id = id;
            _str = str;
            return this;
        }

        private static void InitSuffix()
        {
            if (_listSuffix.Count > 0)
            {
                return;
            }
            _listSuffix.Add(new IntStrPair().InitWithID(Const.EXT_PNG, "png"));
            _listSuffix.Add(new IntStrPair().InitWithID(Const.EXT_PL, "pl"));
            _listSuffix.Add(new IntStrPair().InitWithID(Const.EXT_S, "s"));
            _listSuffix.Add(new IntStrPair().InitWithID(Const.EXT_F, "f"));
        }

        public static IntStrPair GetPairById(byte id)
        {
            if (_listSuffix.Count == 0)
            {
                InitSuffix();
            }
            foreach (IntStrPair temp in _listSuffix)
            {
                if (temp._id == id)
                {
                    return temp;
                }
            }
            return null;
        }

        public static IntStrPair GetPairByStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            if (_listSuffix.Count == 0)
            {
                InitSuffix();
            }
            foreach (IntStrPair temp in _listSuffix)
            {
                if (temp._str.Equals(str))
                {
                    return temp;
                }
            }
            return null;
        }
    }
}

