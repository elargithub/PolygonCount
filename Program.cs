using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace PolygonCount
{
    enum Quadrilateral
    {
        Parallelogram,
        HorizontalRectangle,
        DiagonalalRectangle,
        Square,
        HorizontalSquare,
        DiagonalalSquare
    }
    enum TestCase
    {
        HorizontalLine,
        VercitalLine,
        DiagonalLine,
        OneTriangle,
        TwoTriangle,
        LineAndTriangle,
        OneDiagonalParallelogram,
        OneHorizontalRectangle,
        TwoHorizontalRectangle,
        OneDiagonalRectangle,
        OneHorizontalSquare,
        OneDiagonalSquare,
        TwoDiagonalSquare,
        ThreeHorizontalRectangle
    }
    class Program
    {
        static Random rand = new Random();
        static List<Point> points;
        static int maxXCoord = 9 + 0;
        static int maxYCoord = 5;
        static List<List<StringBuilder>>[] ListOfSbLists;
        static List<Polygon>[] ListOfPolygons;

        static Dictionary<Quadrilateral, Func<int, int, int, int, bool>> isQdrltrlType;

        static void Main(string[] args)
        {
            isQdrltrlType = new Dictionary<Quadrilateral, Func<int, int, int, int, bool>>()
                                    {
                                    // {Quadrilateral.Parallelogram, IsParallelogram},
                                    // {Quadrilateral.HorizontalRectangle, IsHorizontalRectangle},
                                    // {Quadrilateral.DiagonalalRectangle, IsDiagonalalRectangle},

                                     // {Quadrilateral.Square, (DxT, DyT, DxL, DyL) => AreSidesEqual(DxT, DyT, DxL, DyL) && AreSidesPerpendicular(DxT, DyT, DxL, DyL)}
                                        {Quadrilateral.Square, IsSquare}
                                    // {Quadrilateral.Square, IsSquare}
                                    // {Quadrilateral.HorizontalSquare, IsHorizontalSquare},
                                    // {Quadrilateral.DiagonalalSquare, IsDiagonalalSquare},
                                    };


            int quadrilateralTypesCount = Enum.GetNames(typeof(Quadrilateral)).Length;
            ListOfPolygons = new List<Polygon>[quadrilateralTypesCount];
            ListOfSbLists = new List<List<StringBuilder>>[quadrilateralTypesCount];
            for (int type = 0; type < quadrilateralTypesCount; type++)
            {
                ListOfPolygons[type] = new List<Polygon>();
                ListOfSbLists[type] = new List<List<StringBuilder>>();
            }

            int pointsCount = 4 + 14;//rand.Next(25 - 4);
            int[] quadrilateralsCount = new int[quadrilateralTypesCount];

            InitPoints(pointsCount);
            //InitTestPoints(4); pointsCount = points.Count();

            // InitTestPoints(TestCase.HorizontalLine); pointsCount = points.Count();
            // InitTestPoints(TestCase.VercitalLine); pointsCount = points.Count();
            // InitTestPoints(TestCase.DiagonalLine); pointsCount = points.Count();
            // InitTestPoints(TestCase.OneTriangle); pointsCount = points.Count();
            // InitTestPoints(TestCase.TwoTriangle); pointsCount = points.Count();
            // InitTestPoints(TestCase.LineAndTriangle); pointsCount = points.Count();
            // InitTestPoints(TestCase.OneHorizontalRectangle); pointsCount = points.Count();
            // InitTestPoints(TestCase.OneDiagonalParallelogram); pointsCount = points.Count();
            // InitTestPoints(TestCase.TwoHorizontalRectangle); pointsCount = points.Count();
            // InitTestPoints(TestCase.OneDiagonalRectangle); pointsCount = points.Count();
            // InitTestPoints(TestCase.OneHorizontSquare); pointsCount = points.Count();
            // InitTestPoints(TestCase.OneDiagonalSquare); pointsCount = points.Count();
            // InitTestPoints(TestCase.TwoDiagonalSquare); pointsCount = points.Count();
            // InitTestPoints(TestCase.ThreeHorizontalRectangle); pointsCount = points.Count();

            // Plot initial points
            PlotPointsToConsole();

            // Finding all rects Algo
            for (int i = 0; i < pointsCount - 3; i++)
            {
                Point t1 = points[i];// top 1
                for (int j = i + 1; j < pointsCount - 1; j++)
                {
                    Point t2 = points[j];// top 2

                    // top side coord diff
                    int DxT = t2.X - t1.X;
                    int DyT = t2.Y - t1.Y;

                    for (int k = j + 1; k < pointsCount; k++)
                    {
                        Point b1 = points[k];// bottom 1

                        // left side coord diff 
                        int DxL = b1.X - t2.X;
                        int DyL = b1.Y - t2.Y;

                        // check that whether top and left side on the same line
                        if ((DyL != 0 && DyT != 0 && DxL / (double)DyL == DxT / (double)DyT) ||
                            (DxL != 0 && DxT != 0 && DyL / (double)DxL == DyT / (double)DxT))
                            continue;

                        for (int l = j + 1; l < pointsCount; l++)
                        {
                            Point b2 = points[l];// bottom 2
                            // bottom side coord diff  
                            int DxB = b1.X - b2.X;
                            int DyB = b1.Y - b2.Y;
                            // right side coord diff 
                            int DxR = b2.X - t1.X;
                            int DyR = b2.Y - t1.Y;

                            // check for paralellism T-B, L-R   
                            if (DxB == DxT && DyB == DyT && DxR == DxL && DyR == DyL)
                            {
                                for (int type = 0; type < quadrilateralTypesCount; type++)
                                {
                                    if (IsType(type, DxT, DyT, DxL, DyL) || ((Quadrilateral)type == Quadrilateral.Square && isQdrltrlType[(Quadrilateral)type](DxT, DyT, DxL, DyL)))
                                    {
                                        if (quadrilateralsCount[type] % 17 == 0)
                                            InitNextListStringBuilder(ListOfSbLists[type]);

                                        int indx = quadrilateralsCount[type] / 17;
                                        ListOfSbLists[type][indx] = PlotRectsToSbList(new List<Point> { t2, t1, b1, b2 }, ListOfSbLists[type][indx], true);
                                        ListOfPolygons[type].Add(new Polygon((Quadrilateral)type, new Point[] { t2, t1, b1, b2 }));
                                        quadrilateralsCount[type]++;
                                    }
                                }

                                break;
                            }
                        }
                    }
                }
            }

            // Plot res as assembled StringBuilders
            for (int type = 0; type < quadrilateralTypesCount; type++)
            {
                PlotResults(quadrilateralsCount[type], ListOfSbLists[type], ((Quadrilateral)type).ToString());
            }

            // Plot res as polygon list
            foreach (var polygList in ListOfPolygons)
            {
                if (polygList.Count > 0)
                {
                    WriteHeader(polygList.Count, polygList[0].Name);
                    // Console.WriteLine(polygList[0].Name + "s : " + polygList.Count + "\n");
                    foreach (var polyg in polygList)
                    {
                        List<StringBuilder> sbList = new List<StringBuilder>();
                        InitSbList(sbList);
                        // Console.WriteLine("\n" + polyg.ToString());
                        sbList = PlotRectsToSbList(polyg.Verteces.ToList(), sbList, true);
                        foreach (var sb in sbList)
                            Console.WriteLine(sb);
                        Console.WriteLine();

                    }
                }
            }
        }

        // Initialize list of distinct descending points 
        private static void InitPoints(int pointsCount)
        {
            points = new List<Point>();
            for (int i = 0; i < pointsCount; i++)
            {
                Point newP;
                do
                {
                    newP = new Point(rand.Next(maxXCoord + 1), rand.Next(maxYCoord + 1));
                } while (IsDuplicate(newP));

                points.Add(newP);
            }
            points = points.OrderByDescending(p => p.Y).ThenByDescending(p => p.X).ToList();

            //C# 7.0, local function
            bool IsDuplicate(Point newP)
            {
                foreach (var item in points)
                {
                    if (item == newP)
                        return true;
                }
                return false;
            }
        }
        private static void InitNextListStringBuilder(List<List<StringBuilder>> listOfSbLists)
        {
            List<StringBuilder> sbList = new List<StringBuilder>();
            // for (int i = 0; i < 5 + maxYCoord; i++)
            // {
            //     sbList.Add(new StringBuilder(" "));
            // }
            InitSbList(sbList);
            listOfSbLists.Add(sbList);
        }
        private static void InitSbList(List<StringBuilder> sbList)
        {
            for (int i = 0; i < 5 + maxYCoord; i++)
                sbList.Add(new StringBuilder(" "));
        }
        private static void PlotPointsToConsole()
        {
            Console.WriteLine();

            int prevY = points[0].Y;
            List<Point> pointsWithSameY = new List<Point>();
            foreach (var p in points)
            {
                if (p.Y != prevY)
                {
                    WritePointRow(prevY, pointsWithSameY);
                    pointsWithSameY.Clear();
                    prevY = p.Y;
                }
                pointsWithSameY.Add(p);
            }

            WritePointRow(prevY, pointsWithSameY);

            Console.WriteLine();

            PlotPoints(points);

            Console.WriteLine();
        }
        private static void WritePointRow(int prevY, List<Point> pointsWithSameY)
        {
            pointsWithSameY.Reverse();
            StringBuilder pRow = new StringBuilder(prevY + " : ");
            foreach (var pWithSameY in pointsWithSameY)
            {
                pRow.Append(pWithSameY.X + "; ");
            }
            Console.WriteLine(pRow);
        }
        private static void PlotPoints(List<Point> ps)
        {
            string borderStr = GetRowStr('-');
            Console.WriteLine(borderStr);
            int j = 0;
            for (int y = maxYCoord; y >= 0; y--)
            {
                StringBuilder rowStrB = new StringBuilder(GetRowStr(' '));

                GetRowStringBuilder(ref j, y, ps, rowStrB);
                Console.WriteLine(rowStrB);
            }
            Console.WriteLine(borderStr);
            Console.WriteLine();
        }
        private static List<StringBuilder> PlotRectsToSbList(List<Point> ps, List<StringBuilder> sbList, bool IsRotd)
        {
            Point t2 = ps[0];// top-left
            Point t1 = ps[1];// top-right
            Point b1 = ps[2];// bottom-left
            Point b2 = ps[3];// bottom-right

            sbList[0].Append(" (" + t2.X + "," + t2.Y + ")" + "(" + t1.X + "," + t1.Y + ") ");
            sbList[1].Append(" (" + b1.X + "," + b1.Y + ")" + "(" + b2.X + "," + b2.Y + ") ");

            string borderStr = GetRowStr('-');
            sbList[2].Append(borderStr);

            int j = 0;
            for (int y = maxYCoord; y >= 0; y--)
            {
                StringBuilder rowStrB = new StringBuilder(GetRowStr(' '));

                sbList[3 + maxYCoord - y].Append(IsRotd ? GetRowStringBuilder(y, ps, rowStrB) : GetRowStringBuilder(ref j, y, ps, rowStrB));
            }
            sbList[4 + maxYCoord].Append(borderStr);

            return sbList;
        }
        public static string GetRowStr(char chr = ' ')
        {
            return "|" + new String(chr, maxXCoord + 1) + "|"; ;
        }
        public static StringBuilder GetRowStringBuilder(ref int j, int y, List<Point> ps, StringBuilder rowStrB)
        {
            while (j < ps.Count && ps[j].Y == y)
            {
                SetStringBuilder(ps[j], rowStrB);
                j++;
            }
            return rowStrB;
        }
        private static StringBuilder GetRowStringBuilder(int y, List<Point> ps, StringBuilder rowStrB)
        {
            foreach (var p in ps)
            {
                if (p.Y == y)
                {
                    SetStringBuilder(p, rowStrB);
                }
            }
            return rowStrB;
        }
        private static void SetStringBuilder(Point p, StringBuilder sb)
        {
            sb.Remove(p.X + 1, 1);
            sb.Insert(p.X + 1, "+");
        }
        private static void PlotResults(int objCount, List<List<StringBuilder>> listOfSbLists, string type)
        {
            WriteHeader(objCount, type);
            // Console.WriteLine();
            // Console.WriteLine($"-------------------  {type} No.: {objCount}  -------------------");
            // Console.WriteLine();

            if (listOfSbLists.Count > 0 && listOfSbLists[0][0].Length > 1)
                foreach (var SBList in listOfSbLists)
                {
                    foreach (var sb in SBList)
                        Console.WriteLine(sb);

                    Console.WriteLine();
                }
        }
        private static void WriteHeader(int objCount, string type)
        {
            Console.WriteLine();
            Console.WriteLine($"-------------------  {type} No.: {objCount}  -------------------");
            Console.WriteLine();
        }
        private static bool IsType(int type, int DxT, int DyT, int DxL, int DyL)
        {
            switch ((Quadrilateral)type)
            {
                case Quadrilateral.Parallelogram:
                    return true;
                case Quadrilateral.HorizontalRectangle:
                    return IsHorizontal();
                case Quadrilateral.DiagonalalRectangle:
                    return !IsHorizontal() && IsPerpendicular();
                // case Quadrilateral.Square:
                //     return IsEqualSided() && IsPerpendicular();                    
                case Quadrilateral.HorizontalSquare:
                    return IsEqualSided() && IsPerpendicular() && IsHorizontal();
                case Quadrilateral.DiagonalalSquare:
                    return IsEqualSided() && IsPerpendicular() && !IsHorizontal();
                default:
                    return false;
            }

            //C# 7.0, local functions
            bool IsHorizontal()
            {
                return (DyT == 0 && DxL == 0) || (DxT == 0 && DyL == 0);
            }

            bool IsPerpendicular()
            {
                return ((DyL != 0 && DxT != 0 && Math.Abs(DxL / (double)DyL) == Math.Abs(DyT / (double)DxT)) ||
                        (DxL != 0 && DyT != 0 && Math.Abs(DyL / (double)DxL) == Math.Abs(DxT / (double)DyT)));
            }

            bool IsEqualSided()
            {
                return Math.Abs(DxT) == Math.Abs(DyL) && Math.Abs(DyT) == Math.Abs(DxL);
            }

        }

        private static Func<int, int, int, int, bool> AreSidesEqual = (DxT, DyT, DxL, DyL) => Math.Abs(DxT) == Math.Abs(DyL) && Math.Abs(DyT) == Math.Abs(DxL);
        // private static Predicate<List<int>> AreSidesEqual = (Diffs) => Math.Abs(Diffs[0]) == Math.Abs(Diffs[3]) && Math.Abs(Diffs[1]) == Math.Abs(Diffs[2]);
        private static Func<int, int, int, int, bool> AreSidesPerpendicular = (DxT, DyT, DxL, DyL) =>
                       ((DyL != 0 && DxT != 0 && Math.Abs(DxL / (double)DyL) == Math.Abs(DyT / (double)DxT)) ||
                        (DxL != 0 && DyT != 0 && Math.Abs(DyL / (double)DxL) == Math.Abs(DxT / (double)DyT)));
        private static Func<int, int, int, int, bool> IsSquare = (DxT, DyT, DxL, DyL) => AreSidesEqual(DxT, DyT, DxL, DyL) && AreSidesPerpendicular(DxT, DyT, DxL, DyL);

        private static void InitTestPoints(int testCase)
        {
            points.Clear();
            switch (testCase)
            {
                case 1:
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 4));
                    points.Add(new Point(4, 4));
                    points.Add(new Point(3, 3));
                    break;
                case 2:
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 4));
                    points.Add(new Point(4, 4));
                    points.Add(new Point(1, 3));
                    points.Add(new Point(2, 2));
                    points.Add(new Point(5, 3));
                    points.Add(new Point(4, 2));
                    break;
                case 3:
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 4));
                    points.Add(new Point(4, 4));
                    points.Add(new Point(1, 2));
                    points.Add(new Point(2, 1));
                    points.Add(new Point(5, 2));
                    points.Add(new Point(4, 1));
                    break;
                case 4:
                    points.Add(new Point(3, 5));
                    points.Add(new Point(5, 5));
                    points.Add(new Point(2, 5));
                    points.Add(new Point(4, 5));

                    points.Add(new Point(2, 4));
                    points.Add(new Point(4, 4));

                    points.Add(new Point(5, 3));
                    points.Add(new Point(3, 3));
                    points.Add(new Point(2, 3));
                    points.Add(new Point(1, 3));
                    break;
                case 5:
                    points.Add(new Point(5, 5));
                    points.Add(new Point(4, 5));
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 5));

                    points.Add(new Point(5, 1));
                    points.Add(new Point(3, 1));
                    points.Add(new Point(2, 1));
                    points.Add(new Point(1, 1));
                    break;
                case 6:
                    points.Add(new Point(5, 5));
                    points.Add(new Point(4, 5));
                    points.Add(new Point(2, 5));

                    points.Add(new Point(2, 4));
                    points.Add(new Point(4, 4));

                    points.Add(new Point(5, 3));

                    points.Add(new Point(2, 3));
                    break;
                case 7:
                    points.Add(new Point(5, 5));
                    points.Add(new Point(4, 5));
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 5));

                    points.Add(new Point(5, 4));
                    points.Add(new Point(4, 4));
                    points.Add(new Point(3, 4));
                    points.Add(new Point(2, 4));

                    points.Add(new Point(5, 3));
                    points.Add(new Point(4, 3));
                    points.Add(new Point(3, 3));
                    points.Add(new Point(2, 3));

                    points.Add(new Point(5, 2));
                    points.Add(new Point(4, 2));
                    points.Add(new Point(3, 2));
                    points.Add(new Point(2, 2));
                    break;

                default:
                    break;
            }
            points = points.OrderByDescending(p => p.Y).ThenByDescending(p => p.X).ToList();
        }
        private static void InitTestPoints(TestCase testCase)
        {
            Console.WriteLine(testCase);
            points.Clear();
            switch (testCase)
            {
                case TestCase.HorizontalLine:
                    points.Add(new Point(4, 5));
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 5));
                    points.Add(new Point(1, 5));
                    break;
                case TestCase.VercitalLine:
                    points.Add(new Point(3, 5));
                    points.Add(new Point(3, 4));
                    points.Add(new Point(3, 3));
                    points.Add(new Point(3, 2));
                    break;
                case TestCase.DiagonalLine:
                    points.Add(new Point(5, 5));
                    points.Add(new Point(4, 4));
                    points.Add(new Point(3, 3));
                    points.Add(new Point(2, 2));
                    break;
                case TestCase.OneTriangle:
                    points.Add(new Point(5, 5));
                    points.Add(new Point(2, 5));
                    points.Add(new Point(3, 3));
                    points.Add(new Point(2, 2));
                    break;
                case TestCase.TwoTriangle:
                    points.Add(new Point(5, 5));
                    points.Add(new Point(2, 5));
                    points.Add(new Point(6, 1));
                    points.Add(new Point(2, 2));
                    break;
                case TestCase.LineAndTriangle:
                    points.Add(new Point(5, 5));
                    points.Add(new Point(2, 4));
                    points.Add(new Point(3, 3));
                    points.Add(new Point(2, 2));
                    break;
                case TestCase.OneHorizontalRectangle:
                    points.Add(new Point(5, 5));
                    points.Add(new Point(2, 5));
                    points.Add(new Point(5, 4));
                    points.Add(new Point(2, 4));
                    break;
                case TestCase.TwoHorizontalRectangle:
                    points.Add(new Point(5, 5));
                    points.Add(new Point(2, 5));
                    points.Add(new Point(5, 4));
                    points.Add(new Point(2, 4));

                    points.Add(new Point(8, 3));
                    points.Add(new Point(3, 3));
                    points.Add(new Point(8, 0));
                    points.Add(new Point(3, 0));
                    break;
                case TestCase.OneDiagonalRectangle:
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 2));
                    points.Add(new Point(6, 4));
                    points.Add(new Point(5, 1));
                    break;
                case TestCase.OneDiagonalParallelogram:
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 2));
                    points.Add(new Point(5, 4));
                    points.Add(new Point(4, 1));
                    break;
                case TestCase.OneHorizontalSquare:
                    points.Add(new Point(7, 5));
                    points.Add(new Point(5, 5));
                    points.Add(new Point(7, 3));
                    points.Add(new Point(5, 3));
                    break;
                case TestCase.OneDiagonalSquare:
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 4));
                    points.Add(new Point(4, 4));
                    points.Add(new Point(3, 3));
                    break;
                case TestCase.TwoDiagonalSquare:
                    points.Add(new Point(1, 5));
                    points.Add(new Point(0, 4));
                    points.Add(new Point(2, 4));
                    points.Add(new Point(1, 3));

                    points.Add(new Point(6, 5));
                    points.Add(new Point(4, 3));
                    points.Add(new Point(8, 3));
                    points.Add(new Point(6, 1));
                    break;
                case TestCase.ThreeHorizontalRectangle:
                    points.Add(new Point(7, 5));
                    points.Add(new Point(5, 5));
                    points.Add(new Point(7, 3));
                    points.Add(new Point(5, 3));

                    points.Add(new Point(6, 5));
                    points.Add(new Point(6, 3));
                    break;

                default:
                    break;
            }
            points = points.OrderByDescending(p => p.Y).ThenByDescending(p => p.X).ToList();
        }
    }
}
