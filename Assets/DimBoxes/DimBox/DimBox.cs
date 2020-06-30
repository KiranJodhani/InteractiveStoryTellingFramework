using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
//using System.Linq;


namespace DimBoxes
{
    [System.Serializable]
    public class bool_xyz
    {
        public bool width = false; public bool depth = false; public bool height = false;
    }


    public enum w_placement { top_front = 0, bottom_front = 1, top_back = 2, bottom_back = 3 };
    public enum d_placement { top_right = 0, bottom_right = 1, top_left = 2, bottom_left = 3 };
    public enum h_placement { front_right = 0, front_left = 1, back_right = 2, back_left = 3 };

    public enum dimension_alignment { UpperCenter = TextAnchor.UpperCenter, LowerCenter = TextAnchor.LowerCenter };


    [ExecuteInEditMode]

    public class DimBox : MonoBehaviour
    {

        public bool colliderBased = false;
        public bool permanent = false; //permanent/onMouseDown
        public w_placement widthPlacement;
        public dimension_alignment widthAlignment = dimension_alignment.UpperCenter;
        public d_placement depthPlacement;
        public dimension_alignment depthAlignment = dimension_alignment.UpperCenter;
        public h_placement heightPlacement;
        public dimension_alignment heightAlignment = dimension_alignment.UpperCenter;
        public bool_xyz extensions = new bool_xyz();
        public bool_xyz faceCamera = new bool_xyz();

        public Color lineColor = new Color(0f, 1f, 0.4f, 0.74f);
        public float extentionDist = 0.1f;
        public Material letterMaterial;
        public Font font;

        public bool drawArrows = true;
        public float arrowSize = 0.12f;

        public bool Z_up_orientation = false;

        public bool setupOnAwake = false;

        private Vector3 topFrontLeft;
        private Vector3 topFrontRight;
        private Vector3 topBackLeft;
        private Vector3 topBackRight;
        private Vector3 bottomFrontLeft;
        private Vector3 bottomFrontRight;
        private Vector3 bottomBackLeft;
        private Vector3 bottomBackRight;

        private Vector3[] widthExt;

        private Vector3[] depthExt;
        private Vector3 depthLookAt;
        private Vector3[] heightExt;

        [HideInInspector]
        public GameObject hDimension;
        [HideInInspector]
        public GameObject dDimension;
        [HideInInspector]
        public GameObject wDimension;
        [HideInInspector]
        public GameObject hDimensionMesh;
        [HideInInspector]
        public GameObject dDimensionMesh;
        [HideInInspector]
        public GameObject wDimensionMesh;
        [HideInInspector]
        public TextMesh htm;
        [HideInInspector]
        public TextMesh dtm;
        [HideInInspector]
        public TextMesh wtm;

        private float alpha;

        private Bounds bound;
        private Vector3 boundOffset;
        [HideInInspector]
        public Bounds colliderBound;
        [HideInInspector]
        public Vector3 colliderBoundOffset;
        [HideInInspector]
        public Bounds meshBound;
        [HideInInspector]
        public Vector3 meshBoundOffset;

        private Vector3[] corners;
        private Vector3[,] arrows;

        private Vector3[,] lines;

        private Vector3[,] triangles;

        private Quaternion quat;

        private Camera mcamera;

        private DimBoxes.DrawLines cameralines;

        private MeshFilter[] meshes;
        //private bool isStatic = false;
        [HideInInspector]
        public Vector3 startingScale;
        private Vector3 previousScale;
        private Vector3 startingBoundSize;
        private Vector3 startingBoundCenterLocal;
        private Vector3 previousPosition;
        private Quaternion previousRotation;

        void Reset()
        {
            TextMesh[] tms = GetComponentsInChildren<TextMesh>();
            for (int i = 0; i < tms.Length; i++) DestroyImmediate(tms[i].transform.parent.gameObject);

            meshes = GetComponentsInChildren<MeshFilter>();
            calculateBounds();
            Start();
        }

        void Awake()
        {
            if (setupOnAwake)
            {
                meshes = GetComponentsInChildren<MeshFilter>();
                calculateBounds();
            }
        }


