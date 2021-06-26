using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicButtons {

    public enum RoundedCornerType { ALL, LTRB }

    public class RoundedRectangleGraphic : MaskableGraphic {

        private int cornerPointsCount = 20;

        [SerializeField]
        private RoundedCornerType cornersType = RoundedCornerType.ALL;
        [SerializeField]
        private float cornerRadius = 8;
        [SerializeField]
        private float cornerRadiusTL = 8;
        [SerializeField]
        private float cornerRadiusTR = 8;
        [SerializeField]
        private float cornerRadiusBR = 8;
        [SerializeField]
        private float cornerRadiusBL = 8;
        [SerializeField]
        private float borderWidth = 0;
        [SerializeField]
        private Color borderColor = new Color (0, 0, 0, 1);

        public RoundedCornerType CornersType {
            get { return cornersType; }
            set {
                cornersType = value;
                SetAllDirty ();
            }
        }

        public float CornerRadius {
            get { return cornerRadius; }
            set {
                cornerRadius = value;
                SetAllDirty ();
            }
        }

        public float CornerRadiusTL {
            get { return cornerRadiusTL; }
            set {
                cornerRadiusTL = value;
                SetAllDirty ();
            }
        }

        public float CornerRadiusTR {
            get { return cornerRadiusTR; }
            set {
                cornerRadiusTR = value;
                SetAllDirty ();
            }
        }

        public float CornerRadiusBR {
            get { return cornerRadiusBR; }
            set {
                cornerRadiusBR = value;
                SetAllDirty ();
            }
        }

        public float CornerRadiusBL {
            get { return cornerRadiusBL; }
            set {
                cornerRadiusBL = value;
                SetAllDirty ();
            }
        }

        public float BorderWidth {
            get { return borderWidth; }
            set {
                borderWidth = value;
                SetAllDirty ();
            }
        }

        public Color BorderColor {
            get { return borderColor; }
            set {
                borderColor = value;
                SetAllDirty ();
            }
        }

        private bool hasBorder () {
            return BorderWidth > 0;
        }

        private float ClampCornerRadius (float value, Vector2 rectangleSize) {
            return Mathf.Clamp (value, 0, Mathf.Min (rectangleSize.x, rectangleSize.y) / 2);
        }

        private float GetCornerRadiusTL (Vector2 rectangleSize) {
            var radius = CornersType == RoundedCornerType.ALL ? CornerRadius : CornerRadiusTL;
            return ClampCornerRadius (radius, rectangleSize);
        }

        private float GetCornerRadiusTR (Vector2 rectangleSize) {
            var radius = CornersType == RoundedCornerType.ALL ? CornerRadius : CornerRadiusTR;
            return ClampCornerRadius (radius, rectangleSize);
        }

        private float GetCornerRadiusBR (Vector2 rectangleSize) {
            var radius = CornersType == RoundedCornerType.ALL ? CornerRadius : CornerRadiusBR;
            return ClampCornerRadius (radius, rectangleSize);
        }

        private float GetCornerRadiusBL (Vector2 rectangleSize) {
            var radius = CornersType == RoundedCornerType.ALL ? CornerRadius : CornerRadiusBL;
            return ClampCornerRadius (radius, rectangleSize);
        }

        protected override void OnPopulateMesh (VertexHelper vh) {
            vh.Clear ();

            RectTransform rt = gameObject.GetComponent<RectTransform> ();
            Vector2 size = new Vector2 (rt.rect.width, rt.rect.height);

            float radiusTL = GetCornerRadiusTL (size);
            float radiusTR = GetCornerRadiusTR (size);
            float radiusBR = GetCornerRadiusBR (size);
            float radiusBL = GetCornerRadiusBL (size);

            List<UIVertex> vertexList = new List<UIVertex> ();
            List<int> triangleIndexesList = new List<int> ();

            List<Vector2> rectangleVertices = new List<Vector2> ();
            rectangleVertices.Add (Vector2.zero);
            rectangleVertices.AddRange (CreateRectangleVertices (
                new Vector2 (size.x - 2 * BorderWidth, size.y - 2 * BorderWidth),
                Mathf.Max (0, radiusTL - BorderWidth),
                Mathf.Max (0, radiusTR - BorderWidth),
                Mathf.Max (0, radiusBR - BorderWidth),
                Mathf.Max (0, radiusBL - BorderWidth),
                cornerPointsCount));

            vertexList.AddRange (CreateUIVertexList (rectangleVertices.ToArray (), size, color));
            triangleIndexesList.AddRange (CreateTriangleIndexesForRectangle (rectangleVertices.Count, 0));

            if (hasBorder ()) {

                List<Vector2> borderRectangleVertices = new List<Vector2> ();

                borderRectangleVertices.AddRange (CreateRectangleVertices (
                    new Vector2 (size.x - 2 * BorderWidth, size.y - 2 * BorderWidth),
                    Mathf.Max (0, radiusTL - BorderWidth),
                    Mathf.Max (0, radiusTR - BorderWidth),
                    Mathf.Max (0, radiusBR - BorderWidth),
                    Mathf.Max (0, radiusBL - BorderWidth),
                    cornerPointsCount));

                borderRectangleVertices.AddRange (CreateRectangleVertices (
                    size,
                    radiusTL,
                    radiusTR,
                    radiusBR,
                    radiusBL,
                    cornerPointsCount));

                vertexList.AddRange (CreateUIVertexList (borderRectangleVertices.ToArray (), null, BorderColor));
                var borderIndexes = CreateTriangleIndexesForRectangleBorder (borderRectangleVertices.Count / 2, rectangleVertices.Count);
                borderIndexes.AddRange (triangleIndexesList);
                triangleIndexesList = borderIndexes;
            }

            vh.AddUIVertexStream (vertexList, triangleIndexesList);
        }

        private Vector2[] CreateRectangleVertices (
            Vector2 size,
            float cornerRadiusTL,
            float cornerRadiusTR,
            float cornerRadiusBR,
            float cornerRadiusBL,
            int cornerPointsCount
        ) {
            Vector2 tl = new Vector2 (size.x / 2 - cornerRadiusTL, size.y / 2 - cornerRadiusTL);
            Vector2 tr = new Vector2 (size.x / 2 - cornerRadiusTR, size.y / 2 - cornerRadiusTR);
            Vector2 br = new Vector2 (size.x / 2 - cornerRadiusBR, size.y / 2 - cornerRadiusBR);
            Vector2 bl = new Vector2 (size.x / 2 - cornerRadiusBL, size.y / 2 - cornerRadiusBL);

            int rectangleVerticesCount = cornerPointsCount * 4;
            int totalVerticesCount = rectangleVerticesCount;

            Vector2[] cornerPoints = CalculateCornerPoints (cornerPointsCount);
            Vector2[] vertices = new Vector2[totalVerticesCount];

            for (int i = 0; i < cornerPointsCount; i++) {
                Vector2 cornerPoint = cornerPoints[i];
                vertices[i] = new Vector2 (tr.x + (cornerRadiusTR * cornerPoint.x), tr.y + (cornerRadiusTR * cornerPoint.y));
                vertices[i + cornerPointsCount] = new Vector2 (br.x + (cornerRadiusBR * cornerPoint.y), (br.y + (cornerRadiusBR * cornerPoint.x)) * -1f);
                vertices[i + (cornerPointsCount * 2)] = new Vector2 ((bl.x + (cornerRadiusBL * cornerPoint.x)) * -1, (bl.y + (cornerRadiusBL * cornerPoint.y)) * -1f);
                vertices[i + (cornerPointsCount * 3)] = new Vector2 ((tl.x + (cornerRadiusTL * cornerPoint.y)) * -1, tl.y + (cornerRadiusTL * cornerPoint.x));
            }

            return vertices;
        }

        private Vector2[] CalculateCornerPoints (int pointsCount) {
            Vector2[] result = new Vector2[pointsCount];
            float step = 90f / (pointsCount - 1);
            for (int i = 0; i < pointsCount; i++) {
                float x = Mathf.Sin (Mathf.Deg2Rad * i * step);
                float y = Mathf.Cos (Mathf.Deg2Rad * i * step);
                result[i] = new Vector2 (x, y);
            }
            return result;
        }

        private List<int> CreateTriangleIndexesForRectangle (int verticesCount, int vertextStartingIndex) {
            var triangleIndexes = new List<int> ();
            for (int i = 0; i < verticesCount - 2; i++) {
                triangleIndexes.Add (vertextStartingIndex);
                triangleIndexes.Add (vertextStartingIndex + i + 1);
                triangleIndexes.Add (vertextStartingIndex + i + 2);
            }

            triangleIndexes.Add (vertextStartingIndex);
            triangleIndexes.Add (vertextStartingIndex + verticesCount - 1);
            triangleIndexes.Add (vertextStartingIndex + 1);

            return triangleIndexes;
        }

        private List<int> CreateTriangleIndexesForRectangleBorder (int verticesCount, int vertextStartingIndex) {
            var triangleIndexes = new List<int> ();
            for (int i = 0; i < verticesCount - 1; i++) {
                triangleIndexes.Add (vertextStartingIndex + i);
                triangleIndexes.Add (vertextStartingIndex + verticesCount + i);
                triangleIndexes.Add (vertextStartingIndex + verticesCount + i + 1);

                triangleIndexes.Add (vertextStartingIndex + i);
                triangleIndexes.Add (vertextStartingIndex + verticesCount + i + 1);
                triangleIndexes.Add (vertextStartingIndex + i + 1);
            }

            triangleIndexes.Add (verticesCount + vertextStartingIndex - 1);
            triangleIndexes.Add (verticesCount * 2 + vertextStartingIndex - 1);
            triangleIndexes.Add (verticesCount + vertextStartingIndex);

            triangleIndexes.Add (verticesCount + vertextStartingIndex - 1);
            triangleIndexes.Add (verticesCount + vertextStartingIndex);
            triangleIndexes.Add (vertextStartingIndex);

            return triangleIndexes;
        }

        private List<UIVertex> CreateUIVertexList (Vector2[] positions, Nullable<Vector2> size, Color color) {
            var result = new List<UIVertex> ();
            for (int i = 0; i < positions.Length; i++) {
                Vector2 position = positions[i];
                Vector2 uv = size != null ?
                    new Vector2 (size.Value.x / (position.x + size.Value.x / 2), size.Value.y / (position.y + size.Value.x / 2)) :
                    Vector2.zero;
                result.Add (CreateUIVertex (position, uv, color));
            }
            return result;
        }

        private UIVertex CreateUIVertex (Vector2 position, Vector2 uv0, Color color) {
            var vert = UIVertex.simpleVert;
            vert.color = color;
            vert.position = position;
            vert.uv0 = uv0;

            return vert;
        }
    }
}