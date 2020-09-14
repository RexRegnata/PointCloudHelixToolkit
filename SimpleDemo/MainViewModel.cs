// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SimpleDemo
{
    using System.Linq;
    using DemoCore;
    using HelixToolkit.Wpf.SharpDX;
    using SharpDX;
    using Media3D = System.Windows.Media.Media3D;
    using Point3D = System.Windows.Media.Media3D.Point3D;
    using Vector3D = System.Windows.Media.Media3D.Vector3D;
    using Transform3D = System.Windows.Media.Media3D.Transform3D;
    using Color = System.Windows.Media.Color;  
    using Vector3 = SharpDX.Vector3;
    using Colors = System.Windows.Media.Colors;
    using Color4 = SharpDX.Color4;   
    using System.Windows.Media.Imaging;
    using System.IO;
    using System.Windows.Input;   
    using System.Windows.Media.Media3D;    
    using System.Diagnostics; 
    using System.Collections.Generic;

    public class MainViewModel : BaseViewModel
    {
        public HelixToolkit.Wpf.SharpDX.MeshGeometry3D Model { get; private set; }
        public HelixToolkit.Wpf.SharpDX.MeshGeometry3D TextModel { get; private set; }
        public LineGeometry3D Lines { get; private set; }
        public LineGeometry3D Arrow { get; private set; }
        public LineGeometry3D Grid { get; private set; }
        public PointGeometry3D Points { get; private set; }
        public BillboardText3D Text { get; private set; }

        public Color GridColor { get; private set; }
        public Color ArrowColor { get; private set; }
        public Color PointsColor { get; private set; }
        public Transform3D GridTransform { get; private set; }
        public Transform3D PointsTransform { get; private set; }
        public Transform3D ArrowTransform { get; private set; }

        public Vector3D DirectionalLightDirection { get; private set; }
        public Color DirectionalLightColor { get; private set; }
        public Color AmbientLightColor { get; private set; }

        public Vector3D UpDirection { set; get; } = new Vector3D(0, 1, 0);
        public Stream BackgroundTexture { get; }
        public ICommand UpXCommand { private set; get; }
        public ICommand UpYCommand { private set; get; }
        public ICommand UpZCommand { private set; get; }

        public MainViewModel()
        {
            EffectsManager = new DefaultEffectsManager();
            // titles
            Title = "Viewer";
            SubTitle = "Marine Technology";

            // camera setup
            Camera = new HelixToolkit.Wpf.SharpDX.PerspectiveCamera
            {
                Position = new Point3D(0, -5, 5),
                LookDirection = new Vector3D(0, 10, 5),
                UpDirection = new Vector3D(0, 0, 1),
                FarPlaneDistance = 5000000
                
            };
                     
            // setup lighting            
            AmbientLightColor = Colors.DimGray;
            DirectionalLightColor = Colors.White;

            DirectionalLightDirection = Camera.LookDirection; 

            var positionX = new List<double>();
            var positionY = new List<double>();
            var positionZ = new List<double>();

            var points = new PointGeometry3D();
            var col = new Color4Collection();
            var ptPos = new Vector3Collection();
            var ptIdx = new IntCollection();
            var lines = new LineBuilder();
            var arrows = new LineBuilder();

            float positionToColor = 0;
            float positionZReversed = 0;
            int dataSampling = 2;
    
            string path = "C:/Users/astat/Desktop/DaneViewer/xyzi.txt";
            //path = "C:/Users/astat/Desktop/DaneViewer/xyzi_wrakII.txt";
            //path = "C:/Users/astat/Desktop/DaneViewer/export_xyzi.txt";

            dataFromTxt(path, positionX, positionY, positionZ);
            //newdataFromTxt(path, positionX, positionY, positionZ);

            int gridMaxX = (int)positionX.Max();
            int gridMinX = (int)positionX.Min();
            int gridMaxY = (int)positionY.Max();
            int gridMinY = (int)positionY.Min();
            int gridMaxZ = (int)positionZ.Max();
            int gridMinZ = (int)positionZ.Min();
            float colorMaxZ = (float)positionZ.Max();

            Trace.WriteLine(colorMaxZ);

            addPointCloud(positionZReversed, positionToColor, colorMaxZ, positionX, positionY, positionZ, ptIdx, ptPos, col, dataSampling, gridMaxZ);

            Trace.WriteLine(positionToColor);

            points.Positions = ptPos;
            points.Indices = ptIdx;
            points.Colors = col;
            PointsColor = Colors.White;
            Points = points;
            PointsTransform = new Media3D.TranslateTransform3D(0, -10, -5);
           
            // floor plane grid

            createGrid(gridMaxX, gridMinX, gridMaxY, gridMinZ, lines);
            Grid = lines.ToLineGeometry3D();
            GridColor = new Color4(153 / 255.0f, 204 / 255.0f, 255 / 255.0f, (float)0.3).ToColor(); ;
            GridTransform = new Media3D.TranslateTransform3D(0, 0, gridMinZ - 2);

            createArrows(gridMaxX, gridMinX, gridMaxY, gridMinY, gridMaxZ, gridMinZ, arrows);
            Arrow = arrows.ToLineGeometry3D();
            ArrowColor = new Color4(0, 255 / 255.0f, 255 / 255.0f, (float)0.5).ToColor(); ;
            ArrowTransform = new Media3D.TranslateTransform3D(0, 0, gridMinZ - 2);

            UpXCommand = new RelayCommand(x => { UpDirection = new Vector3D(1, 0, 0); });
            UpYCommand = new RelayCommand(x => { UpDirection = new Vector3D(0, 1, 0); });
            UpZCommand = new RelayCommand(x => { UpDirection = new Vector3D(0, 0, 1); });
        }

        static (IntCollection, Vector3Collection, Color4Collection) addPointCloud(float positionZReversed, float positionToColor, float colorMaxZ, List<double> positionX, List<double> positionY, List<double> positionZ, IntCollection ptIdx, Vector3Collection ptPos, Color4Collection col, int dataSampling, int gridMaxZ)
        {
            for (int i = 0; i < positionX.Count(); i += dataSampling)
            {
                positionZReversed = (float)gridMaxZ - (float)positionZ[i];
                positionToColor = (float)(positionZ[i] / colorMaxZ);
                ptIdx.Add(ptPos.Count);
                ptPos.Add(new Vector3((float)(positionX[i]), (float)(positionY[i]), positionZReversed));
                col.Add(new Color4(positionToColor / 10, (1 - positionToColor), (float)(positionToColor), 1f));
            }
            return (ptIdx, ptPos, col);
        }
        static LineBuilder createArrows(int gridMaxX, int gridMinX, int gridMaxY, int gridMinY, int gridMaxZ, int gridMinZ, LineBuilder arrows)
        {

            var offset = 5;
            var width = 2;

            Vector3 p0 = new Vector3(gridMinX, gridMaxY, gridMinZ);

            //y
            Vector3 px1 = new Vector3(gridMinX, gridMinY, gridMinZ);
            Vector3 px2 = new Vector3(gridMinX + width, gridMinY + offset, gridMinZ);
            Vector3 px3 = new Vector3(gridMinX - width, gridMinY + offset, gridMinZ);

            //x
            Vector3 py1 = new Vector3(gridMaxX, gridMaxY, gridMinZ);
            Vector3 py2 = new Vector3(gridMaxX - offset, gridMaxY + width, gridMinZ);
            Vector3 py3 = new Vector3(gridMaxX - offset, gridMaxY - width, gridMinZ);

            //z
            Vector3 pz1 = new Vector3(gridMinX, gridMaxY, gridMaxZ);
            Vector3 pz2 = new Vector3(gridMinX + width, gridMaxY, gridMaxZ - offset);
            Vector3 pz3 = new Vector3(gridMinX - width, gridMaxY, gridMaxZ - offset);

            arrows.AddLine(p0, px1);
            arrows.AddLine(px1, px2);
            arrows.AddLine(px1, px3);

            arrows.AddLine(p0, py1);
            arrows.AddLine(py1, py2);
            arrows.AddLine(py1, py3);

            arrows.AddLine(p0, pz1);
            arrows.AddLine(pz1, pz2);
            arrows.AddLine(pz1, pz3);

            return arrows;
        }
        static LineBuilder createGrid(int gridMaxX, int gridMinX, int gridMaxY, int gridMinZ, LineBuilder lines)
        {

            for (int i = gridMaxY; i >= 0; i -= 10)
            {
                Vector3 p1 = new Vector3(gridMaxX, i,0);
                Vector3 p2 = new Vector3(gridMinX, i, 0);
                lines.AddLine(p1, p2);
            }
            return lines;
        }
        static (List<double> positionX, List<double> positionY, List<double> positionZ) dataFromTxt(string path, List<double> positionX, List<double> positionY, List<double> positionZ)
        {
            StreamReader sr = new StreamReader(path);
            string headerLine = sr.ReadLine();
            string data = sr.ReadLine();

            int i = 0;

            while ((data != null))
            {
                //Trace.WriteLine(i+":  "+data);
                string[] values = data.Split(',');
                values.OrderBy(x => values[1]);
                positionX.Add(double.Parse(values[0]));
                positionY.Add(double.Parse(values[1]) - 10);
                positionZ.Add(double.Parse(values[2]));

                data = sr.ReadLine();
                i++;
            }

            return (positionX, positionY, positionZ);
        }
        static (List<double> positionX, List<double> positionY, List<double> positionZ) newdataFromTxt(string path, List<double> positionX, List<double> positionY, List<double> positionZ)
        {
            StreamReader sr = new StreamReader(path);
            string headerLine = sr.ReadLine();
            string data = sr.ReadLine();
            double maxY = positionY.Max();
            int i = 0;

            while ((data != null))
            {
                //Trace.WriteLine(i+":  "+data);
                string[] values = data.Split(',');
                values.OrderBy(x => values[1]);

                positionX.Add(double.Parse(values[0]));
                positionY.Add(double.Parse(values[1]) - 10 + maxY);
                positionZ.Add(double.Parse(values[2]));

                data = sr.ReadLine();
                i++;
            }
            return (positionX, positionY, positionZ);
        }

        private BitmapSource CreateBitmapSample()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //Read the texture description           
            var texDescriptionStream = assembly.GetManifestResourceStream("SimpleDemo.Sample.png");
            var decoder = new PngBitmapDecoder(texDescriptionStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnDemand);
            return decoder.Frames[0];
        }

        private Stream CreatePNGSample()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //Read the texture description           
            var texDescriptionStream = assembly.GetManifestResourceStream("SimpleDemo.Sample.png");
            texDescriptionStream.Position = 0;
            return texDescriptionStream;
        }
    }
}
