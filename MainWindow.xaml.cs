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

using System.Xml.Serialization;
using System.IO;
using System.Windows.Threading;

namespace lab6OOP
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DispatcherTimer timer;

		private static List<Line> lines = new List<Line>();
		private static List<Ellipse> pointsOfStations = new List<Ellipse>();
		private static List<TextBlock> namesOfCities = new List<TextBlock>();


		public static DateTime TIME = new DateTime(2022, 12, 1, 0, 0, 0, 0);

		public const int charWidth = 7;
		public const int charHeight = 9;

		public const int dateTimePosX = 0;
		public const int dateTimePosY = 0;

		public const int TIME_STEP = 20;

		public const int lineWidth = 2;

		public static bool SIMULATION_STARTED = false;


		public MainWindow()
		{
			timer = new DispatcherTimer();
			timer.Tick += new EventHandler(Simulation);
			timer.Interval = new TimeSpan(0, 0, 1);

			InitializeComponent();
			LocoEditButton.IsEnabled = false;
			LocoRemoveButton.IsEnabled = false;

			WagonEditButton.IsEnabled = false;
			WagonRemoveButton.IsEnabled = false;
			
			wagonTabItem.IsEnabled = false;
			TrainsTabItem.IsEnabled = wagonTabItem.IsEnabled;
			scheduleTabItem.IsEnabled = wagonTabItem.IsEnabled;


			CustomColor.occupiedColors.Add(CustomColor.lineColor);
			CustomColor.occupiedColors.Add(CustomColor.textColor);
			CustomColor.occupiedColors.Add(CustomColor.whiteColor);

			Station.CreateStations();
			ListOfStations.InitAllStations(StationDataGrid);
			DrawRoutes(lineWidth, CustomColor.lineColor, visualizationCanvas);
			DrawStations(CustomColor.textColor, CustomColor.lineColor, visualizationCanvas);
			Visualize(visualizationCanvas, DateTimeLabel, TIME);
		}


		//---------------------------------[Локомотив]---------------------------------//
		private void locoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			LocoRemoveButton.IsEnabled = locoDataGrid.SelectedItem != null;
			locoComboBox.SelectedItem = null;

			if (!SIMULATION_STARTED) LocoEditButton.IsEnabled = locoDataGrid.SelectedItem != null;
			else LocoEditButton.IsEnabled = false;
		}

		private void LocoAddButton_Click(object sender, RoutedEventArgs e)
		{
			var appendLocomotive = new AppendLocomotive(this);
			appendLocomotive.Show();
			TrainsDataGrid.UpdateTrainsDataGrid(sender,e);
		}

		private void LocoEditButton_Click(object sender, RoutedEventArgs e)
		{
			var editLocomotive = new EditLocomotive(this);
			editLocomotive.Show();
			TrainsDataGrid.UpdateTrainsDataGrid(sender,e);
		}

		private void LocoRemoveButton_Click(object sender, RoutedEventArgs e)
		{
			var removeLocomotive = new ConfirmationOfDeletingLocomotive(this);
			removeLocomotive.Show();
		}

		public static Locomotive FindLocomotive(object comboBoxSelectedItem)
		{
			if (comboBoxSelectedItem != null)
			{
				string selectedItem = comboBoxSelectedItem.ToString();
				string ID = "";
				int index = 0;

				for (int i = 0; i < selectedItem.Length; i++)
					if (selectedItem[i] == 'I' && selectedItem[i + 1] == 'D')
					{
						index += i + 4;
						break;
					}

				for (; index < selectedItem.Length; index++)
					ID += selectedItem[index];

				foreach (var loco in ListOfLocomotives.locomotives)
					if (loco.ID == ID)
						return loco;
			}
			return null;
		}

		private void LocoRandomButton_Click(object sender, RoutedEventArgs e)
		{
			var randomLoco = Locomotive.CreateRandom(visualizationCanvas);
			ListOfLocomotives.Insert(randomLoco);

			locoDataGrid.ItemsSource = null;
			locoDataGrid.ItemsSource = ListOfLocomotives.locomotivesForDataGrid;

			List<string> itemsForComboBox = new List<string>();

			foreach (var loco in ListOfLocomotives.locomotivesForDataGrid)
			{
				string firstStation = loco.FirstStation;
				string lastStation = loco.LastStation;
				string description = $"№{loco.RouteNum} ({firstStation} - {lastStation}) ID: {loco.ID}";
				itemsForComboBox.Add(description);
			}

			locoComboBox.ItemsSource = itemsForComboBox;
			locoComboBox.SelectedItem = null;
			trainComboBox.ItemsSource = itemsForComboBox;
			trainComboBox.SelectedItem = null;
			
			wagonTabItem.IsEnabled = true;
			scheduleTabItem.IsEnabled = wagonTabItem.IsEnabled;
			TrainsTabItem.IsEnabled = wagonTabItem.IsEnabled;
			
			WagonAddButton.IsEnabled = false;
			WagonRandomButton.IsEnabled = WagonAddButton.IsEnabled;
			WagonsManyRandomButton.IsEnabled = WagonAddButton.IsEnabled;
			
			StationDataGrid.UpdateStationDataGrid(sender, e);
			TrainsDataGrid.UpdateTrainsDataGrid(sender, e);
			locoDataGrid.UpdateLocoDataGrid(sender, e);
			Visualize(visualizationCanvas, DateTimeLabel, TIME);
		}

		public void UpdateComboBoxes()
		{
			List<string> itemsForComboBox = new List<string>();

			foreach (var loco in ListOfLocomotives.locomotivesForDataGrid)
			{
				string firstStation = loco.FirstStation;
				string lastStation = loco.LastStation;
				string description = $"№{loco.RouteNum} ({firstStation} - {lastStation}) ID: {loco.ID}";
				itemsForComboBox.Add(description);
			}

			locoComboBox.ItemsSource = itemsForComboBox;
			trainComboBox.ItemsSource = itemsForComboBox;
		}


		//-----------------------------------[Вагон]-----------------------------------//
		private void wagonDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			WagonEditButton.IsEnabled = wagonDataGrid.SelectedItem != null;
			WagonRemoveButton.IsEnabled = wagonDataGrid.SelectedItem != null;
		}

		private void WagonAddButton_Click(object sender, RoutedEventArgs e)
		{
			var loco = FindLocomotive(locoComboBox.SelectedItem);
			if (loco.wagons.Count < loco.MaxWagonsAmount)
			{
				var appendWagonWindow = new AppendWagon(this);
				appendWagonWindow.Show();
			}
			else
			{
				MessageBox.Show("Достигнуто максимальное количество вагонов для локомотива!");
			}
		}

		private void WagonEditButton_Click(object sender, RoutedEventArgs e)
		{
			var editWagonWindow = new EditWagon(this);
			editWagonWindow.Show();
		}

		private void WagonRemoveButton_Click(object sender, RoutedEventArgs e)
		{
			var removeWagon = new ConfirmationOfDeletingWagon(this);
			removeWagon.Show();
		}

		private void locoComboBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			var loco = FindLocomotive(locoComboBox.SelectedItem);
			if (loco != null)
			{

				WagonAddButton.IsEnabled = true;
				ListOfWagons.wagonsForDataGrid = ListOfWagons.ConvertToListForDataGrid(loco.wagons);
			}
			else
			{
				WagonAddButton.IsEnabled = false;
				ListOfWagons.wagonsForDataGrid = new List<WagonForDataGrid>();
			}
			WagonRandomButton.IsEnabled = WagonAddButton.IsEnabled;
			WagonsManyRandomButton.IsEnabled = WagonAddButton.IsEnabled;
			wagonDataGrid.ItemsSource = ListOfWagons.wagonsForDataGrid;
		}

		private void WagonRandomButton_Click(object sender, RoutedEventArgs e)
		{
			var loco = FindLocomotive(locoComboBox.SelectedItem);
			if (loco.wagons.Count < loco.MaxWagonsAmount)
			{
				var selectedLoco = FindLocomotive(locoComboBox.SelectedItem);
				var randomWagon = Passenger.CreateRandom(selectedLoco);

				ListOfWagons.Insert(randomWagon, selectedLoco);

				wagonDataGrid.ItemsSource = null;
				wagonDataGrid.ItemsSource = ListOfWagons.wagonsForDataGrid;
			}
			else
			{
				MessageBox.Show("Достигнуто максимальное количество вагонов для локомотива!");
			}
			TrainsDataGrid.UpdateTrainsDataGrid(sender,e);
			Visualize(visualizationCanvas, DateTimeLabel, TIME);
		}

		private void WagonsManyRandomButton_Click(object sender, RoutedEventArgs e)
		{
			var loco = FindLocomotive(locoComboBox.SelectedItem);

			if (loco.wagons.Count < loco.MaxWagonsAmount)
			{
				var randomWagons = Train.CreateRandom(loco);

				for (int i = 0; i < randomWagons.Length; i++)
				{
					if (loco.wagons.Count < loco.MaxWagonsAmount)
						ListOfWagons.Insert(randomWagons[i], loco);
					else
					{
						MessageBox.Show("Достигнуто максимальное количество вагонов для локомотива!");
						break;
					}
				}
				ListOfWagons.FixUpIndexes(loco);
				wagonDataGrid.ItemsSource = null;
				wagonDataGrid.ItemsSource = ListOfWagons.wagonsForDataGrid;
			}
			else
			{
				MessageBox.Show("Достигнуто максимальное количество вагонов для локомотива!");
			}

			TrainsDataGrid.UpdateTrainsDataGrid(sender,e);
			Visualize(visualizationCanvas, DateTimeLabel, TIME);
		}


		//-------------------------------[Сериализация]--------------------------------//
		/// <summary>
		/// Сериализация списков
		/// typeNameForErrorMsg - имя типа данных списка в родительном падеже
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="typeNameForErrorMsg"></param>
		private void Serialize<T>(T list, string typeNameForErrorMsg, string fileName)
		{
			XmlSerializer xml = new XmlSerializer(typeof(T));

			DateTime nowDateTime = DateTime.Now;

			string year =   (nowDateTime.Year < 10)   ? "0" + nowDateTime.Year.ToString()   : nowDateTime.Year.ToString();
			string month =  (nowDateTime.Month < 10)  ? "0" + nowDateTime.Month.ToString()  : nowDateTime.Month.ToString();
			string day =    (nowDateTime.Day < 10)    ? "0" + nowDateTime.Day.ToString()    : nowDateTime.Day.ToString();
			string hour =   (nowDateTime.Hour < 10)   ? "0" + nowDateTime.Hour.ToString()   : nowDateTime.Hour.ToString();
			string minute = (nowDateTime.Minute < 10) ? "0" + nowDateTime.Minute.ToString() : nowDateTime.Minute.ToString();
			string second = (nowDateTime.Second < 10) ? "0" + nowDateTime.Second.ToString() : nowDateTime.Second.ToString();

			string date = $"{day}.{month}.{year}";
			string time = $"{hour}.{minute}.{second}";

			string nameOfFile = $"[{date} {time}] {fileName}.xml";

			using (FileStream fs = new FileStream(nameOfFile, FileMode.Create))
			{
				try
				{
					xml.Serialize(fs, list);
					MessageBox.Show("Сериализация прошла успешно!");
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Не удалось сериализовать список {typeNameForErrorMsg}.\nОписание ошибки:\n{ex.Message}");
				}
			}
		}

		private void LocoSerializeButton_Click(object sender, RoutedEventArgs e)
		{
			List<LocoForDataGrid> locomotives = ListOfLocomotives.locomotivesForDataGrid;
			Serialize(locomotives, "локомотивов", "Локомотивы");
		}

		private void WagonSerializeButton_Click(object sender, RoutedEventArgs e)
		{
			List<WagonForDataGrid> wagons = ListOfWagons.wagonsForDataGrid;
			Serialize(wagons, "вагонов", "Вагоны");
		}

		private void TrainSerializeButton_Click(object sender, RoutedEventArgs e)
		{
			List<TrainForDataGrid> trains = ListOfTrains.Trains;
			Serialize(trains, "поездов", "Поезда");
		}

		private void StationSerializeButton_Click(object sender, RoutedEventArgs e)
		{
			List<StationForDataGrid> stations = ListOfStations.stationsForDataGrid;
			Serialize(stations, "станций", "Станции");
		}

		//--------------------------------[Расписание]---------------------------------//
		private void trainComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var loco = FindLocomotive(trainComboBox.SelectedItem);
			if (loco != null)
			{
				try
				{
					scheduleDataGrid.UpdateScheduleDataGrid(this, loco, TIME_STEP);
					scheduleDataGrid.ItemsSource = null;
					scheduleDataGrid.ItemsSource = Schedule.stations;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Чё-та не вышло :/");
				}
			}
		}

		//-------------------------------[Визуализация]--------------------------------//
		private static void DrawStations(SolidColorBrush textColor,
			SolidColorBrush pointColor, Canvas visualizationCanvas)
		{
			foreach (var station in Station.stations)
			{
				Point point = new Point(station.x, station.y); //координаты точки
				int borderWidth = Locomotive.borderThickness;
				int radius = Locomotive.pointRadius + 2;

				//------------------------------------[Точка]------------------------------------//
				Ellipse ellipse = new Ellipse();
				if (station.Name == "Москва")
					borderWidth++;

				radius += borderWidth;
				ellipse.Width = radius * 2;
				ellipse.Height = radius * 2;
				ellipse.StrokeThickness = Locomotive.pointRadius;
				ellipse.Fill = CustomColor.whiteColor;
				ellipse.Stroke = pointColor;
				ellipse.StrokeThickness = borderWidth;
				ellipse.Margin = new Thickness(point.X - radius, point.Y - radius, 0, 0);

				pointsOfStations.Add(ellipse);
				visualizationCanvas.Children.Add(ellipse);
			}
		}

		private static void DrawStationsNames(Canvas visualizationCanvas)
		{
			foreach (var station in Station.stations)
			{

				//-----------------------------------[Надпись]-----------------------------------//

				int borderWidth = Locomotive.borderThickness;
				TextBlock cityName = new TextBlock();

				cityName.Text = station.Name;

				double lengthInPixels = charWidth * station.Name.Length;

				double posX = station.x - lengthInPixels / 2;
				double posY = station.y - charHeight * 2 - (Locomotive.pointRadius + 2 + borderWidth) * 2;

				if (station.Name == "Москва")
				{
					cityName.FontWeight = FontWeights.Bold;
					borderWidth += 1;

				}

				cityName.Margin = new Thickness(posX, posY, 0, 0);
				cityName.Foreground = CustomColor.textColor;
				cityName.Background = CustomColor.whiteColor;
				
				visualizationCanvas.Children.Add(cityName);
			}
		}

		private static void DrawRoutes(int lineWidth, SolidColorBrush color, Canvas visualizationCanvas)
		{
			for (int i = 0; i < Station.routesAmount; i++)
			{
				var route = Station.GetRoute(i + 1);
				for (int j = 1; j < route.Length; j++)
				{
					Line line = new Line();

					line.X1 = route[j - 1].x;
					line.Y1 = route[j - 1].y;
					line.X2 = route[j].x;
					line.Y2 = route[j].y;

					line.Stroke = color;
					visualizationCanvas.Children.Add(line);
					lines.Add(line);
				}
			}
		}

		public void Simulation(object sender, EventArgs e)
		{
			TIME = TIME.AddMinutes(TIME_STEP);
			Visualize(visualizationCanvas, DateTimeLabel, TIME);
			for (int i = 0; i < ListOfLocomotives.locomotives.Count; i++)
			{
				if (!ListOfLocomotives.locomotives[i].OnTheWay)
				{
					ListOfLocomotives.locomotives[i].GoToNextStation(TIME_STEP, this);
					if (ListOfLocomotives.locomotives[i].timeOfStart == null) ListOfLocomotives.locomotives[i].timeOfStart = TIME;
				}
			}
		}

		public static void DrawDateTime(Label DateTimeLabel, DateTime dateTime)
		{
			DateTimeLabel.Content = dateTime.ToString();
			DateTimeLabel.Foreground = CustomColor.textColor;
			DateTimeLabel.Background = CustomColor.whiteColor;
		}

		public static void Visualize(Canvas visualizationCanvas, Label DateTimeLabel, DateTime dateTime)
		{
			DrawDateTime(DateTimeLabel, dateTime);
			DrawStationsNames(visualizationCanvas);
		}
		
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			SIMULATION_STARTED = true;
			LocoEditButton.IsEnabled = false;
			timer.Start();
		}

		private void TrainsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			TrainForDataGrid trainForDataGrid = (TrainForDataGrid)e.Row.DataContext;
			e.Row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(trainForDataGrid.GetColor()));
			e.Row.Foreground = CustomColor.whiteColor;
		}

	}
}
