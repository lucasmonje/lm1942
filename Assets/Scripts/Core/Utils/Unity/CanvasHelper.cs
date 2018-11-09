using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Models;
using Core.Domain.Utils;
using Core.Utils.Extensions;
using UniRx;
using UnityEngine;

namespace Core.Utils.Unity
{
    public static class CanvasHelper
    {
        private static Canvas canvas;
        private static RectTransform canvasRectTransform;
        private static Vector2 tileSize;
        
        private static readonly List<Position> outSides;

        private static readonly Position[] Left =
        {
            Position.A2, Position.A3, Position.A4, Position.A5, Position.A6, Position.A7, Position.A8
        };

        private static readonly Position[] Right =
        {
            Position.I2, Position.I3, Position.I4, Position.I5, Position.I6, Position.I7, Position.I8
        };

        private static readonly Position[] Top =
        {
            Position.A1, Position.B1, Position.C1, Position.D1, Position.E1, Position.F1, Position.G1, Position.H1,
            Position.I1
        };

        private static readonly Position[] Bottom =
        {
            Position.A9, Position.B9, Position.C9, Position.D9, Position.E9, Position.F9, Position.G9, Position.H9,
            Position.I9
        };

        static CanvasHelper()
        {
            outSides = new List<Position>();
            outSides.AddRange(Left);
            outSides.AddRange(Right);
            outSides.AddRange(Top);
            outSides.AddRange(Bottom);
        }

        public static bool IsLeft(Position position)
        {
            return Left.Contains(position);
        }
        
        public static bool IsRight(Position position)
        {
            return Right.Contains(position);
        }
        
        public static bool IsTop(Position position)
        {
            return Top.Contains(position);
        }
        
        public static bool IsBottom(Position position)
        {
            return Bottom.Contains(position);
        }

        public static Vector2 GetTileSize()
        {
            return tileSize;
        }

        public static void SetCanvas(Canvas value)
        {
            canvas = value;
            canvasRectTransform = canvas.GetComponent<RectTransform>();
            var rect = canvasRectTransform.rect;
            UpdateTileSize(rect);
        }

        private static void UpdateTileSize(Rect rect)
        {
            var a = A(rect);
            var b = B(rect);
            var row1 = Row1(rect);
            var row2 = Row2(rect);
            tileSize = new Vector2(Math.Abs(a - b), Math.Abs(row1 - row2));
        }

        public static Vector2 GetCanvasPosition(Position position, RectTransform rectTransform)
        {
            var rect = canvasRectTransform.rect;
            UpdateTileSize(rect);
            
            switch (position)
            {
                case Position.Up:
                    return new Vector2(rectTransform.anchoredPosition.x, rect.yMax);
                case Position.Down:
                    return new Vector2(rectTransform.anchoredPosition.x, rect.yMin);
                case Position.Left:
                    return new Vector2(rect.xMin, rectTransform.anchoredPosition.y);
                case Position.Right:
                    return new Vector2(rect.xMax, rectTransform.anchoredPosition.y);
                default:
                    return GetCanvasPosition(position);
            }
        }

