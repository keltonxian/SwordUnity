using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sword
{
    public class Const
    {
        public const int SPRITE_PIXEL_PER_UNIT = 100;

        public const byte TRANS_NONE = 0;
        public const byte TRANS_MIRROR = 1;
        public const byte TRANS_MIRROR_ROT180 = 2;
        public const byte TRANS_ROT90 = 3;
        public const byte TRANS_ROT180 = 4;
        public const byte TRANS_ROT270 = 5;
        public const byte TRANS_MIRROR_ROT270 = 6;
        public const byte TRANS_MIRROR_ROT90 = 7;

        public const byte TRANSFORM_NONE = 0;
        public const byte TRANSFORM_MIRROR = 1;
        public const byte TRANSFORM_MIRROR_ROT180 = 2;
        public const byte TRANSFORM_ROT90 = 3;
        public const byte TRANSFORM_ROT180 = 4;
        public const byte TRANSFORM_ROT270 = 5;

        public const byte ANCHOR_HCENTER = 1;
        public const byte ANCHOR_VCENTER = 2;
        public const byte ANCHOR_LEFT = 4;
        public const byte ANCHOR_RIGHT = 8;
        public const byte ANCHOR_TOP = 16;
        public const byte ANCHOR_BOTTOM = 32;
        public const byte ANCHOR_BASELINE = 64;

        public const byte TOP_LEFT = ANCHOR_LEFT | ANCHOR_TOP;
        public const byte TOP_HCENTER = ANCHOR_TOP | ANCHOR_HCENTER;
        public const byte TOP_RIGHT = ANCHOR_TOP | ANCHOR_RIGHT;
        public const byte BOTTOM_HCENTER = ANCHOR_BOTTOM | ANCHOR_HCENTER;
        public const byte BOTTOM_LEFT = ANCHOR_BOTTOM | ANCHOR_LEFT;
        public const byte BOTTOM_RIGHT = ANCHOR_BOTTOM | ANCHOR_RIGHT;
        public const byte H_V = ANCHOR_HCENTER | ANCHOR_VCENTER;
        public const byte LEFT_VCENTER = ANCHOR_LEFT | ANCHOR_VCENTER;
        public const byte RIGHT_VCENTER = ANCHOR_RIGHT | ANCHOR_VCENTER;

        public const byte DIR_RIGHT = 0;
        public const byte DIR_LEFT = 1;
        public const byte DIR_DOWN = 2;
        public const byte DIR_UP = 3;
        public const byte DIR_NONE = 4;
        public const byte DIR_LEFT_UP = 5;
        public const byte DIR_LEFT_DOWN = 6;
        public const byte DIR_RIGHT_UP = 7;
        public const byte DIR_RIGHT_DOWN = 8;

        public const int INFINITE_LOOP = 100000;

        public const int RES_IMAGE = 1;
        public const int RES_SPRITE = 2;
        public const int RES_FRAME = 3;
        public const int RES_PL = 4;

        public const byte EXT_PNG = 1;
        public const byte EXT_PL = 2;
        public const byte EXT_F = 3;
        public const byte EXT_S = 4;

        public const byte PET_TYPE_PET_ANI = 0;

        public const string PART_TYPE_NAME_TIARA = "tiara";
        public const string PART_TYPE_NAME_HAIR = "hair";
        public const string PART_TYPE_NAME_HEAD = "head";
        public const string PART_TYPE_NAME_BODY = "body";
        public const string PART_TYPE_NAME_HANDS = "hands";
        public const string PART_TYPE_NAME_FEET = "feet";
        public const string PART_TYPE_NAME_TAIL = "tail";
        public const string PART_TYPE_NAME_WING = "wing";
        public const string PART_TYPE_NAME_WEAPON = "weapon";
        public const string PART_TYPE_NAME_SHADOW = "shadow";
        public const string PART_TYPE_NAME_SHOULDER = "shoulder";
        public const string PART_TYPE_NAME_CWE = "cwe";
        public const string PART_TYPE_NAME_CWNECK = "cwneck";
        public const string PART_TYPE_NAME_CW = "cw";
        public const string PART_TYPE_NAME_G = "g";

        public const byte PART_TYPE_FEET = 1;
        public const byte PART_TYPE_HAND = 2;
        public const byte PART_TYPE_HAIR = 3;
        public const byte PART_TYPE_FACE = 4;
        public const byte PART_TYPE_BODY = 5;
        public const byte PART_TYPE_WEAPON = 6;
        public const byte PART_TYPE_SHIELD = 7;
        public const byte PART_TYPE_TAIL = 8;
        public const byte PART_TYPE_SHOULDER = 9;
        public const byte PART_TYPE_TIARA = 10;
        public const byte PART_TYPE_SHADOW = 11;
        public const byte PART_TYPE_WING = 12;
        public const byte PART_TYPE_PET_SPEC = 13;
        public const byte PART_TYPE_PET_GEN = 14;
        public const byte PART_TYPE_RIDER = 15;

        public const string FOLDER_RES_SPRITE = "res/sprite/";

        public const byte TEAM_NONE = 0;
        public const byte FEMALE = 0;
        public const byte MALE = 1;

        public const byte SCHOOL_NONE = 0;
        public const byte SCHOOL_WD = 1;
        public const byte SCHOOL_XY = 2;
        public const byte SCHOOL_GG = 3;
        public const byte SCHOOL_TM = 4;
        public const byte SCHOOL_MJ = 5;
    }
}