        void Start()
        {
            cameralines = FindObjectOfType(typeof(DimBoxes.DrawLines)) as DimBoxes.DrawLines;

            if (!cameralines)
            {
                Debug.LogError("DimBoxes: no camera with DimBoxes.DrawLines in the scene", gameObject);
                return;
            }

            mcamera = cameralines.GetComponent<Camera>();
            previousPosition = transform.position;
            previousRotation = transform.rotation;
            startingBoundSize = bound.size;
            startingScale = transform.localScale;
            previousScale = startingScale;
            startingBoundCenterLocal = transform.InverseTransformPoint(bound.center);
            init();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (EditorApplication.isPlaying)
            {
                addText();
                return; 
            }
            init();
        }
#endif


        void OnDestroy()
        {
            if (hDimension) DestroyImmediate(hDimension);
            if (wDimension) DestroyImmediate(wDimension);
            if (dDimension) DestroyImmediate(dDimension);
        }



        public void init()
        {
            //if(hDimension) DestroyImmediate(hDimension);
            //if(wDimension) DestroyImmediate(wDimension);
            //if(dDimension) DestroyImmediate(dDimension);

            setPoints();
            setLines();
            if (cameralines) cameralines.setOutlines(lines, lineColor, triangles);
            
            if (!enabled)
            {
                hDimension.SetActive(false);
                wDimension.SetActive(false);
                dDimension.SetActive(false);
            }
        }


        void calculateBounds()
        {
            quat = transform.rotation;//object axis AABB

            BoxCollider coll = GetComponent<BoxCollider>();
            if (coll)
            {
                GameObject co = new GameObject("dummy");
                co.transform.position = transform.position;
                co.transform.localScale = transform.lossyScale;
                BoxCollider cobc = co.AddComponent<BoxCollider>();
                //quat = transform.rotation;
                cobc.center = coll.center;
                cobc.size = coll.size;
                colliderBound = cobc.bounds;
                DestroyImmediate(co);
                colliderBoundOffset = colliderBound.center - transform.position;
            }
                //return;

            /*isStatic = meshes[0].GetComponent<Renderer>().isPartOfStaticBatch;
            if (isStatic) quat = Quaternion.Euler(0f, 0f, 0f);//world axis
            
            if (isStatic)
            {
                meshBound = meshes[0].GetComponent<Renderer>().bounds;
                for (int i = 1; i < meshes.Length; i++)
                {
                    meshBound.Encapsulate(meshes[i].GetComponent<Renderer>().bounds);
                }
                return;
            }*/

            //if (Vector3.Magnitude(meshBound.size)>0.01f) return; //because of lenghty calculations - don't recalculate again
            meshBound = new Bounds();

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            for (int i = 0; i < meshes.Length; i++)
            {
                Mesh ms = meshes[i].sharedMesh;
                int vc = ms.vertexCount;
                for (int j = 0; j < vc; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        meshBound = new Bounds(meshes[i].transform.TransformPoint(ms.vertices[j]), Vector3.zero);
                    }
                    else
                    {
                        meshBound.Encapsulate(meshes[i].transform.TransformPoint(ms.vertices[j]));
                    }
                }
            }
            transform.rotation = quat;
            meshBoundOffset = meshBound.center - transform.position;
        }




