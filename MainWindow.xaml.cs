using PZ2.Common;
using PZ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = PZ2.Models.Point;

namespace PZ2
{
    public partial class MainWindow : Window
    {

        //COLOR CHANGE COUNTER
        int colorChangeCounter = 0;
        int colorChangeCounterSwitch = 0;

        //XML LOADING AND BASIC DATA COLLECTIONS INIT
        static Data data;

        //TOOLTIP & HIT

        ToolTip toolTip = new ToolTip();
        System.Windows.Point toolTipMousePosition = new System.Windows.Point();
        GeometryModel3D geometryModelHit;

        //TRANSFORMATION VARIABLES

        double scaleX = 1;
        double scaleY = 1;
        double scaleZ = 1;
        int currentZoom = 1;
        int minZoom = -1;
        int maxZoom = 45;
        System.Windows.Point panStart = new System.Windows.Point();
        System.Windows.Point panOffset = new System.Windows.Point();
        System.Windows.Point rotateStartPosition = new System.Windows.Point();

        public MainWindow()
        {
            InitializeComponent();
            data = new Data();
            DrawNodes();
            DrawSubstations();
            DrawSwitches();
            DrawLines();
        }

        //3D OBJECT DRAWING

        public void DrawNodes()
        {
            foreach (var node in data.NodeList)
            {

                MeshGeometry3D meshGeometry3D = CreateCube(node.Longitude, node.Latitude, node.Id);
                DiffuseMaterial diffuseMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.DarkOliveGreen));

                GeometryModel3D geometryModel3D = new GeometryModel3D(meshGeometry3D, diffuseMaterial);

                RotateTransform3D rotateTransform3D = new RotateTransform3D();

                Transform3DGroup myTransform3DGroup = new Transform3DGroup();
                myTransform3DGroup.Children.Add(rotateTransform3D);

                //SO THAT THE MODELS DO NOT STAY STILL WHILE DOING TRANFORMATIONS WE ADD IT TO THE SAME TRANSFORMATION GROUP AS THE MAP
                geometryModel3D.Transform = myTransform3DGroup; 

                data.EntityGeo.Add(geometryModel3D, node);
                model3dGroup.Children.Add(geometryModel3D);

                //DODATNI ZADATAK
                data.NodeGeometryModel.Add(geometryModel3D);

