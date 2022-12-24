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
	public class TrainForDataGrid
	{
		private string ColorOnMap;
		public string LocomotiveID { get; set; }
		public int RouteNum { get; set; }
		public string FirstStation { get; set; }
		public string LastStation { get; set; }
		public string CurrentStation { get; set; }
		public string NextStation { get; set; }
		public string LateTime { get; set; }
		public string Speed { get; set; }
		public int WagonsAmount { get; set; }
		public int PassengersAmount { get; set; }
		public string Incident { get; set; }
		public string Route { get; set; }

		public void SetColor(string color)
		{
			ColorOnMap = color;
		}

		public string GetColor()
		{
			return ColorOnMap;
		}

		public TrainForDataGrid(Locomotive locomotive)
		{
			LocomotiveID = locomotive.ID;
			FirstStation = locomotive.Route[0].Name;
			LastStation = locomotive.Route[locomotive.Route.Length - 1].Name;
			CurrentStation = locomotive.CurrentStation.Name;
			NextStation = (locomotive.GetNextStation() != null) ? locomotive.GetNextStation().Name : "-";
			LateTime = $"{locomotive.LateTime} мин.";
			WagonsAmount = locomotive.wagons.Count;
			PassengersAmount = 0;
			Speed = $"{locomotive.Speed} км/ч";
			SetColor(locomotive.pointColor.ToString());
			
			foreach(var wagon in locomotive.wagons)
				PassengersAmount += wagon.Capacity;

			Route = "";

			for (int i = 0; i < locomotive.Route.Length; i++)
			{
				Route += locomotive.Route[i].Name;
				if (i < locomotive.Route.Length - 1) Route += ", ";
			}

			RouteNum = locomotive.RouteNum;
			Incident = locomotive.Incident;
		}
		TrainForDataGrid() { }

		public void UpdateData(Locomotive locomotive)
		{
			LocomotiveID = locomotive.ID;
			FirstStation = locomotive.Route[0].Name;
			LastStation = locomotive.Route[locomotive.Route.Length - 1].Name;
			CurrentStation = locomotive.CurrentStation.Name;
			NextStation = (locomotive.GetNextStation() != null) ? locomotive.GetNextStation().Name : "-";
			LateTime = $"{locomotive.LateTime} мин.";
			WagonsAmount = locomotive.wagons.Count;
			PassengersAmount = 0;
			Speed = $"{locomotive.Speed} км/ч";
			if (locomotive.pointColor != null) SetColor(locomotive.pointColor.ToString());

			foreach (var wagon in locomotive.wagons)
				PassengersAmount += wagon.Capacity;

			Route = "";

			for (int i = 0; i < locomotive.Route.Length; i++)
			{
				Route += locomotive.Route[i].Name;
				if (i < locomotive.Route.Length - 1) Route += ", ";
			}

			RouteNum = locomotive.RouteNum;
			Incident = locomotive.Incident;
		}
	}

	[Serializable]
	public class ListOfTrains
	{
		public static List<TrainForDataGrid> Trains = new List<TrainForDataGrid>();
		ListOfTrains() { }
	}

	public static partial class DataGridUpdate
	{
		public static void UpdateTrainsDataGrid(this DataGrid TrainsDataGrid, object sender, EventArgs e)
		{
			for (int i = 0; i < ListOfTrains.Trains.Count; i++)
			{
				if (i >= ListOfLocomotives.locomotives.Count)
					ListOfTrains.Trains.RemoveAt(i);
				else
				{
					var loco = ListOfLocomotives.locomotives[i];
					ListOfTrains.Trains[i].UpdateData(loco);
				}
			}
			TrainsDataGrid.ItemsSource = null;
			TrainsDataGrid.ItemsSource = ListOfTrains.Trains;
		}
	}
}