        void setPoints()
        {
            if (colliderBased)
            {
                if (colliderBound == null)
                {
                    Debug.LogError("no collider - add collider to " + gameObject.name + " gameObject");
                    return;

                }
                bound = colliderBound;
                boundOffset = colliderBoundOffset;
            }

            else
            {
                bound = meshBound;
                boundOffset = meshBoundOffset;
            }
            //startingBoundCenterLocal = transform.InverseTransformPoint(bound.center);
            bound.size = new Vector3(bound.size.x * transform.localScale.x / startingScale.x, bound.size.y * transform.localScale.y / startingScale.y, bound.size.z * transform.localScale.z / startingScale.z);
            boundOffset = new Vector3(boundOffset.x * transform.localScale.x / startingScale.x, boundOffset.y * transform.localScale.y / startingScale.y, boundOffset.z * transform.localScale.z / startingScale.z);

            float orientationAngle = Z_up_orientation ? 90 : 0;


            topFrontRight = boundOffset + Vector3.Scale(bound.extents, Quaternion.AngleAxis(orientationAngle, Vector3.right) * new Vector3(1, 1, 1));
            topFrontLeft = boundOffset + Vector3.Scale(bound.extents, Quaternion.AngleAxis(orientationAngle, Vector3.right) * new Vector3(-1, 1, 1));
            topBackLeft = boundOffset + Vector3.Scale(bound.extents, Quaternion.AngleAxis(orientationAngle, Vector3.right) * new Vector3(-1, 1, -1));
            topBackRight = boundOffset + Vector3.Scale(bound.extents, Quaternion.AngleAxis(orientationAngle, Vector3.right) * new Vector3(1, 1, -1));
            bottomFrontRight = boundOffset + Vector3.Scale(bound.extents, Quaternion.AngleAxis(orientationAngle, Vector3.right) * new Vector3(1, -1, 1));
            bottomFrontLeft = boundOffset + Vector3.Scale(bound.extents, Quaternion.AngleAxis(orientationAngle, Vector3.right) * new Vector3(-1, -1, 1));
            bottomBackLeft = boundOffset + Vector3.Scale(bound.extents, Quaternion.AngleAxis(orientationAngle, Vector3.right) * new Vector3(-1, -1, -1));
            bottomBackRight = boundOffset + Vector3.Scale(bound.extents, Quaternion.AngleAxis(orientationAngle, Vector3.right) * new Vector3(1, -1, -1));

            //Quaternion.AngleAxis(90, transform.up) * transform.forward;



            corners = new Vector3[] { topFrontRight, topFrontLeft, topBackLeft, topBackRight, bottomFrontRight, bottomFrontLeft, bottomBackLeft, bottomBackRight };
            arrows = new Vector3[6, 3];

            Vector3 lineDirection = Vector3.zero;
            Vector3 extDirection = Vector3.zero;

            widthExt = new Vector3[4];
            switch (widthPlacement)
            {//top_front,bottom_front,top_back,bottom_back
                case w_placement.top_front:
                    widthExt[0] = corners[1];
                    widthExt[1] = corners[1] + (extensions.width ? extentionDist : 0) * (corners[1] - corners[0]).normalized;
                    widthExt[2] = corners[2] + (extensions.width ? extentionDist : 0) * (corners[2] - corners[3]).normalized;
                    widthExt[3] = corners[2];

                    extDirection = (corners[1] - corners[0]).normalized;

                    break;
                case w_placement.bottom_front:
                    widthExt[0] = corners[5];
                    widthExt[1] = corners[5] + (extensions.width ? extentionDist : 0) * (corners[5] - corners[4]).normalized;
                    widthExt[2] = corners[6] + (extensions.width ? extentionDist : 0) * (corners[6] - corners[7]).normalized;
                    widthExt[3] = corners[6];

                    extDirection = (corners[5] - corners[4]).normalized;

                    break;
                case w_placement.top_back:
                    widthExt[0] = corners[3];
                    widthExt[1] = corners[3] + (extensions.width ? extentionDist : 0) * (corners[3] - corners[2]).normalized;
                    widthExt[2] = corners[0] + (extensions.width ? extentionDist : 0) * (corners[0] - corners[1]).normalized;
                    widthExt[3] = corners[0];

                    extDirection = (corners[3] - corners[2]).normalized;

                    break;
                case w_placement.bottom_back:
                    widthExt[0] = corners[7];
                    widthExt[1] = corners[7] + (extensions.width ? extentionDist : 0) * (corners[7] - corners[6]).normalized;
                    widthExt[2] = corners[4] + (extensions.width ? extentionDist : 0) * (corners[4] - corners[5]).normalized;
                    widthExt[3] = corners[4];

                    extDirection = (corners[7] - corners[6]).normalized;

                    break;
                default:
                    break;
            }
            lineDirection = (widthExt[2] - widthExt[1]).normalized;
            arrows[0, 0] = widthExt[1];
            arrows[0, 1] = widthExt[1] + (lineDirection + extDirection * 0.3f) * arrowSize;
            arrows[0, 2] = widthExt[1] + (lineDirection - extDirection * 0.3f) * arrowSize;
            arrows[1, 0] = widthExt[2];
            arrows[1, 1] = widthExt[2] + (-lineDirection + extDirection * 0.3f) * arrowSize;
            arrows[1, 2] = widthExt[2] + (-lineDirection - extDirection * 0.3f) * arrowSize;


            depthExt = new Vector3[4];
            switch (depthPlacement)
            {//top_right=0,bottom_right=1,top_left=2,bottom_left=3
                case d_placement.top_right:
                    depthExt[0] = corners[2];
                    depthExt[1] = corners[2] + (extensions.depth ? extentionDist : 0) * (corners[2] - corners[1]).normalized;
                    depthExt[2] = corners[3] + (extensions.depth ? extentionDist : 0) * (corners[3] - corners[0]).normalized;
                    depthExt[3] = corners[3];
                    extDirection = (corners[2] - corners[1]).normalized;
                    break;
                case d_placement.bottom_right:
                    depthExt[0] = corners[6];
                    depthExt[1] = corners[6] + (extensions.depth ? extentionDist : 0) * (corners[6] - corners[5]).normalized;
                    depthExt[2] = corners[7] + (extensions.depth ? extentionDist : 0) * (corners[7] - corners[4]).normalized;
                    depthExt[3] = corners[7];
                    extDirection = (corners[6] - corners[5]).normalized;
                    break;
                case d_placement.top_left:
                    depthExt[0] = corners[0];
                    depthExt[1] = corners[0] + (extensions.depth ? extentionDist : 0) * (corners[0] - corners[3]).normalized;
                    depthExt[2] = corners[1] + (extensions.depth ? extentionDist : 0) * (corners[1] - corners[2]).normalized;
                    depthExt[3] = corners[1];
                    extDirection = (corners[0] - corners[3]).normalized;
                    break;
                case d_placement.bottom_left:
                    depthExt[0] = corners[4];
                    depthExt[1] = corners[4] + (extensions.depth ? extentionDist : 0) * (corners[4] - corners[7]).normalized;
                    depthExt[2] = corners[5] + (extensions.depth ? extentionDist : 0) * (corners[5] - corners[6]).normalized;
                    depthExt[3] = corners[5];
                    extDirection = (corners[4] - corners[7]).normalized;
                    break;
                default:
                    break;
            }
            lineDirection = (depthExt[2] - depthExt[1]).normalized;
            arrows[2, 0] = depthExt[1];
            arrows[2, 1] = depthExt[1] + (lineDirection + extDirection * 0.3f) * arrowSize;
            arrows[2, 2] = depthExt[1] + (lineDirection - extDirection * 0.3f) * arrowSize;
            arrows[3, 0] = depthExt[2];
            arrows[3, 1] = depthExt[2] + (-lineDirection + extDirection * 0.3f) * arrowSize;
            arrows[3, 2] = depthExt[2] + (-lineDirection - extDirection * 0.3f) * arrowSize;


            heightExt = new Vector3[4];
            switch (heightPlacement)
            {//front_right=0,front_left=1,back_right=2,back_left=3
                case h_placement.front_right:
                    heightExt[0] = corners[2];
                    heightExt[1] = corners[2] + (extensions.height ? extentionDist : 0) * (corners[2] - corners[3]).normalized;
                    heightExt[2] = corners[6] + (extensions.height ? extentionDist : 0) * (corners[6] - corners[7]).normalized;
                    heightExt[3] = corners[6];
                    extDirection = (corners[2] - corners[3]).normalized;
                    break;
                case h_placement.front_left:
                    heightExt[0] = corners[5];
                    heightExt[1] = corners[5] + (extensions.height ? extentionDist : 0) * (corners[5] - corners[4]).normalized;
                    heightExt[2] = corners[1] + (extensions.height ? extentionDist : 0) * (corners[1] - corners[0]).normalized;
                    heightExt[3] = corners[1];
                    extDirection = (corners[5] - corners[4]).normalized;
                    break;
                case h_placement.back_right:
                    heightExt[0] = corners[3];
                    heightExt[1] = corners[3] + (extensions.height ? extentionDist : 0) * (corners[3] - corners[0]).normalized;
                    heightExt[2] = corners[7] + (extensions.height ? extentionDist : 0) * (corners[7] - corners[4]).normalized;
                    heightExt[3] = corners[7];
                    extDirection = (corners[3] - corners[0]).normalized;
                    break;
                case h_placement.back_left:
                    heightExt[0] = corners[4];
                    heightExt[1] = corners[4] + (extensions.height ? extentionDist : 0) * (corners[4] - corners[5]).normalized;
                    heightExt[2] = corners[0] + (extensions.height ? extentionDist : 0) * (corners[0] - corners[1]).normalized;
                    heightExt[3] = corners[0];
                    extDirection = (corners[4] - corners[5]).normalized;
                    break;
                default:
                    break;
            }
            lineDirection = (heightExt[2] - heightExt[1]).normalized;
            arrows[4, 0] = heightExt[1];
            arrows[4, 1] = heightExt[1] + (lineDirection + extDirection * 0.3f) * arrowSize;
            arrows[4, 2] = heightExt[1] + (lineDirection - extDirection * 0.3f) * arrowSize;
            arrows[5, 0] = heightExt[2];
            arrows[5, 1] = heightExt[2] + (-lineDirection + extDirection * 0.3f) * arrowSize;
            arrows[5, 2] = heightExt[2] + (-lineDirection - extDirection * 0.3f) * arrowSize;

            addText();
        }




