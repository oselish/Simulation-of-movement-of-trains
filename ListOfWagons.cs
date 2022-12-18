using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 

namespace lab6OOP
{
	public class WagonForDataGrid
	{
		public string ID { get; set; }
		public int SerialNum { get; set; }
		public string Type { get; set; }
		public int Capacity { get; set; }

		public WagonForDataGrid(Passenger passenger)
		{
			this.ID = passenger.ID;
			this.SerialNum = passenger.SerialNum + 1;
			this.Type = passenger.Type;
			this.Capacity = passenger.Capacity;
		}
		WagonForDataGrid() { }
	}

	[Serializable]
	public class ListOfWagons
	{
		public static List<Passenger> wagons = new List<Passenger>();

		public static List<WagonForDataGrid> wagonsForDataGrid = new List<WagonForDataGrid>();
		public static List<WagonForDataGrid> ConvertToListForDataGrid(List<Passenger> passengerWagons)
		{
			List<WagonForDataGrid> result = new List<WagonForDataGrid>();
			foreach (var wagon in passengerWagons)
				result.Add(new WagonForDataGrid(wagon));
			return result;
		}

		/// <summary>
		/// Добавление вагона
		/// </summary>
		/// <param name="passenger"></param>
		public static void Insert(Passenger passenger, Locomotive locomotive)
		{
			locomotive.wagons.Add(passenger);
			wagons = locomotive.wagons;
			wagonsForDataGrid = ConvertToListForDataGrid(locomotive.wagons);
		}

		/// <summary>
		/// Поиск вагона. Первый индекс - вагон, второй - вагон для таблицы
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public static object[] Search(string ID)
		{
			object[] result = new object[2];
			for (int i = 0; i < wagonsForDataGrid.Count; i++)
			{
				var wagonFDG = wagonsForDataGrid[i];
				var wagon = wagons[i];
				if (wagon.ID == ID)
				{
					result[0] = wagon;
					result[1] = wagonFDG;
					return result;
				}
			}
			return null;
		}

		public static bool Remove(string ID, Locomotive locomotive)
		{
			for (int i = 0; i < wagonsForDataGrid.Count; i++)
			{
				var wagonFDG = wagonsForDataGrid[i];
				var wagon = wagons[i];
				if (wagon.ID == ID)
				{
					wagons.Remove(wagon);
					wagonsForDataGrid.Remove(wagonFDG);
					locomotive.wagons = wagons;
					return true;
				}
			}
			return false;
		}

		public static void Sort()
		{
			wagons.Sort();
			wagonsForDataGrid.Sort();
		}

		public static void FixUpIndexes(Locomotive locomotive)
		{
			for (int i = 0; i < wagonsForDataGrid.Count; i++)
			{
				locomotive.wagons[i].SerialNum = i;
			}
			wagons = locomotive.wagons;
			wagonsForDataGrid = ConvertToListForDataGrid(locomotive.wagons);
		}

		public static bool Replace(string ID, Passenger newWagon)
		{
			for (int i = 0; i < wagonsForDataGrid.Count; i++)
			{
				var wagonFDG = wagonsForDataGrid[i];
				var wagon = wagons[i];
				if (wagon.ID == ID)
				{
					wagons[i] = newWagon;
					wagonsForDataGrid[i] = new WagonForDataGrid(newWagon);
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
			foreach (var wagon in wagons)
				if (ID == wagon.ID)
					return false;
			return true;
		}

		ListOfWagons() { }
	}
	/*
	internal class ListOfWagons
	{
		//public static List<Wagon> wagons           = new List<Wagon>();
		//public static List<Station> stations       = new List<Station>();
		//public static List<Wagon[]> trains         = new List<Wagon[]>();
		//public static List<WagonForDataGrid> wagonsForDataGrid           = new List<Wagon>();
		//public static List<StationForDataGrid> stationsForDataGrid       = new List<Station>();
		//public static List<WagonForDataGrid[]> trainsForDataGrid         = new List<Wagon[]>();
	}
	 */
}
