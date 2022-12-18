using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab6OOP
{
	public class ScheduleOfTrain
	{
		public string StationName { get; set; }
		/// <summary>
		/// Время прибытия
		/// </summary>
		public string ComingTime { get; set; }
		/// <summary>
		/// Время отбытия
		/// </summary>
		public string DepartureTime { get; set; }

		public ScheduleOfTrain(string StationName, string ComingTime, string DepartureTime)
		{
			this.StationName =   StationName;
			this.ComingTime =    ComingTime;
			this.DepartureTime = DepartureTime;
		}
	}

	public class Schedule
	{
		public static List<ScheduleOfTrain> stations = new List<ScheduleOfTrain>();
	}
	partial class DataGridUpdate
	{
		public static void UpdateScheduleDataGrid(this DataGrid scheduleDataGrid, MainWindow window, Locomotive locomotive, double timeOfStep)
		{
			if (MainWindow.FindLocomotive(window.trainComboBox.SelectedItem) == locomotive)
			{

				Schedule.stations.Clear();

				ScheduleOfTrain scheduleRowFirst = new ScheduleOfTrain(locomotive.Route[0].Name, $"-", $"{locomotive.timeOfStart}");

				DateTime temp = locomotive.timeOfStart;

				Schedule.stations.Add(scheduleRowFirst);
				for (int i = 1; i < locomotive.Route.Length; i++)
				{
					var prevStation = locomotive.Route[i - 1];
					var curStation = locomotive.Route[i];
					var distance = DrawTrain.GetDistanceKM(curStation.x, curStation.y, prevStation.x, prevStation.y);
					var time = locomotive.GetWayTime(timeOfStep, distance);
					temp = temp.AddMinutes(time * timeOfStep);

					DateTime temp1 = temp.AddMinutes(timeOfStep);

					ScheduleOfTrain scheduleRow;
					if (i != locomotive.Route.Length - 1)
						scheduleRow = new ScheduleOfTrain(curStation.Name, $"{temp}", $"{temp1}");
					else
						scheduleRow = new ScheduleOfTrain(curStation.Name, $"{temp}", $"-");
					temp = temp1;
					Schedule.stations.Add(scheduleRow);
				}
				window.scheduleDataGrid.ItemsSource = null;
				window.scheduleDataGrid.ItemsSource = Schedule.stations;
			}
		}
	}
}