        void setLines()
        {
            Quaternion rot = transform.rotation;
            //if(isStatic) rot = Quaternion.identity;
            Vector3 pos = transform.position;
            //rot = Quaternion.identity;

            List<Vector3[]> _lines = new List<Vector3[]>();
            //int linesCount = 12;
            //linesCount += (extensions.depth ? 3:0) + (extensions.width ? 3:0) + (extensions.height ? 3:0);
            //int i1;
            //return;
            //lines = new Vector3[linesCount,2];
            Vector3[] _line;
            for (int i = 0; i < 4; i++)
            {
                //width
                _line = new Vector3[] { rot * corners[2 * i] + pos, rot * corners[2 * i + 1] + pos };
                _lines.Add(_line);
                //height
                _line = new Vector3[] { rot * corners[i] + pos, rot * corners[i + 4] + pos };
                _lines.Add(_line);
                //depth
                _line = new Vector3[] { rot * corners[2 * i] + pos, rot * corners[2 * i + 3 - 4 * (i % 2)] + pos };
                _lines.Add(_line);

                //i1 = i + 4;//heightlines
                //lines[i+4,0] = corners[i];
                //lines[i+4,1] = corners[i1];
                //bottom rectangle
                //lines[i+8,0] = corners[i1];
                //i1 = 4 + (i+1)%4;
                //lines[i+8,1] = corners[i1];
            }
            //int j = 12;
            if (extensions.width)
            {
                _lines.Add(new Vector3[] { rot * widthExt[0] + pos, rot * widthExt[1] + pos });
                _lines.Add(new Vector3[] { rot * widthExt[1] + pos, rot * widthExt[2] + pos });
                _lines.Add(new Vector3[] { rot * widthExt[2] + pos, rot * widthExt[3] + pos });
                //lines[12,0] = widthExt[0]; lines[12,1] = widthExt[1];
                //lines[13,0]= widthExt[1]; lines[13,1]= widthExt[2];
                //lines[14,0]= widthExt[2]; lines[14,1]= widthExt[3];
                //j += 3;
            }
            if (extensions.depth)
            {
                _lines.Add(new Vector3[] { rot * depthExt[0] + pos, rot * depthExt[1] + pos });
                _lines.Add(new Vector3[] { rot * depthExt[1] + pos, rot * depthExt[2] + pos });
                _lines.Add(new Vector3[] { rot * depthExt[2] + pos, rot * depthExt[3] + pos });
                //lines[j,0]= depthExt[0]; lines[j,1] = depthExt[1];
                //lines[j+1,0]= depthExt[1]; lines[j+1,1] = depthExt[2];
                //lines[j+2,0]= depthExt[2]; lines[j+2,1] = depthExt[3];
                //j += 3;
            }
            if (extensions.height)
            {
                _lines.Add(new Vector3[] { rot * heightExt[0] + pos, rot * heightExt[1] + pos });
                _lines.Add(new Vector3[] { rot * heightExt[1] + pos, rot * heightExt[2] + pos });
                _lines.Add(new Vector3[] { rot * heightExt[2] + pos, rot * heightExt[3] + pos });
            }
            lines = new Vector3[_lines.Count, 2];
            for (int j = 0; j < _lines.Count; j++)
            {
                lines[j, 0] = _lines[j][0];
                lines[j, 1] = _lines[j][1];
            }

            if (drawArrows)
            {
                triangles = new Vector3[arrows.GetLength(0), arrows.GetLength(1)];
                for (int i = 0; i < arrows.GetLength(0); i++)
                {
                    for (int j = 0; j < arrows.GetLength(1); j++)
                    {
                        triangles[i, j] = rot * arrows[i, j] + pos;
                    }
                }
            }
            else
            {
                triangles = new Vector3[0, 0];
            }
        }

