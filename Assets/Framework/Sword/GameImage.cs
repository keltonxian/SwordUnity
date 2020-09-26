using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Manager;
using UnityEngine.Rendering;

namespace Sword
{
    public class GameImage : MonoBehaviour
    {
        private Texture2D _image;
        public Texture2D Image
        {
            get
            {
                return _image;
            }   
            set
            {
                _image = value;
                //Debug.Log("_image.width:" + _image.width);
                //Debug.Log("_image.height:" + _image.height);
            }
        }

        private Dictionary<string, GameObject> _dicRenderer = new Dictionary<string, GameObject>();

        private string _imageName;
        public string ImageName
        {
            get
            {
                return _imageName;
            }
        }
        private string _plName;
        public string PlName
        {
            get
            {
                return _plName;
            }
        }

        private Data _imageDatas;
        public Data ImageDatas
        {
            get
            {
                return _imageDatas;
            }
            set
            {
                _imageDatas = value;
            }
        }
        private Data _plDatas;
        public Data PlDatas
        {
            get
            {
                return _plDatas;
            }
            set
            {
                _plDatas = value;
            }
        }

        private void Init()
        {
        }

        public void InitWithImage(Texture2D image)
        {
            Init();
            Image = image;
        }

        public void InitWithImageName(string imageName, string plName)
        {
            Init();
            _imageName = imageName;
            _plName = plName;
        }

        public void InitWithImageName(string imageName)
        {
            InitWithImageName(imageName, null);
        }

        public void AddRenderer(byte id, byte x, byte y, byte width, byte height, sbyte avatarX, sbyte avatarY, byte trans)
        {
            string key = string.Format("{0}_{1}", id, trans);
            Transform tf = new GameObject(string.Format("Renderer[{0}]", key)).transform;
            tf.SetParent(transform);
            tf.localPosition = Vector3.zero;
            tf.localScale = Vector3.one;

            Material material = InitMat(Shader.Find("Unlit/Transparent"), BlendMode.SrcAlpha, BlendMode.OneMinusSrcAlpha, Color.white, 0.02f, 1f, CullMode.Back);
            material.SetTexture("_MainTex", Image);
            float ox = 1.0f * x / Image.width;
            float oy = 1.0f * y / Image.height;
            float ow = 1.0f * width / Image.width;
            float oh = 1.0f * height / Image.height;
            //if (3 == trans)
            //{
            //    ox = 1.0f * x / Image.width;
            //    oy = 1.0f * y / Image.height;
            //    oh = 1.0f * height / Image.width;
            //    ow = 1.0f * width / Image.height;
            //}
            Debug.Log(string.Format("key:{0}", key));
            Debug.Log(string.Format("x:{0}, y:{1}", x, y));
            Debug.Log(string.Format("ox:{0}, oy:{1}", ox, oy));
            material.SetTextureOffset("_MainTex", new Vector2(ox, 1 - oy - oh));
            material.SetTextureScale("_MainTex", new Vector2(ow, oh));
            Mesh mesh = InitMesh(tf.gameObject, material, width, height, trans);

            _dicRenderer.Add(key, tf.gameObject);
            //SpriteRenderer sr = tf.gameObject.AddComponent<SpriteRenderer>();
            //sr.sprite = UnityEngine.Sprite.Create(tex, new Rect(x, texHeight - y - height, width, height), new Vector2(0, 1), Const.SPRITE_PIXEL_PER_UNIT, 0, SpriteMeshType.FullRect);
        }

        public void SetAllRendererShow(bool isShow)
        {
            foreach (GameObject go in _dicRenderer.Values)
            {
                go.SetActive(isShow);
            }
        }

        public void ShowRenderer(byte id, byte x, byte y, byte width, byte height, sbyte avatarX, sbyte avatarY, byte trans, Vector3 pos, Vector3 scale)
        {
            string key = string.Format("{0}_{1}", id, trans);
            if (!_dicRenderer.ContainsKey(key))
            {
                AddRenderer(id, x, y, width, height, avatarX, avatarY, trans);
            }
            GameObject go = _dicRenderer[key];
            go.SetActive(true);
            Transform tf = go.transform;
            tf.localPosition = pos;
            tf.localScale = scale;
        }

        private void Update()
        {
            //if (null != _material)
            //{
            //    //_material.SetTextureOffset("_MainTex", new Vector2(0.2f, 0.2f));
            //    //_material.SetTextureScale("_MainTex", new Vector2(1, 1));
            //}
            
        }

