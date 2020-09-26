using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Manager;

namespace Sword
{
    public class Model
    {
        private static Dictionary<string, Sprite> _dicSprite = new Dictionary<string, Sprite>();
        private static Dictionary<string, Data> _dicSpriteData = new Dictionary<string, Data>();

        private static Dictionary<string, Fragment> _dicFragment = new Dictionary<string, Fragment>();
        private static Dictionary<string, Data> _dicFragmentData = new Dictionary<string, Data>();

        private static Sprite _faceSprite;

        public static Sprite GetCacheSprite(string key)
        {
            if (!_dicSprite.ContainsKey(key))
            {
                return null;
            }
            return _dicSprite[key];
        }

        public static bool AddCacheSprite(string key, Sprite sprite)
        {
            if (_dicSprite.Count > 20)
            {
                _dicSprite.Clear();
            }
            if (_dicSprite.ContainsKey(key))
            {
                return false;
            }
            _dicSprite[key] = sprite;
            return true;
        }

        public static Data FindCacheSpriteData(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            if (!_dicSpriteData.ContainsKey(key))
            {
                return null;
            }
            return _dicSpriteData[key];
        }

        public static Fragment GetCacheFragment(string key)
        {
            if (!_dicFragment.ContainsKey(key))
            {
                return null;
            }
            return _dicFragment[key];
        }

        public static bool AddCacheFragment(string key, Fragment fragment)
        {
            if (_dicFragment.Count > 20)
            {
                _dicFragment.Clear();
            }
            if (_dicFragment.ContainsKey(key))
            {
                return false;
            }
            _dicFragment[key] = fragment;
            return true;
        }

        public static Data FindCacheFragmentData(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return _dicFragmentData[key];
        }

        public static GameImage FindGameImageInGame(string fragmentName, int plIndex)
        {
            if (string.IsNullOrEmpty(fragmentName))
            {
                return null;
            }
            if (null != _faceSprite)
            {
                GameImage gameImage = _faceSprite.GetFragmentImageByName(fragmentName, plIndex);
                if (null != gameImage)
                {
                    return gameImage;
                }
            }
            return null;
        }
    }
}

