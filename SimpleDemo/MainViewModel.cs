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
    using HelixToolkit.Wpf.SharpDX.Utilities;
    using System.Windows.Media;
    using System.Drawing;
    using HelixToolkit.Wpf.SharpDX.Extensions;
    using System.Drawing.Imaging;
    using System;

    /// <summary>
    /// Aplikacja do wyświetlania chmury punktów,
    /// zrobiona na początku kariery programisty
    /// PROSZĘ NIE OCENIAĆ
    /// WFWF
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        public HelixToolkit.Wpf.SharpDX.MeshGeometry3D Model { get; private set; } // mesh to jakby model WFWF
        public HelixToolkit.Wpf.SharpDX.MeshGeometry3D TextModel { get; private set; }
        public LineGeometry3D Lines { get; private set; }
        public LineGeometry3D Arrow { get; private set; }
        public LineGeometry3D Grid { get; private set; }
        public PointGeometry3D Points { get; private set; }
        public BillboardText3D Text { get; private set; } // text, który może być np. nad punktami (ogólnie w VIEW)
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

        private ICommand _openCommand;

        private ICommand _displayCommand;

        private ICommand _clearCommand;

        private List<double> positionX = new List<double>();

        private  List<double> positionY = new List<double>();

        private List<double> positionZ = new List<double>();

        private LineBuilder lines = new LineBuilder();

        private LineBuilder arrows = new LineBuilder();

        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand(
                  param => this.dataFromTxt(filepath, positionX, positionY, positionZ),
                  param => true
                   );
                }
                return _openCommand;
            }

        }
        public ICommand DisplayCommand
        {
            get
            {
                if (_displayCommand == null)
                {
                    _displayCommand = new RelayCommand(
                  param => this.AddPointCloud(),
                  param => true
                   );
                }
                return _displayCommand;
            }

        }
        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new RelayCommand(
                  param => this.ClearView(),
                  param => true
                   );
                }
                return _clearCommand;
            }

        }
        public string filepath { get; set; }
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
                LookDirection = new Vector3D(0, 10, 5),//z kamerą jest problem przy przybliżaniu prawdopodobnie przez ten LookDirection albo sposób poruszania kamerą
                UpDirection = new Vector3D(0, 0, 1),
                FarPlaneDistance = 5000000

            };

            // setup lighting            
            AmbientLightColor = Colors.DimGray;
            DirectionalLightColor = Colors.White; // światełko musi być 

            DirectionalLightDirection = Camera.LookDirection;
            MainView();
            AddPointCloud();

            UpXCommand = new RelayCommand(x => { UpDirection = new Vector3D(1, 0, 0); });
            UpYCommand = new RelayCommand(x => { UpDirection = new Vector3D(0, 1, 0); });
            UpZCommand = new RelayCommand(x => { UpDirection = new Vector3D(0, 0, 1); });

        }

        private void ClearView()
        {
            // floor plane grid

            var Maxreset = 100;
            var Minreset = -100;
            EffectsManager.Dispose();
            EffectsManager = new DefaultEffectsManager();
            var points = new PointGeometry3D();
            var col = new Color4Collection(); // gradient na kolory
            var ptPos = new Vector3Collection(); // pozycje punktów
            var ptIdx = new IntCollection(); // indeksy punktów
            arrows = lines = new LineBuilder();

            points.Positions = ptPos;
            points.Indices = ptIdx;
            points.Colors = col;
            PointsColor = Colors.White; // <- nie pamiętam po co to tutaj ale bez tego nie działa
            Points = points;
            PointsTransform = new Media3D.TranslateTransform3D(0, 0, Minreset);
            
            CreateGrid(Maxreset, Minreset, Maxreset, Maxreset, Minreset);
            Grid = lines.ToLineGeometry3D();
            GridColor = new Color4(153 / 255.0f, 204 / 255.0f, 255 / 255.0f, (float)0.3).ToColor(); ;
            GridTransform = new Media3D.TranslateTransform3D(0, 0, Minreset);

            // strzałki 

            CreateArrows(Maxreset, Minreset, Maxreset, Minreset, Maxreset, Minreset);
            Arrow = arrows.ToLineGeometry3D();
            ArrowColor = new Color4(0, 255 / 255.0f, 255 / 255.0f, (float)0.5).ToColor(); ;
            ArrowTransform = new Media3D.TranslateTransform3D(0, 0, Minreset);
        }
        private void MainView()
        {


        }
        private void AddPointCloud()
        {
            var points = new PointGeometry3D();
            var col = new Color4Collection(); // gradient na kolory
            var ptPos = new Vector3Collection(); // pozycje punktów
            var ptIdx = new IntCollection(); // indeksy punktów


            int dataSampling = 2; // taki prosty przeskok miedzy punktami /można zrobić coś na podstawie odległości kamery albo różnicy wysokości między kolejnymi punktami

            // wpisywane na sztywno dane (lepiej zrobić funkcję obsługującą odpowiednie typy)
            // path = "C:/Users/astat/Desktop/DaneViewer/xyzi_wrakII.txt";
            //path = "C:/Users/astat/Desktop/DaneViewer/export_xyzi.txt";
            if (filepath != null && positionX != null && positionY != null && positionZ != null)
            {
                dataFromTxt(filepath, positionX, positionY, positionZ);
                //newdataFromTxt(path, positionX, positionY, positionZ);
                double maxX = positionX.Max();
                double minX = positionX.Min();
                double maxY = positionY.Max();
                double minY = positionY.Min();
                double maxZ = positionZ.Max();
                double minZ = positionZ.Min();

                Trace.WriteLine($"{maxX} {minX} {maxY} {minY} {maxZ} {minZ}");

                for (int i = 0; i < positionX.Count(); i += dataSampling) // tutaj powinna być pętla foreach ale potrzebowałem prostego przerzedzenia dannych
                {
                    //  Trace.WriteLine($"{i} {positionX[i]} {positionY[i]} {positionZ[i]}");
                    bool reverse = true;

                    var positionZReversed = maxZ - positionZ[i]; // czasami dostawałem punkty z odwróconą osią Z po to ta zmienna 

                    var positionToColour = Math.Abs(positionZ[i] / (maxZ-minZ));

                    ptIdx.Add(ptPos.Count);
                    if (reverse == true) ptPos.Add(new Vector3((float)positionX[i], (float)positionY[i], (float)positionZReversed));
                    else ptPos.Add(new Vector3((float)positionX[i], (float)positionY[i], (float)positionZ[i]));

                    // Trace.WriteLine($"{i} {positionX[i]} {positionY[i]} {positionZ[i]} {positionToColour}"); 

                    var colourR = Math.Sqrt(-positionToColour + 0.75);
                    var colourG = (Math.Sin(4*positionToColour - 0.2))/1.6;//tutaj trzeba potworzyć funkcje matematyczne żeby zmieniały się kolorki/ można też użyć warunków i color4.Scale 
                    var colourB = Math.Sqrt(positionToColour - 0.25);

                    col.Add(new Color4((float)colourR, (float)colourG, (float)colourB, 1f)); //te kolory to czarna magia ale nie są według żadnej skali tylko żeby ładnie wyglądały hehe
                }                                                                        //Trace.WriteLine($"{i} {positionToColour} {colourR} {colourG} {colourB} {col[i]}");

                points.Positions = ptPos;
                points.Indices = ptIdx;
                points.Colors = col;
                PointsColor = Colors.White; // <- nie pamiętam po co to tutaj ale bez tego nie działa
                Points = points;
                PointsTransform = new Media3D.TranslateTransform3D(0, 0, minZ);

                // floor plane grid

                CreateGrid(maxX, minX, maxY, minY, minZ);
                Grid = lines.ToLineGeometry3D();
                GridColor = new Color4(153 / 255.0f, 204 / 255.0f, 255 / 255.0f, (float)0.3).ToColor(); ;
                GridTransform = new Media3D.TranslateTransform3D(0, 0, minZ);

                // strzałki 

                CreateArrows((float)maxX, (float)minX, (float)maxY, (float)minY, (float)maxZ, (float)minZ);
                Arrow = arrows.ToLineGeometry3D();
                ArrowColor = new Color4(0, 255 / 255.0f, 255 / 255.0f, (float)0.5).ToColor(); ;
                ArrowTransform = new Media3D.TranslateTransform3D(0, 0, minZ);

            }

        }
        private void CreateArrows(float gridMaxX, float gridMinX, float gridMaxY, float gridMinY, float gridMaxZ, float gridMinZ)
        {

            var offset = 20;
            var width = 10;

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

        }
        private void CreateGrid(double gridMaxX, double gridMinX, double gridMaxY, double gridMinY, double gridMinZ)
        {
            for (double i = gridMinY; i < gridMaxY - 10; i += (gridMaxY - gridMinY) / 10)  // tworzenie siatki
            {
                Vector3 p1 = new Vector3((float)gridMaxX, (float)i,(float)gridMinZ);
                Vector3 p2 = new Vector3((float)gridMinX, (float)i, (float)gridMinZ);
                lines.AddLine(p1, p2);
            }

        }
        public void dataFromTxt(string path, List<double> positionX, List<double> positionY, List<double> positionZ)
        {
            StreamReader sr = new StreamReader(path); // przechwytywanie danych
            string headerLine = sr.ReadLine(); // usunięcie pierwszego wiersza
            string data = sr.ReadLine();

            int i = 0;

            while ((data != null)) //pobieranie wartości i przypisywanie ich do list / można zrobić lepiej
            {
                //Trace.WriteLine(i+":  "+data);
                string[] values = data.Split(',');
                values.OrderBy(x => values[1]);
                positionX.Add(10 * (double.Parse(values[0])));
                positionY.Add(10 * (double.Parse(values[1])) - 10);
                positionZ.Add(10 * (double.Parse(values[2])));

                data = sr.ReadLine();
                i++;
            }
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

                positionX.Add(10 * double.Parse(values[0]));
                positionY.Add(10 * double.Parse(values[1]));
                positionZ.Add(10 * double.Parse(values[2]));

                data = sr.ReadLine();
                i++;
            }
            return (positionX, positionY, positionZ);
        }

    }
}
