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
	public class StationForDataGrid
	{
		public string Name { get; set; }
		public string StoppedLocomotivesID { get; set; }

		private double X { get; set; }
		private double Y { get; set; }
		StationForDataGrid() { }

		public StationForDataGrid(Station station)
		{
			station.UpdateInfo();
			Name = station.Name;
			StoppedLocomotivesID = "";

			for (int i = 0; i < station.StoppedLocomotives.Count; i++)
			{
				var loco = station.StoppedLocomotives[i];
				StoppedLocomotivesID += loco.ID;
				if (i < station.StoppedLocomotives.Count - 1)
					StoppedLocomotivesID += ", ";
			}
			X = station.x;
			Y = station.y;
		}

		public void UpdateData(Station station)
		{
			station.UpdateInfo();
			Name = station.Name;
			StoppedLocomotivesID = "";

			for (int i = 0; i < station.StoppedLocomotives.Count; i++)
			{
				var loco = station.StoppedLocomotives[i];
				StoppedLocomotivesID += loco.ID;
				if (i < station.StoppedLocomotives.Count - 1)
					StoppedLocomotivesID += ", ";
			}
			X = station.x;
			Y = station.y;
		}
	}

	[Serializable]
	public class ListOfStations
	{
		public static List<StationForDataGrid> stationsForDataGrid = new List<StationForDataGrid>();

		public static void InitAllStations(DataGrid StationDataGrid)
		{
			stationsForDataGrid.Clear();
			foreach(var station in Station.stations)
				stationsForDataGrid.Add(new StationForDataGrid(station));
			
			StationDataGrid.ItemsSource = null;
			StationDataGrid.ItemsSource = stationsForDataGrid;
		}

		ListOfStations() { }
	}

	public static partial class DataGridUpdate
	{
		public static void UpdateStationDataGrid(this DataGrid StationDataGrid, object sender, EventArgs e)
		{
			for (int i = 0; i < Station.stations.Length; i++)
				ListOfStations.stationsForDataGrid[i].UpdateData(Station.stations[i]);

			StationDataGrid.ItemsSource = null;
			StationDataGrid.ItemsSource = ListOfStations.stationsForDataGrid;
		}
	}
}
