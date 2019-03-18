using OrderDemo.Common;
using OrderDemo.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderDemo.Views
{
    /// <summary>
    /// UCOrderExterior.xaml 的交互逻辑
    /// </summary>
    public partial class UCOrderExterior : UserControl
    {
        //瓶子外观模式（exterior mode）分为默认模式（default mode）和自定义模式（custom mode）
        private bool _exteriorMode = true;//true为默认模式，false为自定义模式
        public bool ExteriorMode
        {
            get { return _exteriorMode; }
        }

        private List<Point3D> leftCurve;
        private List<Point3D> rightCurve;

        //button颜色  
        SolidColorBrush myBtnUnClickedBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0x19, 0x80, 0x80, 0x80));
        SolidColorBrush myBtnClickedBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x00, 0x86, 0xFF));
        public UCOrderExterior()
        {
            InitializeComponent();
            this.DataContext = UserViewModel.InstanceUserViewModel;

            this.sliderX2.AddHandler(Slider.MouseLeftButtonUpEvent, new MouseButtonEventHandler(sliderX2_MouseLeftButtonUp), true);
            this.sliderX3.AddHandler(Slider.MouseLeftButtonUpEvent, new MouseButtonEventHandler(sliderX3_MouseLeftButtonUp), true);
            this.sliderX4.AddHandler(Slider.MouseLeftButtonUpEvent, new MouseButtonEventHandler(sliderX4_MouseLeftButtonUp), true);

            DefaultOrCustomizeModeInitial();

            SetInitialValue();
        }

        private void DefaultOrCustomizeModeInitial()
        {
            btnDefaultMode.Background = (System.Windows.Media.Brush)myBtnClickedBrush;
            btnCustomizeMode.Background = (System.Windows.Media.Brush)myBtnUnClickedBrush;
            this.sliderX2.IsEnabled = false;
            this.sliderX3.IsEnabled = false;
            this.sliderX4.IsEnabled = false;
        }
        private void SetInitialValue()
        {
            leftCurve = new List<Point3D>();
            rightCurve = new List<Point3D>();
            leftCurve.Add(new Point3D(90 + this.sliderX1.Value, 0, 0));
            leftCurve.Add(new Point3D(90 + this.sliderX2.Value, 32, 0));
            leftCurve.Add(new Point3D(90 + this.sliderX3.Value, 64, 0));
            leftCurve.Add(new Point3D(90 + this.sliderX4.Value, 96, 0));
            leftCurve.Add(new Point3D(90 + this.sliderX5.Value, 128, 0));
            rightCurve.Add(new Point3D(322 - this.sliderX1.Value, 0, 0));
            rightCurve.Add(new Point3D(322 - this.sliderX2.Value, 32, 0));
            rightCurve.Add(new Point3D(322 - this.sliderX3.Value, 64, 0));
            rightCurve.Add(new Point3D(322 - this.sliderX4.Value, 96, 0));
            rightCurve.Add(new Point3D(322 - this.sliderX5.Value, 128, 0));
            this.canvasExterior.Children.Clear();
            DrawCurve(leftCurve, rightCurve);
        }
        private void DrawCurve(List<Point3D> leftCurvePara, List<Point3D> rightCurvePara)
        {
            this.canvasExterior.Children.Clear();
            System.Windows.Shapes.Path pathCurve = new System.Windows.Shapes.Path();
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new System.Windows.Point(90, 180);
            PathSegmentCollection segmentCollection = new PathSegmentCollection();
            List<Point3D> returnLeftPositions = BSplineCurves.DensifyCurve(leftCurvePara, 0.01);
            for (int i = 0; i < returnLeftPositions.Count; i++)
            {
                segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(returnLeftPositions[i].x, returnLeftPositions[i].y + 180) });
            }
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(90, 308) });
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(90, 335) });
            segmentCollection.Add(new ArcSegment(new System.Windows.Point(95, 340), new System.Windows.Size(5, 5), 90, false, SweepDirection.Counterclockwise, true));
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(317, 340) });
            segmentCollection.Add(new ArcSegment(new System.Windows.Point(322, 335), new System.Windows.Size(5, 5), 90, false, SweepDirection.Counterclockwise, true));
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(322, 308) });
            List<Point3D> returnRightPositions = BSplineCurves.DensifyCurve(rightCurvePara, 0.01);
            for (int j = returnRightPositions.Count - 1; j >= 0; j--)
            {
                segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(returnRightPositions[j].x, returnRightPositions[j].y + 180) });
            }
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(322, 180) });
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(322, 140) });
            segmentCollection.Add(new ArcSegment(new System.Windows.Point(306, 124), new System.Windows.Size(16, 16), 90, false, SweepDirection.Counterclockwise, true));
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(301, 124) });
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(306, 119) });
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(306, 84) });
            segmentCollection.Add(new ArcSegment(new System.Windows.Point(254, 60), new System.Windows.Size(24, 52), 90, false, SweepDirection.Counterclockwise, true));
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(158, 60) });
            segmentCollection.Add(new ArcSegment(new System.Windows.Point(106, 84), new System.Windows.Size(24, 52), 90, false, SweepDirection.Counterclockwise, true));
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(106, 119) });
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(111, 124) });
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(106, 124) });
            segmentCollection.Add(new ArcSegment(new System.Windows.Point(90, 140), new System.Windows.Size(16, 16), 90, false, SweepDirection.Counterclockwise, true));
            segmentCollection.Add(new LineSegment() { Point = new System.Windows.Point(90, 180) });
            pathFigure.Segments = segmentCollection;
            pathGeometry.Figures.Add(pathFigure);
            pathCurve.Data = pathGeometry;
            pathCurve.Fill = SnColorTemplate;
            pathCurve.Stroke = System.Windows.Media.Brushes.Black;
            pathCurve.StrokeThickness = 1;

            //绘制茶叶罐体内直线
            System.Windows.Shapes.Path pathCurveLine1 = new System.Windows.Shapes.Path();
            PathGeometry pathGeometryLine1 = new PathGeometry();
            PathFigure pathFigureLine1 = new PathFigure();
            pathFigureLine1.StartPoint = new System.Windows.Point(106, 84);
            PathSegmentCollection segmentCollectionLine1 = new PathSegmentCollection();
            segmentCollectionLine1.Add(new LineSegment() { Point = new System.Windows.Point(306, 84) });
            pathFigureLine1.Segments = segmentCollectionLine1;
            pathGeometryLine1.Figures.Add(pathFigureLine1);
            pathGeometryLine1.Figures = new PathFigureCollection() { pathFigureLine1 };
            pathCurveLine1.Data = pathGeometryLine1;
            pathCurveLine1.Stroke = System.Windows.Media.Brushes.Black;
            pathCurveLine1.StrokeThickness = 1;

            System.Windows.Shapes.Path pathCurveLine2 = new System.Windows.Shapes.Path();
            PathGeometry pathGeometryLine2 = new PathGeometry();
            PathFigure pathFigureLine2 = new PathFigure();
            pathFigureLine2.StartPoint = new System.Windows.Point(106, 119);
            PathSegmentCollection segmentCollectionLine2 = new PathSegmentCollection();
            segmentCollectionLine2.Add(new LineSegment() { Point = new System.Windows.Point(306, 119) });
            pathFigureLine2.Segments = segmentCollectionLine2;
            pathGeometryLine2.Figures.Add(pathFigureLine2);
            pathGeometryLine2.Figures = new PathFigureCollection() { pathFigureLine2 };
            pathCurveLine2.Data = pathGeometryLine2;
            pathCurveLine2.Stroke = System.Windows.Media.Brushes.Black;
            pathCurveLine2.StrokeThickness = 1;

            System.Windows.Shapes.Path pathCurveLine3 = new System.Windows.Shapes.Path();
            PathGeometry pathGeometryLine3 = new PathGeometry();
            PathFigure pathFigureLine3 = new PathFigure();
            pathFigureLine3.StartPoint = new System.Windows.Point(111, 124);
            PathSegmentCollection segmentCollectionLine3 = new PathSegmentCollection();
            segmentCollectionLine3.Add(new LineSegment() { Point = new System.Windows.Point(301, 124) });
            pathFigureLine3.Segments = segmentCollectionLine3;
            pathGeometryLine3.Figures.Add(pathFigureLine3);
            pathGeometryLine3.Figures = new PathFigureCollection() { pathFigureLine3 };
            pathCurveLine3.Data = pathGeometryLine3;
            pathCurveLine3.Stroke = System.Windows.Media.Brushes.Black;
            pathCurveLine3.StrokeThickness = 1;

            this.canvasExterior.Children.Add(pathCurve);
            this.canvasExterior.Children.Add(pathCurveLine1);
            this.canvasExterior.Children.Add(pathCurveLine2);
            this.canvasExterior.Children.Add(pathCurveLine3);
        }
        private void sliderX2_MouseLeftButtonUp(object o, MouseButtonEventArgs e)
        {
            leftCurve[1].x = 90 + this.sliderX2.Value;
            rightCurve[1].x = 322 - this.sliderX2.Value;
            DrawCurve(leftCurve, rightCurve);
        }
        private void sliderX3_MouseLeftButtonUp(object o, MouseButtonEventArgs e)
        {
            leftCurve[2].x = 90 + this.sliderX3.Value; ;
            rightCurve[2].x = 322 - this.sliderX3.Value;
            DrawCurve(leftCurve, rightCurve);
        }
        private void sliderX4_MouseLeftButtonUp(object o, MouseButtonEventArgs e)
        {
            leftCurve[3].x = 90 + this.sliderX4.Value;
            rightCurve[3].x = 322 - this.sliderX4.Value;
            DrawCurve(leftCurve, rightCurve);
        }

        private void btnDefaultMode_Click(object sender, RoutedEventArgs e)
        {
            _exteriorMode = true;
            btnDefaultMode.Background = (System.Windows.Media.Brush)myBtnClickedBrush;
            btnCustomizeMode.Background = (System.Windows.Media.Brush)myBtnUnClickedBrush;
            this.sliderX2.Value = 0;
            this.sliderX3.Value = 0;
            this.sliderX4.Value = 0;
            this.sliderX2.IsEnabled = false;
            this.sliderX3.IsEnabled = false;
            this.sliderX4.IsEnabled = false;
            SetInitialValue();
        }
        private void btnCustomizeMode_Click(object sender, RoutedEventArgs e)
        {
            _exteriorMode = false;
            btnDefaultMode.Background = (System.Windows.Media.Brush)myBtnUnClickedBrush;
            btnCustomizeMode.Background = (System.Windows.Media.Brush)myBtnClickedBrush;
            this.sliderX2.IsEnabled = true;
            this.sliderX3.IsEnabled = true;
            this.sliderX4.IsEnabled = true;
        }
    }
}
