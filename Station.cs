using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6OOP
{
	/// <summary>
	/// Ж/Д станция
	/// </summary>

	public class Station
	{
		public const int offsetX = -50;
		public const int offsetY = -50;
		public const int routesAmount = 8;

		public static string[] stationNames =
		{
			"Екатеринбург",	   // Индекс 0
			"Казань",		   // Индекс 1 
			"Красноярск",	   // Индекс 2 
			"Москва",		   // Индекс 3 
			"Нижний Новгород", // Индекс 4 
			"Новосибирск",	   // Индекс 5 
			"Омск",			   // Индекс 6 
			"Оренбург",		   // Индекс 7 
			"Пермь",		   // Индекс 8 
			"Самара",		   // Индекс 9 
			"Санкт-Петербург", // Индекс 10 
			"Сургут",		   // Индекс 11 
			"Тюмень",		   // Индекс 12 
			"Уфа",			   // Индекс 13
			"Челябинск"		   // Индекс 14 
		};

		public static Station[] stations;

		public static string[][] routes = new string[][]
		{
			new string[] { "Оренбург", "Самара", "Казань", "Нижний Новгород", "Москва" },
			new string[] { "Москва", "Нижний Новгород", "Казань", "Самара", "Оренбург" },
			new string[] { "Санкт-Петербург", "Москва", "Нижний Новгород", "Казань", "Пермь", "Екатеринбург" },
			new string[] { "Екатеринбург", "Пермь", "Казань", "Нижний Новгород", "Москва", "Санкт-Петербург" },
			new string[] { "Оренбург", "Уфа", "Челябинск", "Екатеринбург", "Тюмень", "Омск", "Новосибирск", "Красноярск" },
			new string[] { "Красноярск", "Новосибирск", "Омск", "Тюмень", "Екатеринбург", "Челябинск", "Уфа", "Оренбург" },
			new string[] { "Оренбург", "Уфа", "Челябинск", "Екатеринбург", "Сургут", "Новосибирск" },
			new string[] { "Новосибирск", "Сургут", "Екатеринбург", "Челябинск", "Уфа", "Оренбург" }

		};

		//Генерация всех станций
		public static void CreateStations()
		{
			stations = new Station[stationNames.Length];
			for (int i = 0; i < stationNames.Length; i++)
			{
				stations[i] = new Station(stationNames[i]);
			}
		}

		/// <summary>
		/// Порядковый номер станции
		/// </summary>
		private int serialNum;
		public int SerialNum { get; set; }

		/// <summary>
		/// Поезд на станции
		/// </summary>
		public List<Locomotive> StoppedLocomotives { get; set; }

		~Station() { }
		Station() { }
		public void UpdateInfo()
		{
			StoppedLocomotives = new List<Locomotive>();
			foreach (var loco in ListOfLocomotives.locomotives)
			{
				if (loco.CurrentStation == this)
					StoppedLocomotives.Add(loco);
			}
		}

		/// <summary>
		/// Название станции
		/// </summary>
		private string name;
		public string Name { get; set; }

		/// <summary>
		/// Генерация маршрута
		/// </summary>
		/// <param Name="stations"></param>
		/// <param Name="namesOfStations"></param>
		//public static void GenerateRoute(Station[] stations, string[] namesOfStations)
		//{
		//	for (int i = 0; i < stations.Length; i++)
		//		stations[i] = new Station(namesOfStations[i]);
		//}

		private static Station[] GetStations(string[] Names)
		{
			Station[] result = new Station[Names.Length];
			int index = 0;

			for (int i = 0; i < Names.Length; i++)
			{
				for (int j = 0; j < stations.Length; j++)
				{
					if (Names[i] == stations[j].Name)
					{
						result[index] = stations[j];
						index++;
					}
				}
			}
			return result;
		}

		public static Station GetStation(string name)
		{
			for (int i = 0; i < stations.Length; i++)
				if (name == stations[i].Name)
					return stations[i];
			return null;
		}

		//Генерация маршрута по номеру маршрута
		public static Station[] GetRoute(int RouteNum)
		{
			Station[] tempRoute = GetStations(routes[RouteNum - 1]);
			return tempRoute;
		}

		/// <summary>
		/// Отображение маршрута
		/// </summary>
		/// <param Name="stations"></param>
		public static void ShowRoute(Station[] stations)
		{
			for (int i = 0; i < stations.Length; i++)
				Console.WriteLine($"{stations[i].SerialNum + 1}. {stations[i].Name}");
		}

		public Station(string Name, int SerialNum)
		{
			this.Name = Name;
			this.SerialNum = SerialNum;
		}

		public Station(string Name)
		{
			this.Name = Name;
			this.x = PositionsOfStations[Name].X;
			this.y = PositionsOfStations[Name].Y;
		}


		/// <summary>
		/// Получение информации о станции
		/// </summary>
		/// <returns></returns>
		public string GetInfo()
		{
			string info = "";
			string stoppedLocomotivesNums = ""; //номера маршрутов остановленныпоездов на станции
			int lastNum = StoppedLocomotives.Count - 1;
			for (int i = 0; i < lastNum; i++)
			{
				stoppedLocomotivesNums += StoppedLocomotives[i].SerialNum.ToString() + ", ";
			}
			stoppedLocomotivesNums += StoppedLocomotives[lastNum].SerialNum.ToString();
			info += $"Название станции: {Name}\nПорядковый номер станции: {SerialNum}\nПоезда на станции: {stoppedLocomotivesNums}\n";
			return info;
		}

		//-------------------------------------[Рисование]-------------------------------------//
		public double x;
		public double y;

		private static int[,] defaultPositions =
		{
			{830,275},
			{559,320},
			{1586,311},
			{289,332},
			{439,300},
			{1351,354},
			{1129,352},
			{700,482},
			{726,226},
			{583,426},
			{121,139},
			{1131,78},
			{942,263},
			{720,365},
			{850,347},
		};

		private static Dictionary<string, Point> PositionsOfStations = new Dictionary<string, Point>()
		{
			{ "Екатеринбург",      new Point(  830 + offsetX,  275 + offsetY) },
			{ "Казань",            new Point(  559 + offsetX,  320 + offsetY) },
			{ "Красноярск",        new Point( 1586 + offsetX,  311 + offsetY) },
			{ "Москва",            new Point(  289 + offsetX,  332 + offsetY) },
			{ "Нижний Новгород",   new Point(  439 + offsetX,  300 + offsetY) },
			{ "Новосибирск",       new Point( 1351 + offsetX,  354 + offsetY) },
			{ "Омск",              new Point( 1129 + offsetX,  352 + offsetY) },
			{ "Оренбург",          new Point(  700 + offsetX,  482 + offsetY) },
			{ "Пермь",             new Point(  726 + offsetX,  226 + offsetY) },
			{ "Самара",            new Point(  583 + offsetX,  426 + offsetY) },
			{ "Санкт-Петербург",   new Point(  121 + offsetX,  139 + offsetY) },
			{ "Сургут",            new Point( 1131 + offsetX,   78 + offsetY) },
			{ "Тюмень",            new Point(  942 + offsetX,  263 + offsetY) },
			{ "Уфа",               new Point(  720 + offsetX,  365 + offsetY) },
			{ "Челябинск",         new Point(  850 + offsetX,  347 + offsetY) }

		};
	}
}