                //SAVING ALL ENTITIES
                data.AllEntities.Add(geometryModel3D, node.Id);
            }

        }

        public void DrawSubstations()
        {
            foreach (var sub in data.SubstationList)
            {
                MeshGeometry3D meshGeometry3D = CreateCube(sub.Longitude, sub.Latitude, sub.Id);
                DiffuseMaterial diffuseMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Violet));

                GeometryModel3D geometryModel3D = new GeometryModel3D(meshGeometry3D, diffuseMaterial);

                RotateTransform3D rotateTransform3D = new RotateTransform3D();

                Transform3DGroup myTransform3DGroup = new Transform3DGroup();
                myTransform3DGroup.Children.Add(rotateTransform3D);

                //SO THAT THE MODELS DO NOT STAY STILL WHILE DOING TRANFORMATIONS WE ADD IT TO THE SAME TRANSFORMATION GROUP AS THE MAP
                geometryModel3D.Transform = myTransform3DGroup;

                data.EntityGeo.Add(geometryModel3D, sub);
                model3dGroup.Children.Add(geometryModel3D);

                //DODATNI ZADATAK
                data.SubstationsGeometryModel.Add(geometryModel3D);

                data.AllEntities.Add(geometryModel3D, sub.Id);
            }
        }

        public void DrawSwitches()
        {
            foreach (var sw in data.SwitchList)
            {
                MeshGeometry3D meshGeometry3D = CreateCube(sw.Longitude, sw.Latitude, sw.Id);
                DiffuseMaterial diffuseMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.MediumPurple));

                GeometryModel3D geometryModel3D = new GeometryModel3D(meshGeometry3D, diffuseMaterial);

                RotateTransform3D rotateTransform3D = new RotateTransform3D();

                Transform3DGroup myTransform3DGroup = new Transform3DGroup();
                myTransform3DGroup.Children.Add(rotateTransform3D);

                //SO THAT THE MODELS DO NOT STAY STILL WHILE DOING TRANFORMATIONS WE ADD IT TO THE SAME TRANSFORMATION GROUP AS THE MAP
                geometryModel3D.Transform = myTransform3DGroup;

                data.EntityGeo.Add(geometryModel3D, sw);
                model3dGroup.Children.Add(geometryModel3D);

                //DODATNI ZADATAK
                data.SwitchGeometryModel.Add(geometryModel3D);

                data.AllEntities.Add(geometryModel3D, sw.Id);
            }
        }

        public void DrawLines()
        {
            foreach (var line in data.LinesList)
            {
                System.Windows.Point point1 = new System.Windows.Point();
                System.Windows.Point point2 = new System.Windows.Point();
                var temp = 1;
                System.Windows.Point Startpoint; //START OF THE 3D LINE
                System.Windows.Point Endpoint; //END OF THE 3D LINE
                GeometryModel3D line3D;
                DiffuseMaterial color = new DiffuseMaterial();

                //BEGINING AND END DATA CHECKING
                if (data.EntityCollection.ContainsKey(line.FirstEnd) && data.EntityCollection.ContainsKey(line.SecondEnd))
                {
                    Startpoint = CreatePoint(data.EntityCollection[line.FirstEnd].Longitude, data.EntityCollection[line.FirstEnd].Latitude); //START OF THE 3D LINE
                    Endpoint = CreatePoint(data.EntityCollection[line.SecondEnd].Longitude, data.EntityCollection[line.SecondEnd].Latitude); //END OF THE 3D LINE

                }
                else
                    continue;

                if (line.ConductorMaterial == "Steel")
                    color = new DiffuseMaterial(System.Windows.Media.Brushes.Black);
                else if (line.ConductorMaterial == "Acsr")
                    color = new DiffuseMaterial(System.Windows.Media.Brushes.DarkGray);
                else if (line.ConductorMaterial == "Copper")
                    color = new DiffuseMaterial(System.Windows.Media.Brushes.DarkGoldenrod);

                foreach (var point in line.Vertices)
                {
                    if (temp == 1)
                    {
                        point1 = CreatePoint(point.Longitude, point.Latitude);
                        model3dGroup.Children.Add(CreateLines(Startpoint, point1, color));
                        temp++;
                        continue;
                    }
                    else if (temp == 2)
                    {
                        point2 = CreatePoint(point.Longitude, point.Latitude);
                        model3dGroup.Children.Add(CreateLines(point1, point2, color));
                        point1 = point2;
                    }
                }

                line3D = CreateLines(point2, Endpoint, color);

                data.AllLines.Add(line3D, line.Id);
                data.LineGeo.Add(line3D, line);

                //SO THAT THE MODELS DO NOT STAY STILL WHILE DOING TRANFORMATIONS WE ADD IT TO THE SAME TRANSFORMATION GROUP AS THE MAP

                model3dGroup.Children.Add(CreateLines(point2, Endpoint, color));
                data.LineGeometryModel.Add(line3D);
                data.AllEntities.Add(line3D, line.Id);
            }
        }

        //3D OBJECT DRAWING

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_hide_Click(object sender, RoutedEventArgs e)
        {

            if (model3dGroup.Children.Contains(data.SwitchGeometryModel[0]))
            {
                foreach (var geoModel in data.SwitchGeometryModel)
                {
                    model3dGroup.Children.Remove(geoModel);
                }
            }
            else if (model3dGroup.Children.Contains(data.NodeGeometryModel[0]))
            {
                foreach (var geoModel in data.NodeGeometryModel)
                {
                    model3dGroup.Children.Remove(geoModel);
                }
            }
            else if (model3dGroup.Children.Contains(data.SubstationsGeometryModel[0]))
            {
                foreach (var geoModel in data.SubstationsGeometryModel)
                {
                    model3dGroup.Children.Remove(geoModel);
                }
            }
        }

        private void btn_show_Click(object sender, RoutedEventArgs e)
        {
            if (!model3dGroup.Children.Contains(data.SubstationsGeometryModel[0]))
            {
                foreach (var geoModel in data.SubstationsGeometryModel)
                {
                    model3dGroup.Children.Add(geoModel);
                }
            }
            else if (!model3dGroup.Children.Contains(data.NodeGeometryModel[0]))
            {
                foreach (var geoModel in data.NodeGeometryModel)
                {
                    model3dGroup.Children.Add(geoModel);
                }
            }
            else if (!model3dGroup.Children.Contains(data.SwitchGeometryModel[0]))
            {
                foreach (var geoModel in data.SwitchGeometryModel)
                {
                    model3dGroup.Children.Add(geoModel);
                }
            }
        }

        private void btn_color_pipe_Click(object sender, RoutedEventArgs e)
        {

            if (colorChangeCounter == 1)
            {
                foreach (LineEntity line in data.LinesList)
                {
                    foreach (KeyValuePair<GeometryModel3D, long> gm in data.AllLines)
                    {
                        if (gm.Value == line.Id)
                        {
                            if (line.R < 1)
                                ((DiffuseMaterial)gm.Key.Material).Brush = System.Windows.Media.Brushes.Red;
                            else if (line.R >= 1 && line.R <= 2)
                                ((DiffuseMaterial)gm.Key.Material).Brush = System.Windows.Media.Brushes.Orange;
                            else if (line.R > 2)
                                ((DiffuseMaterial)gm.Key.Material).Brush = System.Windows.Media.Brushes.Yellow;
                        }
                    }
                }
                colorChangeCounter = 0;
            }
            else if (colorChangeCounter == 0)
            {
                foreach (LineEntity line in data.LinesList)
                {
                    foreach (KeyValuePair<GeometryModel3D, long> gm in data.AllLines)
                    {
                        if (gm.Value == line.Id)
                        {
                            if (line.ConductorMaterial == "Steel")
                                ((DiffuseMaterial)gm.Key.Material).Brush = System.Windows.Media.Brushes.Black;
                            else if (line.ConductorMaterial == "Acsr")
                                ((DiffuseMaterial)gm.Key.Material).Brush = System.Windows.Media.Brushes.DarkGray;
                            else if (line.ConductorMaterial == "Copper")
                                ((DiffuseMaterial)gm.Key.Material).Brush = System.Windows.Media.Brushes.DarkGoldenrod;
                        }
                    }
                }
                colorChangeCounter = 1;
            }
        }

        private void btn_color_switch_Click(object sender, RoutedEventArgs e)
        {
            if (colorChangeCounterSwitch == 1)
            {
                foreach (SwitchEntity svic in data.SwitchList)
                {
                    if (svic.Status.ToLower().Equals("open"))
                        data.AllEntities.FirstOrDefault(x => x.Value == svic.Id).Key.Material = new DiffuseMaterial(System.Windows.Media.Brushes.LightGreen);
                    else if (svic.Status.ToLower().Equals("closed"))
                        data.AllEntities.FirstOrDefault(x => x.Value == svic.Id).Key.Material = new DiffuseMaterial(System.Windows.Media.Brushes.Red);
                }
                colorChangeCounterSwitch = 0;
            }
            else if (colorChangeCounterSwitch == 0)
            {
                foreach (SwitchEntity svic in data.SwitchList)
                    data.AllEntities.FirstOrDefault(x => x.Value == svic.Id).Key.Material = new DiffuseMaterial(System.Windows.Media.Brushes.MediumPurple);

                colorChangeCounterSwitch = 1;
            }
        }

        //ZOOM Transformation

        private void MainViewPort_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            toolTip.IsOpen = false; //disable tooltip

            //if we are zoomed in less then the maximum zoom limit then zoom in a bit more

            if (e.Delta > 0 && currentZoom < maxZoom) //has the mouse wheel changed 
            {
                scaleX = scale.ScaleX + 0.1;
                scaleY = scale.ScaleY + 0.1;
                scaleZ = scale.ScaleZ + 0.1;
                scale.ScaleX = scaleX;
                scale.ScaleY = scaleY;
                scale.ScaleZ = scaleZ;
                currentZoom++;
            }

            //if we are zoomed out less then the minimal zoom limit then zoom out a bit more

            else if (e.Delta <= 0 && currentZoom > minZoom)
            {
                scaleX = scale.ScaleX - 0.1;
                scaleY = scale.ScaleY - 0.1;
                scaleZ = scale.ScaleZ - 0.1;
                scale.ScaleX = scaleX;
                scale.ScaleY = scaleY;
                scale.ScaleZ = scaleZ;
                currentZoom--;
            }

        }

        //PAN Transformation capture

        private void MainViewPort_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            toolTip.IsOpen = false; //disable tooltip

            MainViewPort.CaptureMouse(); //force mouse capture inside our ViewPort
            panStart = e.GetPosition(MainViewPort); 
            panOffset.X = translate.OffsetX; //so that the movement is fluid/from where do we start translating
            panOffset.Y = translate.OffsetY;
        }

        //PAN Transformation release

        private void MainViewPort_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainViewPort.ReleaseMouseCapture();
        }

        //ROTATE Transformation

        private void MainViewPort_MouseMove(object sender, MouseEventArgs e)
        {
            if(MainViewPort.IsMouseCaptured)
            {
                System.Windows.Point panEndPosition = e.GetPosition(this); 
                double offsetX = panEndPosition.X - panStart.X; //move for the difference between the end and start of the X
                double offsetY = panEndPosition.Y - panStart.Y; //move for the difference between the end and start of the Y
                //we need to multiply in order to be able to notice the translate effect and not move our mouse much
                double translateX = (offsetX * 150) / Width * 150; 
                double translateY = -(offsetY * 150) / Height * 150;
                translate.OffsetX = panOffset.X + (translateX / (150 * scale.ScaleX)) * 5;
                translate.OffsetY = panOffset.Y + (translateY / (150 * scale.ScaleX)) * 5;
            }

            System.Windows.Point currentRotatePosition = e.GetPosition(this); //current mouse position 

            if (e.MiddleButton == MouseButtonState.Pressed)
            {

                double rotX = currentRotatePosition.X - rotateStartPosition.X; //rotate for the difference between the end and start of the X
                double rotY = currentRotatePosition.Y - rotateStartPosition.Y; //rotate for the difference between the end and start of the Y
                //we need to specify a rotation step/speed
                double rotStep = 0.35;
                rotateX.Angle += rotStep * rotY;
                rotateY.Angle += rotStep * rotX;
            }

            rotateStartPosition = currentRotatePosition;
        }

        private void MainViewPort_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            toolTipMousePosition = e.GetPosition(MainViewPort); //GET THE CURRENT POSITION SO THAT WE KNOW WHERE TO DISPLAY THE TOOLTIP

            //FOR CREATING A TOOLTIP SURFACE/DIRECTION
            Point3D testpoint3D = new Point3D(toolTipMousePosition.X, toolTipMousePosition.Y, 0);
            Vector3D testdirection = new Vector3D(toolTipMousePosition.X, toolTipMousePosition.Y, 10); //POSITIVE Z ORIENTS TORWARDS US

            PointHitTestParameters pointparams = new PointHitTestParameters(toolTipMousePosition); //WHERE ARE WE GOING TO BE TESTING FOR HITS
            RayHitTestParameters rayparams = new RayHitTestParameters(testpoint3D, testdirection); //ALONG WHERE ARE WE GOING TO BE TESTING - RAY HIT TESTING

            geometryModelHit = null;
            VisualTreeHelper.HitTest(MainViewPort, null, HTResult, pointparams); //FOR DISPLAYING HIT TEST RESULTS
        }

        public static MeshGeometry3D CreateCube(double longitude, double latitude, long entityID)
        {
            var point = CreatePoint(longitude, latitude);
            int numEntity;

            if (data.NumberOfEntityOnPoint.ContainsKey(point))
            {
                data.NumberOfEntityOnPoint[point]++;
                numEntity = data.NumberOfEntityOnPoint[point];
            }
            else
            {
                data.NumberOfEntityOnPoint[point] = 1;
                numEntity = 1;
            }

            MeshGeometry3D meshGeometry3D = new MeshGeometry3D();

            List<Point3D> points3D = new List<Point3D>();

            //8 POINTS FOR DRAWING A CUBE
            points3D.Add(new Point3D(point.X - 4, point.Y - 4, numEntity * 5 - 5));//0 - BOTTOM
            points3D.Add(new Point3D(point.X + 4, point.Y - 4, numEntity * 5 - 5));//1 - BOTTOM
            points3D.Add(new Point3D(point.X + 4, point.Y - 4, numEntity * 5));
            points3D.Add(new Point3D(point.X - 4, point.Y - 4, numEntity * 5));
            points3D.Add(new Point3D(point.X - 4, point.Y + 4, numEntity * 5));
            points3D.Add(new Point3D(point.X + 4, point.Y + 4, numEntity * 5));
            points3D.Add(new Point3D(point.X + 4, point.Y + 4, numEntity * 5 - 5));//6 - BOTTOM
            points3D.Add(new Point3D(point.X - 4, point.Y + 4, numEntity * 5 - 5));//7 - BOTTOM

            meshGeometry3D.Positions = new Point3DCollection(points3D);

            //6 SIDES SO THERE ARE NOT ANY HOLES IN THE CUBE

            meshGeometry3D.TriangleIndices.Add(0);
            meshGeometry3D.TriangleIndices.Add(1);
            meshGeometry3D.TriangleIndices.Add(2); //TWO TRIANGLES PER CUBE SIDE
            meshGeometry3D.TriangleIndices.Add(0);
            meshGeometry3D.TriangleIndices.Add(2);
            meshGeometry3D.TriangleIndices.Add(3);

            meshGeometry3D.TriangleIndices.Add(3);
            meshGeometry3D.TriangleIndices.Add(5);
            meshGeometry3D.TriangleIndices.Add(4); //TWO TRIANGLES PER CUBE SIDE
            meshGeometry3D.TriangleIndices.Add(3);
            meshGeometry3D.TriangleIndices.Add(2);
            meshGeometry3D.TriangleIndices.Add(5);

            meshGeometry3D.TriangleIndices.Add(2);
            meshGeometry3D.TriangleIndices.Add(6);
            meshGeometry3D.TriangleIndices.Add(5); //TWO TRIANGLES PER CUBE SIDE
            meshGeometry3D.TriangleIndices.Add(2);
            meshGeometry3D.TriangleIndices.Add(1);
            meshGeometry3D.TriangleIndices.Add(6);

            meshGeometry3D.TriangleIndices.Add(3);
            meshGeometry3D.TriangleIndices.Add(7);
            meshGeometry3D.TriangleIndices.Add(0); //TWO TRIANGLES PER CUBE SIDE
            meshGeometry3D.TriangleIndices.Add(4);
            meshGeometry3D.TriangleIndices.Add(7);
            meshGeometry3D.TriangleIndices.Add(3);

            meshGeometry3D.TriangleIndices.Add(4);
            meshGeometry3D.TriangleIndices.Add(6);
            meshGeometry3D.TriangleIndices.Add(7); //TWO TRIANGLES PER CUBE SIDE
            meshGeometry3D.TriangleIndices.Add(4);
            meshGeometry3D.TriangleIndices.Add(5);
            meshGeometry3D.TriangleIndices.Add(6);

            meshGeometry3D.TriangleIndices.Add(0);
            meshGeometry3D.TriangleIndices.Add(7);
            meshGeometry3D.TriangleIndices.Add(1); //TWO TRIANGLES PER CUBE SIDE
            meshGeometry3D.TriangleIndices.Add(1);
            meshGeometry3D.TriangleIndices.Add(7);
            meshGeometry3D.TriangleIndices.Add(6);

            return meshGeometry3D;
        }

        public static GeometryModel3D CreateLines(System.Windows.Point point1, System.Windows.Point point2, DiffuseMaterial matColor)
        {
            MeshGeometry3D meshGeometry3D = new MeshGeometry3D();
            List<Point3D> points = new List<Point3D>();
            points.Add(new Point3D(point1.X - 0.1, point1.Y - 0.1, 0)); //0
            points.Add(new Point3D(point1.X + 0.1, point1.Y + 0.1, 0)); //1    
            points.Add(new Point3D(point1.X - 0.1, point1.Y - 0.1, 10)); //2   //FIRST CUBE FOR THE FIRST POINT
            points.Add(new Point3D(point1.X + 0.1, point1.Y + 0.1, 10)); //3

            points.Add(new Point3D(point2.X - 0.1, point2.Y - 0.1, 0)); //4
            points.Add(new Point3D(point2.X + 0.1, point2.Y + 0.1, 0)); //5    //SECOND CUBE FOR THE SECOND POINT
            points.Add(new Point3D(point2.X - 0.1, point2.Y - 0.1, 10)); //6
            points.Add(new Point3D(point2.X + 0.1, point2.Y + 0.1, 10)); //7


            meshGeometry3D.Positions = new Point3DCollection(points);
            meshGeometry3D.TriangleIndices.Add(0);
            meshGeometry3D.TriangleIndices.Add(2);
            meshGeometry3D.TriangleIndices.Add(3); // back
            meshGeometry3D.TriangleIndices.Add(3); //TWO TRIANGLES PER CUBE SIDE
            meshGeometry3D.TriangleIndices.Add(1);
            meshGeometry3D.TriangleIndices.Add(0); 

            meshGeometry3D.TriangleIndices.Add(4);
            meshGeometry3D.TriangleIndices.Add(6);
            meshGeometry3D.TriangleIndices.Add(2); // left
            meshGeometry3D.TriangleIndices.Add(2); //TWO TRIANGLES PER CUBE SIDE
            meshGeometry3D.TriangleIndices.Add(0);
            meshGeometry3D.TriangleIndices.Add(4); 

            meshGeometry3D.TriangleIndices.Add(5);
            meshGeometry3D.TriangleIndices.Add(7);
            meshGeometry3D.TriangleIndices.Add(6); //front
            meshGeometry3D.TriangleIndices.Add(6); //TWO TRIANGLES PER CUBE SIDE
            meshGeometry3D.TriangleIndices.Add(4);
            meshGeometry3D.TriangleIndices.Add(5);

            meshGeometry3D.TriangleIndices.Add(1);
            meshGeometry3D.TriangleIndices.Add(3);
            meshGeometry3D.TriangleIndices.Add(7); // right
            meshGeometry3D.TriangleIndices.Add(7); //TWO TRIANGLES PER CUBE SIDE
            meshGeometry3D.TriangleIndices.Add(5);
            meshGeometry3D.TriangleIndices.Add(1);

            meshGeometry3D.TriangleIndices.Add(7);
            meshGeometry3D.TriangleIndices.Add(3);
            meshGeometry3D.TriangleIndices.Add(2); // top
            meshGeometry3D.TriangleIndices.Add(2); //TWO TRIANGLES PER CUBE SIDE
            meshGeometry3D.TriangleIndices.Add(6);
            meshGeometry3D.TriangleIndices.Add(7); 

            meshGeometry3D.TriangleIndices.Add(1);
            meshGeometry3D.TriangleIndices.Add(5);
            meshGeometry3D.TriangleIndices.Add(4); // bottom
            meshGeometry3D.TriangleIndices.Add(4); //TWO TRIANGLES PER CUBE SIDE
            meshGeometry3D.TriangleIndices.Add(0);
            meshGeometry3D.TriangleIndices.Add(1); 

            GeometryModel3D geometryModel3D = new GeometryModel3D(meshGeometry3D, matColor);

            RotateTransform3D rotateTransform3D = new RotateTransform3D();

            Transform3DGroup myTransform3DGroup = new Transform3DGroup();

            //SO THAT THE MODELS DO NOT STAY STILL WHILE DOING TRANFORMATIONS WE ADD IT TO THE SAME TRANSFORMATION GROUP AS THE MAP

            myTransform3DGroup.Children.Add(rotateTransform3D);

            geometryModel3D.Transform = myTransform3DGroup;

            return geometryModel3D;
        }

        public static System.Windows.Point CreatePoint(double longitude, double latitude)
        {

            double oneLongitude = (data.MaxLon - data.MinLon) / 1175; // MAP WIDTH
            double oneLatitude = (data.MaxLat - data.MinLat) / 775; // MAP HEIGHT

            //VALUES BETWEEN THE CURRENT AND THE MINIMUM LAT/LON SCALED DOWN TO DRAWN MAP SIZE
            double x = Math.Round((longitude - data.MinLon) / oneLongitude) - 587.5; // XAML MAP DRAWN SIZE
            double y = Math.Round((latitude - data.MinLat) / oneLatitude) - 387.5; // XAML MAP DRAWN SIZE

            x = x - x % 15; //HOW FAR TO DRAW BETWEEN TWO X COORDS
            y = y - y % 15; //HOW FAR TO DRAW BETWEEN TWO Y COORDS

            System.Windows.Point point = new System.Windows.Point();
            point.X = x;
            point.Y = y;

            return point;
        }

        //HIT RESULTS

        private HitTestResultBehavior HTResult(System.Windows.Media.HitTestResult rawresult)
        {
            RayHitTestResult rayResult = rawresult as RayHitTestResult;

            if (rayResult != null) // IF THERE IS SOMETHING IN THE DIRECTION WE ARE TESTING
            {

                bool hitFlag = false;
                for (int i = 0; i < data.EntityGeo.Keys.Count(); i++)
                {
                    if (data.EntityGeo.Keys.ToList()[i] == rayResult.ModelHit) //POWER ENTITIES ARE STORED IN THE ENTITY GEO DICT
                    {
                        geometryModelHit = (GeometryModel3D)rayResult.ModelHit; //FILL IN THE HIT VARIABLE
                        hitFlag = true;
                        var entity = data.EntityGeo[geometryModelHit]; //SO WE CAN PULL DATA OUT

                        toolTip = new ToolTip();
                        toolTip.Content = "\tPOWER ENTITY:\nID: " + entity.Id.ToString() + "\nName: " + entity.Name + "\nType: " + entity.GetType().Name;
                        toolTip.Height = 80;
                        toolTip.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#424242"));
                        toolTip.Foreground = Brushes.White;
                        toolTip.IsOpen = true;
                        ToolTipService.SetPlacement(MainViewPort, System.Windows.Controls.Primitives.PlacementMode.Mouse); //SO THAT THE TOOLTIP IS DISPLAYED ON THE MOUSE
                        break;
                    }

                    else
                    {
                        toolTip.IsOpen = false;
                    }
                }

                if (!hitFlag)
                    geometryModelHit = null;

            }
            return HitTestResultBehavior.Stop;
        }

        //HIT RESULTS

    }
}