        public static Vector2 GetCanvasPosition(Position position)
        {
            var rect = canvasRectTransform.rect;

            switch (position)
            {
                case Position.A1:
                    return new Vector2(A(rect), Row1(rect));
                case Position.B1:
                    return new Vector2(B(rect), Row1(rect));
                case Position.C1:
                    return new Vector2(C(rect), Row1(rect));
                case Position.D1:
                    return new Vector2(D(rect), Row1(rect));
                case Position.E1:
                    return new Vector2(E(rect), Row1(rect));
                case Position.F1:
                    return new Vector2(F(rect), Row1(rect));
                case Position.G1:
                    return new Vector2(G(rect), Row1(rect));
                case Position.H1:
                    return new Vector2(H(rect), Row1(rect));
                case Position.I1:
                    return new Vector2(I(rect), Row1(rect));

                case Position.A2:
                    return new Vector2(A(rect), Row2(rect));
                case Position.B2:
                    return new Vector2(B(rect), Row2(rect));
                case Position.C2:
                    return new Vector2(C(rect), Row2(rect));
                case Position.D2:
                    return new Vector2(D(rect), Row2(rect));
                case Position.E2:
                    return new Vector2(E(rect), Row2(rect));
                case Position.F2:
                    return new Vector2(F(rect), Row2(rect));
                case Position.G2:
                    return new Vector2(G(rect), Row2(rect));
                case Position.H2:
                    return new Vector2(H(rect), Row2(rect));
                case Position.I2:
                    return new Vector2(I(rect), Row2(rect));

                case Position.A3:
                    return new Vector2(A(rect), Row3(rect));
                case Position.B3:
                    return new Vector2(B(rect), Row3(rect));
                case Position.C3:
                    return new Vector2(C(rect), Row3(rect));
                case Position.D3:
                    return new Vector2(D(rect), Row3(rect));
                case Position.E3:
                    return new Vector2(E(rect), Row3(rect));
                case Position.F3:
                    return new Vector2(F(rect), Row3(rect));
                case Position.G3:
                    return new Vector2(G(rect), Row3(rect));
                case Position.H3:
                    return new Vector2(H(rect), Row3(rect));
                case Position.I3:
                    return new Vector2(I(rect), Row3(rect));

                case Position.A4:
                    return new Vector2(A(rect), Row4(rect));
                case Position.B4:
                    return new Vector2(B(rect), Row4(rect));
                case Position.C4:
                    return new Vector2(C(rect), Row4(rect));
                case Position.D4:
                    return new Vector2(D(rect), Row4(rect));
                case Position.E4:
                    return new Vector2(E(rect), Row4(rect));
                case Position.F4:
                    return new Vector2(F(rect), Row4(rect));
                case Position.G4:
                    return new Vector2(G(rect), Row4(rect));
                case Position.H4:
                    return new Vector2(H(rect), Row4(rect));
                case Position.I4:
                    return new Vector2(I(rect), Row4(rect));

                case Position.A5:
                    return new Vector2(A(rect), Row5(rect));
                case Position.B5:
                    return new Vector2(B(rect), Row5(rect));
                case Position.C5:
                    return new Vector2(C(rect), Row5(rect));
                case Position.D5:
                    return new Vector2(D(rect), Row5(rect));
                case Position.E5:
                    return new Vector2(E(rect), Row5(rect));
                case Position.F5:
                    return new Vector2(F(rect), Row5(rect));
                case Position.G5:
                    return new Vector2(G(rect), Row5(rect));
                case Position.H5:
                    return new Vector2(H(rect), Row5(rect));
                case Position.I5:
                    return new Vector2(I(rect), Row5(rect));

                case Position.A6:
                    return new Vector2(A(rect), Row6(rect));
                case Position.B6:
                    return new Vector2(B(rect), Row6(rect));
                case Position.C6:
                    return new Vector2(C(rect), Row6(rect));
                case Position.D6:
                    return new Vector2(D(rect), Row6(rect));
                case Position.E6:
                    return new Vector2(E(rect), Row6(rect));
                case Position.F6:
                    return new Vector2(F(rect), Row6(rect));
                case Position.G6:
                    return new Vector2(G(rect), Row6(rect));
                case Position.H6:
                    return new Vector2(H(rect), Row6(rect));
                case Position.I6:
                    return new Vector2(I(rect), Row6(rect));

                case Position.A7:
                    return new Vector2(A(rect), Row7(rect));
                case Position.B7:
                    return new Vector2(B(rect), Row7(rect));
                case Position.C7:
                    return new Vector2(C(rect), Row7(rect));
                case Position.D7:
                    return new Vector2(D(rect), Row7(rect));
                case Position.E7:
                    return new Vector2(E(rect), Row7(rect));
                case Position.F7:
                    return new Vector2(F(rect), Row7(rect));
                case Position.G7:
                    return new Vector2(G(rect), Row7(rect));
                case Position.H7:
                    return new Vector2(H(rect), Row7(rect));
                case Position.I7:
                    return new Vector2(I(rect), Row7(rect));

                case Position.A8:
                    return new Vector2(A(rect), Row8(rect));
                case Position.B8:
                    return new Vector2(B(rect), Row8(rect));
                case Position.C8:
                    return new Vector2(C(rect), Row8(rect));
                case Position.D8:
                    return new Vector2(D(rect), Row8(rect));
                case Position.E8:
                    return new Vector2(E(rect), Row8(rect));
                case Position.F8:
                    return new Vector2(F(rect), Row8(rect));
                case Position.G8:
                    return new Vector2(G(rect), Row8(rect));
                case Position.H8:
                    return new Vector2(H(rect), Row8(rect));
                case Position.I8:
                    return new Vector2(I(rect), Row8(rect));

                case Position.A9:
                    return new Vector2(A(rect), Row9(rect));
                case Position.B9:
                    return new Vector2(B(rect), Row9(rect));
                case Position.C9:
                    return new Vector2(C(rect), Row9(rect));
                case Position.D9:
                    return new Vector2(D(rect), Row9(rect));
                case Position.E9:
                    return new Vector2(E(rect), Row9(rect));
                case Position.F9:
                    return new Vector2(F(rect), Row9(rect));
                case Position.G9:
                    return new Vector2(G(rect), Row9(rect));
                case Position.H9:
                    return new Vector2(H(rect), Row9(rect));
                case Position.I9:
                    return new Vector2(I(rect), Row9(rect));
            }

            return Vector2.zero;
        }


