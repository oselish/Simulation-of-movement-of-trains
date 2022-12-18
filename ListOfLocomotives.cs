using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

using System.Xml.Serialization;
using System.IO;

namespace lab6OOP
{
	[Serializable]
	public class LocoForDataGrid
	{
		/// <summary>
		/// ID
		/// </summary>
		public string ID { get; set; }
		/// <summary>
		/// Номер маршрута
		/// </summary>
		public int RouteNum { get; set; }

		/// <summary>
		/// Начальная станция
		/// </summary>
		public string FirstStation { get; set; }

		/// <summary>
		/// Конечная станция
		/// </summary>
		public string LastStation { get; set; }

		/// <summary>
		/// Текущая станция
		/// </summary>
		//Текущая станция
		public string CurrentStation { get; set; }

		/// <summary>
		/// Скорость
		/// </summary>
		public double Speed { get; set; }

		/// <summary>
		/// Макс. скорость
		/// </summary>
		public double MaxSpeed { get; set; }

		/// <summary>
		/// Тип двигателя
		/// </summary>
		public string EngineType { get; set; }

		/// <summary>
		/// Происшествие
		/// </summary>
		public string Incident { get; set; }
		/// <summary>
		/// Маршрут
		/// </summary>
		public string Route { get; set; }


		public LocoForDataGrid(Locomotive locomotive)
		{
			ID = locomotive.ID;
			RouteNum = locomotive.RouteNum;
			FirstStation = locomotive.Route[0].Name;
			LastStation = locomotive.Route[locomotive.Route.Length - 1].Name;
			CurrentStation = locomotive.CurrentStation.Name;
			Speed = locomotive.Speed;
			MaxSpeed = locomotive.MaxSpeed;
			EngineType = locomotive.EngineType;

			Incident = locomotive.Incident;
			Route = "";

			for (int i = 0; i < locomotive.Route.Length; i++)
			{
				Route += locomotive.Route[i].Name;

				if (i < locomotive.Route.Length - 1)
					Route += ", ";
			}
		}

		public void UpdateData(Locomotive locomotive)
		{
			ID = locomotive.ID;
			RouteNum = locomotive.RouteNum;
			FirstStation = locomotive.Route[0].Name;
			LastStation = locomotive.Route[locomotive.Route.Length - 1].Name;
			CurrentStation = locomotive.CurrentStation.Name;
			Speed = locomotive.Speed;
			MaxSpeed = locomotive.MaxSpeed;
			EngineType = locomotive.EngineType;

			Incident = locomotive.Incident;
			Route = "";
			for (int i = 0; i < locomotive.Route.Length; i++)
			{
				Route += locomotive.Route[i].Name;

				if (i < locomotive.Route.Length - 1)
					Route += ", ";
			}
		}

		LocoForDataGrid() { }
	}


	[Serializable]
	public class ListOfLocomotives
	{
		public static List<Locomotive> locomotives = new List<Locomotive>();

		public static List<LocoForDataGrid> locomotivesForDataGrid = new List<LocoForDataGrid>();

		public static void Insert(Locomotive locomotive)
		{
			locomotives.Add(locomotive);
			locomotivesForDataGrid.Add(new LocoForDataGrid(locomotive));
			ListOfTrains.Trains.Add(new TrainForDataGrid(locomotive));
		}

		public static object[] Search(string ID)
		{
			object[] result = new object[2];
			for (int i = 0; i < locomotivesForDataGrid.Count; i++)
			{
				var locoFDG = locomotivesForDataGrid[i];
				var loco = locomotives[i];
				if (loco.ID == ID)
				{
					result[0] = loco;
					result[1] = locoFDG;
					return result;
				}
			}
			return null;
		}

		public static bool Remove(string ID, Canvas visualizationCanvas)
		{
			for (int i = 0; i < locomotivesForDataGrid.Count; i++)
			{
				var locoFDG = locomotivesForDataGrid[i];
				var loco = locomotives[i];
				if (loco.ID == ID)
				{
					//удаление из списков
					//locomotives.Remove(loco);
					//locomotivesForDataGrid.Remove(locoFDG);

					//for (int j = 0; j < visualizationCanvas.Children.Count;j++)
					//{
						//Ellipse locoPoint = loco.point;
						//var lcp = loco.point.
						//Ellipse pointOnCanvas = visualizationCanvas.Children[i] as Ellipse;
						
						//if (pointOnCanvas != null)// && visualizationCanvas.Children[i] is Ellipse) //ПРОВЕРКА НА НАЛИЧИЕ НА КАНВАСЕ
						//if (visualizationCanvas.Children[i] is Ellipse)// && visualizationCanvas.Children[i] is Ellipse) //ПРОВЕРКА НА НАЛИЧИЕ НА КАНВАСЕ
						//{
							//MessageBox.Show("Это точка!");
							//if (pointOnCanvas.Fill == loco.point.Fill)
							//{
								bool result1 = locomotives.Remove(loco);
								bool result2 = locomotivesForDataGrid.Remove(locoFDG);
								visualizationCanvas.Children.Remove(loco.point);
								return result1 && result2;
							//}
						//}
					//}
				}
			}
			return false;
		}

		public static void Sort()
		{
			locomotives.Sort();
			locomotivesForDataGrid.Sort();
		}

		public static bool Replace(string ID, Locomotive newLocomotive)
		{
			for (int i = 0; i < locomotivesForDataGrid.Count; i++)
			{
				var locoFDG = locomotivesForDataGrid[i];
				var loco = locomotives[i];
				if (loco.ID == ID)
				{
					locomotives[i] = newLocomotive;
					locomotivesForDataGrid[i] = new LocoForDataGrid(newLocomotive);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Проекрка уникальности ID
		/// </summary>
		public static bool IDisUnique(string ID)
		{
			foreach (var loco in locomotives)
				if (ID == loco.ID)
					return false;
			return true;
		}

		ListOfLocomotives() { }
	}

	public static partial class DataGridUpdate
	{
		public static void UpdateLocoDataGrid(this DataGrid locoDataGrid, object sender, EventArgs e)
		{
			for (int i = 0; i < ListOfLocomotives.locomotives.Count; i++)
				ListOfLocomotives.locomotivesForDataGrid[i].UpdateData(ListOfLocomotives.locomotives[i]);

			locoDataGrid.ItemsSource = null;
			locoDataGrid.ItemsSource = ListOfLocomotives.locomotivesForDataGrid;
		}
	}
}

