using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PolygonCount
{
    class Polygon
    {

        protected string name;
        public string Name
        {
            get { return name; }
        }

        protected readonly Point[] verteces;
        public Point[] Verteces { get { return verteces; } }


        public Polygon(Quadrilateral quadrilateralType, Point[] pVerteces)
        {
            verteces = pVerteces;
            name = quadrilateralType.ToString();
        }

        // private static Dictionary<Quadrilateral, Polygon> polygonTypeDict;
        // // Design Pattern: Simple Factory 
        // public static Polygon Create(Quadrilateral type, Point[] pVerteces)
        // {
        //     // Design Pattern: Lazy Loading
        //     if (polygonTypeDict == null)
        //     {
        //         polygonTypeDict = new Dictionary<Quadrilateral, Polygon>()
        //         {
        //             {Quadrilateral.Parallelogram, new Polygon(Quadrilateral.Parallelogram,pVerteces)},
        //             {Quadrilateral.HorizontalRectangle, new Polygon(Quadrilateral.HorizontalRectangle,pVerteces)},
        //             {Quadrilateral.DiagonalalRectangle, new Polygon(Quadrilateral.DiagonalalRectangle,pVerteces)},
        //             {Quadrilateral.Square, new Polygon(Quadrilateral.Square,pVerteces)},
        //             {Quadrilateral.HorizontalSquare, new Polygon(Quadrilateral.HorizontalSquare,pVerteces)},
        //             {Quadrilateral.DiagonalalSquare, new Polygon(Quadrilateral.DiagonalalSquare,pVerteces)}
        //         };
        //     }

        //     // Design Pattern: RIP
        //     return polygonTypeDict[type];
        // }


    }
}