        private static float Row1(Rect rect)
        {
            return rect.yMax;
        }

        private static float Row2(Rect rect)
        {
            return rect.yMax * 0.25f;
        }

        private static float Row3(Rect rect)
        {
            return rect.yMax * 0.5f;
        }

        private static float Row4(Rect rect)
        {
            return rect.yMax * 0.75f;
        }

        private static float Row5(Rect rect)
        {
            return rect.yMin + rect.yMax;
        }

        private static float Row6(Rect rect)
        {
            return rect.yMin * 0.25f;
        }

        private static float Row7(Rect rect)
        {
            return rect.yMin * 0.5f;
        }

        private static float Row8(Rect rect)
        {
            return rect.yMin * 0.75f;
        }

        private static float Row9(Rect rect)
        {
            return rect.yMin;
        }

        private static float A(Rect rect)
        {
            return rect.xMin;
        }

        private static float B(Rect rect)
        {
            return rect.xMin * 0.75f;
        }

        private static float C(Rect rect)
        {
            return rect.xMin * 0.5f;
        }

        private static float D(Rect rect)
        {
            return rect.xMin * 0.25f;
        }

        private static float E(Rect rect)
        {
            return rect.xMin + rect.xMax;
        }

        private static float F(Rect rect)
        {
            return rect.xMax * 0.25f;
        }

        private static float G(Rect rect)
        {
            return rect.xMax * 0.5f;
        }

        private static float H(Rect rect)
        {
            return rect.xMax * 0.75f;
        }

        private static float I(Rect rect)
        {
            return rect.xMax;
        }

        public static Vector3 InverseTransformPoint(Vector3 rectTransformPosition)
        {
            return canvasRectTransform.InverseTransformPoint(rectTransformPosition);
        }

        public static PlayerPoints GetPlayerPoints(Vector3 pos, int quantity)
        {
            var distance = float.MaxValue;
            var result = Tuple.Create(Position.A1, Vector2.zero);
            var points = new List<Point>();
            
            
            foreach (var position in outSides)
            {
                var canvasPosition = GetCanvasPosition(position);
                points.Add(new Point(position, canvasPosition, Vector2.Distance(pos, canvasPosition)));
            }
            points.Sort((a, b) => (int)(a.DistanceBetweenPlayer - b.DistanceBetweenPlayer));
            points.RemoveRange(quantity, points.Count  - quantity);

            for (var index = 0; index < points.Count; index++)
            {
                var point = points[index];
                point.Weight = quantity - index;
            }

            return new PlayerPoints(points);
        }
    }
}