        void addText()
        {
            if (heightExt == null) return;
            if (!hDimension)
            {
                hDimension = new GameObject("hDimension");  // the height Dimension 
                hDimensionMesh = new GameObject("hDimensionMesh");
                htm = hDimensionMesh.AddComponent<TextMesh>();
            }
            hDimension.transform.position = transform.position + transform.rotation * Vector3.Lerp(heightExt[1], heightExt[2], 0.5f);
            
            hDimension.transform.localRotation = Quaternion.Euler(Vector3.zero);
            
            hDimensionMesh.transform.position = transform.position + transform.rotation * Vector3.Lerp(heightExt[1], heightExt[2], 0.5f);
            htm.anchor = (TextAnchor)heightAlignment;
            htm.font = font;
            htm.GetComponent<Renderer>().material = letterMaterial;
            htm.text = (100 * Vector3.Distance(heightExt[1], heightExt[2])).ToString("#0.0") + " cm";
            hDimensionMesh.transform.parent = hDimension.transform;
            hDimension.transform.LookAt(transform.position + transform.rotation * heightExt[2], Z_up_orientation? transform.up:transform.forward);
            hDimension.transform.parent = transform;
            hDimensionMesh.transform.localRotation = Quaternion.Euler(new Vector3(90f, 90f, 0));

            //reset text scale when scaling
            hDimensionMesh.transform.SetParent(null);
            hDimensionMesh.transform.localScale = 0.02f * Vector3.one;
            hDimensionMesh.transform.parent = hDimension.transform;

            if (faceCamera.height)
            {
                hDimensionMesh.transform.localRotation = Quaternion.Euler(new Vector3(180f, 0, -90f));
            }


            if (!wDimension)
            {
                wDimension = new GameObject("wDimension");  // the width Dimension
                wDimensionMesh = new GameObject("wDimensionMesh");
                wtm = wDimensionMesh.AddComponent<TextMesh>();
            }
            wDimension.transform.position = transform.position + transform.rotation * Vector3.Lerp(widthExt[1], widthExt[2], 0.5f);
            wtm.anchor = (TextAnchor)widthAlignment;
            wtm.font = font;
            wtm.GetComponent<Renderer>().material = letterMaterial;
            wtm.text = (100 * Vector3.Distance(widthExt[1], widthExt[2])).ToString("#0.0") + " cm";
            wDimensionMesh.transform.parent = wDimension.transform;
            wDimensionMesh.transform.localPosition = Vector3.zero;

            wDimension.transform.LookAt(transform.position + transform.rotation * widthExt[2], Z_up_orientation ? transform.forward : transform.up);
            wDimension.transform.parent = transform;
            wDimensionMesh.transform.localRotation = Quaternion.Euler(new Vector3(90f, 90f, 0));

            //reset text scale when scaling
            wDimensionMesh.transform.SetParent(null);
            wDimensionMesh.transform.localScale = 0.02f * Vector3.one;
            wDimensionMesh.transform.parent = wDimension.transform;

            if (faceCamera.width)
            {
               wDimensionMesh.transform.localRotation = Quaternion.Euler(new Vector3(180f, 0, -90f));
            }

            if (!dDimension)
            {
                dDimension = new GameObject("dDimension");  // the depth Dimension
                dDimensionMesh = new GameObject("dDimensionMesh");
                dtm = dDimensionMesh.AddComponent<TextMesh>();
            }
            dDimension.transform.position = transform.position + transform.rotation * Vector3.Lerp(depthExt[1], depthExt[2], 0.5f);
            dtm.anchor = (TextAnchor)depthAlignment;
            dtm.font = font;
            dtm.GetComponent<Renderer>().material = letterMaterial;
            dtm.text = (100 * Vector3.Distance(depthExt[1], depthExt[2])).ToString("#0.0") + " cm";
            dDimensionMesh.transform.parent = dDimension.transform;
            dDimensionMesh.transform.localPosition = Vector3.zero;
            dDimension.transform.LookAt(transform.position + transform.rotation * depthExt[2], Z_up_orientation? -transform.right :transform.up);
            dDimension.transform.parent = transform;
            dDimensionMesh.transform.localRotation = Quaternion.Euler(new Vector3(90f, 90f, 0));

            //reset text scale when scaling
            dDimensionMesh.transform.parent = null;
            dDimensionMesh.transform.localScale = 0.02f * Vector3.one;
            dDimension.transform.localScale = Vector3.one;
            dDimensionMesh.transform.parent = dDimension.transform;

            if (faceCamera.depth)
            {
                dDimensionMesh.transform.localRotation = Quaternion.Euler(new Vector3(180f, 0, -90f));
            }
        }

