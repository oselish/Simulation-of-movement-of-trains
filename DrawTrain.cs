using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab6OOP
{
	public struct DT
	{
		public DateTime start;
		public DateTime now;
		public DateTime end;
	}

	public static class DrawTrain
	{

        public static List<Ellipse> pointsOfLocomotives = new List<Ellipse>();
        public static List<Ellipse> bordersOfPointsOfLocomotives = new List<Ellipse>();
        //-------------------------------------------------[Рисование]-------------------------------------------------//

		public static List<DT> DateTimeOfTrains = new List<DT>();

		private static MainWindow window;
		public static Ellipse point;

		public static double x1;
		public static double y1;

		public static void GoToNextStation(this Locomotive locomotive, double timeStep, MainWindow mainWindow)
		{
			window = mainWindow;
			for (int i = 0; i < window.visualizationCanvas.Children.Count; i++)
				if (window.visualizationCanvas.Children[i] == locomotive.point)
					point = window.visualizationCanvas.Children[i] as Ellipse;


			var nextStation = locomotive.GetNextStation();
			if (nextStation != null)
			{
				locomotive.LateTime = 0;
				locomotive.Speed = locomotive.MaxSpeed;
				locomotive.OnTheWay = true;

				Random random = new Random();
				int chanceOfLate = random.Next(0, 100);
				if (chanceOfLate <= 3) //шанс опаздания 5%
					locomotive.LateTime += random.Next(-15, 15)/60;

				locomotive.RandomIncident(timeStep);


				DoubleAnimation animX = new DoubleAnimation();
				DoubleAnimation animY = new DoubleAnimation();


				double x2 = nextStation.x - locomotive.x;
				double y2 = nextStation.y - locomotive.y;

				if (locomotive.CurrentStation == locomotive.FirstStation)
				{
					x1 = 0;
					y1 = 0;
				}
				else
				{
					x1 = locomotive.CurrentStation.x - locomotive.x;
					y1 = locomotive.CurrentStation.y - locomotive.y;
				}

				animX.From = x1;
				animY.From = y1;
				animX.To = x2;
				animY.To = y2;

				double timeOfStep = 60 / timeStep; // 3:1 масштаб времени

				var timeOfWay = locomotive.GetWayTime(timeStep, x1, y1, x2, y2) + locomotive.LateTime;

				locomotive.Speed = (int)(GetDistanceKM(locomotive.CurrentStation.x, locomotive.CurrentStation.y, nextStation.x, nextStation.y) / (timeOfWay/timeOfStep));

				animX.Duration = TimeSpan.FromSeconds(timeOfWay);
				animY.Duration = TimeSpan.FromSeconds(timeOfWay);
				animX.Completed += locomotive.AnimationCompleted;
				point.BeginAnimation(Canvas.LeftProperty, animX);
				point.BeginAnimation(Canvas.TopProperty, animY);

				window.locoDataGrid.UpdateLocoDataGrid(null, null);
				window.TrainsDataGrid.UpdateTrainsDataGrid(null, null);
				window.StationDataGrid.UpdateStationDataGrid(null, null);
			}
		}

		private static void AnimationCompleted(this Locomotive locomotive, object sender, EventArgs e)
		{
			locomotive.Incident = null;
			locomotive.OnTheWay = false;
			locomotive.Speed = 0;

			window.scheduleDataGrid.UpdateScheduleDataGrid(window, locomotive, MainWindow.TIME_STEP);
			window.locoDataGrid.UpdateLocoDataGrid(null,null);
			window.TrainsDataGrid.UpdateTrainsDataGrid(null,null);
			window.StationDataGrid.UpdateStationDataGrid(null,null);
			locomotive.CurrentStation = locomotive.GetNextStation();
			locomotive.NextStation = locomotive.GetNextStation();
		}

		public static double GetDistancePixels(double x1, double y1, double x2, double y2)
		{
			double x = Math.Abs(x2 - x1);
			double y = Math.Abs(y2 - y1);

			double distance = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
			return distance;
		}

		public static double GetWayTime(this Locomotive locomotive, double timeOfStep, double x1, double y1, double x2, double y2)
		{
			var distance = GetDistanceKM(x1, y1, x2, y2);

			double timeOfWay = distance / locomotive.MaxSpeed * (60/timeOfStep);

			return timeOfWay;
		}

		public static double GetWayTime(this Locomotive locomotive, double timeOfStep, double distance)
		{
			double timeOfWay = distance / locomotive.MaxSpeed * (60/timeOfStep) + (locomotive.LateTime / 60.0);

			return timeOfWay;
		}

		public static double GetDistanceKM(double x1, double y1, double x2, double y2)
		{
			Station MSK = Station.GetStation("Москва");
			Station SPB = Station.GetStation("Санкт-Петербург");

			double distanceMskSpbKM = 700;                                   //расстояние между Москвой и Санкт-Петербургом в км
			double distanceMskSpbPixels = GetDistancePixels(MSK.x, MSK.y, SPB.x, SPB.y); //расстояние между Москвой и Санкт-Петербургом в пикселях

			double KMinPixels = distanceMskSpbKM / distanceMskSpbPixels;     //кол-во километров в одном пикселе

			double distanceInPixels = GetDistancePixels(x1, y1, x2, y2);     //расстояние между точками в пикселях 
			double result = distanceInPixels * KMinPixels;                   //расстояние между точками в км 
			return result;
		}
	}
}