        public void UpdateTexProp(Rect texRect)
        {
            //MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            //meshRenderer.material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.5f));
            //meshRenderer.material.SetTextureScale("_MainTex", new Vector2(0.5f, 0.5f));
            //_material.SetTextureOffset("_MainTex", new Vector2(0, 0));
            //_material.SetTextureScale("_MainTex", new Vector2(1, 1));
            //_material.SetTextureOffset("_MainTex", new Vector2(texRect.x /Image.width, texRect.y / Image.height));
            //_material.SetTextureScale("_MainTex", new Vector2(0.5f, 0.5f));
        }

        private Material InitMat(Shader shader, BlendMode blendSrc, BlendMode blendDst, Color color, float cutoff, float alpha, CullMode cullMode)
        {
            Material mat = new Material(shader);
            mat.SetFloat("_BlendSrc", (int)blendSrc);
            mat.SetFloat("_BlendDst", (int)blendDst);
            mat.SetColor("_Color", color);
            mat.SetFloat("_Cutoff", cutoff);
            mat.SetFloat("_Alpha", alpha);
            mat.SetFloat("_CullMode", (int)cullMode);
            return mat;
        }

        //private Mesh InitMesh(GameObject parent, Material material, int width, int height, byte trans)
        //{
        //    Mesh mesh = new Mesh();
        //    mesh.vertices = new Vector3[]
        //    {
        //    new Vector3(0, -height * 0.01f),
        //    new Vector3(0, 0),
        //    new Vector3(width * 0.01f, 0),
        //    new Vector3(width * 0.01f, -height * 0.01f),
        //    };
        //    if (1 == trans)
        //    {
        //        mesh.uv = new Vector2[]
        //        {
        //        new Vector2(1,0),
        //        new Vector2(1,1),
        //        new Vector2(0,1),
        //        new Vector2(0,0)
        //        };
        //    }
        //    else
        //    {
        //        mesh.uv = new Vector2[]
        //        {
        //        new Vector2(0,0),
        //        new Vector2(0,1),
        //        new Vector2(1,1),
        //        new Vector2(1,0)
        //        };
        //    }
        //    mesh.triangles = new int[] { 0, 1, 2, 2, 3, 0 };
        //    mesh.RecalculateBounds();
        //    mesh.RecalculateNormals();

        //    MeshFilter meshFilter = parent.GetComponent<MeshFilter>();
        //    if (null == meshFilter)
        //    {
        //        meshFilter = parent.AddComponent<MeshFilter>();
        //    }
        //    meshFilter.mesh = mesh;

        //    MeshRenderer meshRenderer = parent.GetComponent<MeshRenderer>();
        //    if (null == meshRenderer)
        //    {
        //        meshRenderer = parent.AddComponent<MeshRenderer>();
        //    }
        //    meshRenderer.material = material;
        //    //meshRenderer.sortingLayerName = SortingLayerName;
        //    //meshRenderer.sortingOrder = SortingOrder;
        //    return mesh;
        //}