        void LateUpdate()
        {
            if (transform.localScale != previousScale)
            {
                setPoints();
            }
            if (transform.position != previousPosition || transform.rotation != previousRotation || transform.localScale != previousScale)
            { 
                setLines();
                previousRotation = transform.rotation;
                previousPosition = transform.position;
                previousScale = transform.localScale;
            }
            cameralines.setOutlines(lines, lineColor, triangles);

            Vector3 campos = mcamera.transform.position;

            if (faceCamera.height)
            {
                hDimension.transform.LookAt(campos, Z_up_orientation? transform.forward : transform.up);
                //prevent text upside down
                if(mcamera.transform.InverseTransformDirection(hDimension.transform.up).x<0) hDimension.transform.Rotate(0,0,180f);
            }
            if (faceCamera.width)
            {
                wDimension.transform.LookAt(campos, Z_up_orientation ? transform.up : transform.forward);
                if (mcamera.transform.InverseTransformDirection(wDimension.transform.up).x < 0) wDimension.transform.Rotate(0, 0, 180f);
            }
            if (faceCamera.depth)
            {
                dDimension.transform.LookAt(campos, transform.right);
                if (mcamera.transform.InverseTransformDirection(dDimension.transform.up).x < 0) dDimension.transform.Rotate(0, 0, 180f);
            }
        }


        void OnDrawGizmos()
        {
            Gizmos.color = lineColor;
            for (int i = 0; i < lines.GetLength(0); i++)
            {
                Gizmos.DrawLine(lines[i, 0], lines[i, 1]);
            }
            Gizmos.color = Color.white;
            //Gizmos.DrawLine(wDimension.transform.position, wDimension.transform.position + transform.forward);
            //Gizmos.DrawLine(dDimension.transform.position, dDimension.transform.position + transform.right);
            //Gizmos.matrix = wDimension.transform.localToWorldMatrix;
            //Gizmos.DrawWireCube(Vector3.zero,0.25f*Vector3.one);

        }

        void OnMouseDown()
        {
            if (permanent) return;
            enabled = !enabled;
            hDimension.SetActive(enabled);
            dDimension.SetActive(enabled);
            wDimension.SetActive(enabled);
        }

    }
}