        private Mesh InitMesh(GameObject parent, Material material, int width, int height, byte trans)
        {
            Vector3[][] vecs = {
                new Vector3[] {
                    new Vector3(0, -height * 0.01f),
                    new Vector3(width * 0.01f, -height * 0.01f),
                    new Vector3(0, 0),
                    new Vector3(width * 0.01f, 0),
                },
                new Vector3[] {
                    new Vector3(0, -width * 0.01f),
                    new Vector3(height * 0.01f, -width * 0.01f),
                    new Vector3(0, 0),
                    new Vector3(height * 0.01f, 0),
                },
            };
            int vecsIndex;
            int[] verticesIndexs;
            int[] triangles;
            Mesh mesh = new Mesh();
            switch (trans)
            {
                case Const.TRANS_NONE:
                    //  2 +----+ 3
                    //     \
                    //      \
                    //       \
                    //        \
                    //  0 +----+1
                    vecsIndex = 0;
                    verticesIndexs = new int[] { 0, 1, 2, 3 };
                    triangles = new int[] { 0, 2, 1, 1, 2, 3 };
                    break;
                case Const.TRANS_MIRROR:
                    //  3 +----+ 2
                    //        /
                    //       /
                    //      /
                    //     /
                    //  1 +----+ 0
                    vecsIndex = 0;
                    verticesIndexs = new int[] { 1, 0, 3, 2 };
                    triangles = new int[] { 1, 3, 2, 2, 0, 1 };
                    break;
                case Const.TRANS_MIRROR_ROT180:
                    //  0 +----+ 1
                    //        /
                    //       /
                    //      /
                    //     /
                    //  2 +----+ 3
                    vecsIndex = 0;
                    verticesIndexs = new int[] { 2, 3, 0, 1 };
                    triangles = new int[] { 2, 0, 1, 1, 3, 2 };
                    break;
                case Const.TRANS_ROT90:
                    //  2 +    + 3
                    //    |   /|
                    //    |  / |
                    //    | /  |
                    //    |/   |
                    //  0 +    + 1
                    vecsIndex = 1;
                    verticesIndexs = new int[] { 2, 0, 3, 1 };
                    triangles = new int[] { 0, 2, 3, 3, 1, 0 };
                    break;
                case Const.TRANS_ROT180:
                    //  2 +----+ 3
                    //     \
                    //      \
                    //       \
                    //        \
                    //  0 +----+ 1
                    vecsIndex = 0;
                    verticesIndexs = new int[] { 3, 2, 1, 0 };
                    triangles = new int[] { 0, 2, 1, 1, 2, 3 };
                    break;
                case Const.TRANS_ROT270:
                    //  2 +    + 3
                    //    |   /|
                    //    |  / |
                    //    | /  |
                    //    |/   |
                    //  0 +    + 1
                    vecsIndex = 1;
                    verticesIndexs = new int[] { 1, 3, 0, 2 };
                    triangles = new int[] { 0, 2, 3, 3, 1, 0 };
                    break;
                case Const.TRANS_MIRROR_ROT270:
                    //  2 +    + 3
                    //    |   /|
                    //    |  / |
                    //    | /  |
                    //    |/   |
                    //  0 +    + 1
                    vecsIndex = 1;
                    verticesIndexs = new int[] { 3, 1, 2, 0 };
                    triangles = new int[] { 0, 2, 3, 3, 1, 0 };
                    break;
                case Const.TRANS_MIRROR_ROT90:
                    //  2 +    + 3
                    //    |\   |
                    //    | \  |
                    //    |  \ |
                    //    |   \|
                    //  0 +    + 1
                    vecsIndex = 1;
                    verticesIndexs = new int[] { 0, 2, 1, 3 };
                    triangles = new int[] { 0, 2, 3, 3, 1, 0 };
                    break;
                default:
                    vecsIndex = 0;
                    verticesIndexs = new int[] { 0, 1, 2, 3 };
                    triangles = new int[] { 0, 2, 1, 1, 2, 3 };
                    break;
            }

            mesh.vertices = new Vector3[]
            {
                vecs[vecsIndex][verticesIndexs[0]],
                vecs[vecsIndex][verticesIndexs[1]],
                vecs[vecsIndex][verticesIndexs[2]],
                vecs[vecsIndex][verticesIndexs[3]],
            };

            mesh.uv = new Vector2[]
            {
            new Vector2(0,0), // 1
            new Vector2(1,0), // 2
            new Vector2(0,1), // 3
            new Vector2(1,1), // 4
            };

            mesh.triangles = triangles;
            
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            MeshFilter meshFilter = parent.GetComponent<MeshFilter>();
            if (null == meshFilter)
            {
                meshFilter = parent.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = mesh;

            MeshRenderer meshRenderer = parent.GetComponent<MeshRenderer>();
            if (null == meshRenderer)
            {
                meshRenderer = parent.AddComponent<MeshRenderer>();
            }
            meshRenderer.material = material;
            //meshRenderer.sortingLayerName = SortingLayerName;
            //meshRenderer.sortingOrder = SortingOrder;
            return mesh;
        }

        public bool Equals(string imageName, string plName)
        {
            if (string.IsNullOrEmpty(_imageName))
            {
                return false;
            }

            if ((null != _imageName && _imageName.Equals(imageName)) && ((null != _plName && _plName.Equals(plName)) || (null == _plName && null == plName)))
            {
                return true;
            }
            return false;
        }

        public void InitImage()
        {
            Data newImageData = DataManager.ApplyPLETData(ImageDatas, PlDatas);
            //Debug.Log("new data len:" + newImageData.Length);
            //PrintBytes(ImageDatas);
            //PrintBytes(PlDatas);
            //PrintBytes(newImageData);
            Texture2D image = new Texture2D(2, 2);
            image.LoadImage(newImageData.Bytes);
            Image = image;
            ImageDatas = null;
            PlDatas = null;
        }

        //public static void PrintBytes(Data data)
        //{
        //    string t = "";
        //    for (int i = 0; i < data.Bytes.Length; i++)
        //    {
        //        t += (data.Bytes[i] + ",");
        //    }
        //    Debug.Log("new data[" + t + "]");
        //}
    }
}

