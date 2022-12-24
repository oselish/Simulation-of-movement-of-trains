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
using System.Windows.Shapes;

using System.Xml.Serialization;
using System.IO;

namespace lab6OOP
{
	/// <summary>
	/// Логика взаимодействия для SerializeWindow.xaml
	/// </summary>
	public partial class SerializeWindow : Window
	{
		private MainWindow window;
		private static bool serializeResult = true;
		private static bool deserializeResult = true;
		public SerializeWindow(MainWindow window)
		{
			this.window = window;
			InitializeComponent();
		}

		private static void Serialize<T>(T list)
		{
			XmlSerializer xml = new XmlSerializer(typeof(T));
			int len = typeof(T).ToString().Length;
			string nameOfFile = $"{typeof(T).ToString().Substring(41 + 1, len - 41 - 2)}.xml";

			using (FileStream fs = new FileStream(nameOfFile, FileMode.Create))
			{
				try
				{
					xml.Serialize(fs, list);
				}
				catch (Exception ex)
				{
					serializeResult = false;
					MessageBox.Show($"Не удалось сериализовать список {typeof(T)}.\n\nОписание ошибки:\n{ex.Message}\n\n{ex.StackTrace}");
					
				}
			}
		}

		private static T Deserialize<T>(T list)
		{
			XmlSerializer xml = new XmlSerializer(typeof(T));

			int len = typeof(T).ToString().Length;
			string nameOfFile = $"{typeof(T).ToString().Substring(41 + 1, len - 41 - 2)}.xml";

			try
			{
				using (FileStream fs = new FileStream(nameOfFile, FileMode.Open))
				{
					try
					{
						list = (T)xml.Deserialize(fs);
					}
					catch (Exception ex)
					{
						deserializeResult = false;
						MessageBox.Show($"Не удалось десериализовать список {typeof(T)}.\nОписание ошибки:\n{ex.Message}");
					}
				}
			}
			catch (System.IO.FileNotFoundException)
			{
				deserializeResult = false;
				MessageBox.Show($"Не найден файл \"{nameOfFile}\".\nСкорее всего ранее не проводилась сериализация, или файл был удален.");
			}

			return list;
		}

		private void SerializeButton_Click(object sender, RoutedEventArgs e)
		{
			Serialize(ListOfLocomotives.locomotives);
			Serialize(ListOfWagons.wagonsForDataGrid);
			Serialize(ListOfTrains.Trains);
			Serialize(ListOfStations.stationsForDataGrid);

			Close();

			if (serializeResult)
				MessageBox.Show("Сериализация прошла успешно!");
		}

		private void DeserializeButton_Click(object sender, RoutedEventArgs e)
		{
			var ldg = ListOfLocomotives.locomotives;
			var wdg = ListOfWagons.wagonsForDataGrid;
			var tdg = ListOfTrains.Trains;
			var sdg = ListOfStations.stationsForDataGrid;

			ListOfLocomotives.locomotives      = Deserialize(ldg);
			ListOfWagons.wagonsForDataGrid     = Deserialize(wdg);
			ListOfTrains.Trains                = Deserialize(tdg);
			ListOfStations.stationsForDataGrid = Deserialize(sdg);

			CustomColor.occupiedColors.Clear();
			CustomColor.occupiedColors.Add(CustomColor.lineColor);
			CustomColor.occupiedColors.Add(CustomColor.textColor);
			CustomColor.occupiedColors.Add(CustomColor.whiteColor);


			Random random = new Random();
			foreach (var loco in ListOfLocomotives.locomotives)
			{
				SolidColorBrush pointColor;
			generateColor:
				byte R = (byte)random.Next(192);
				byte G = (byte)random.Next(192);
				byte B = (byte)random.Next(192);

				pointColor = new SolidColorBrush(Color.FromRgb(R, G, B));
				foreach (var color in CustomColor.occupiedColors)
					if (pointColor == color)
						goto generateColor;


				loco.OnTheWay = false;
				loco.CreatePoint(window.visualizationCanvas, pointColor);
			}


			window.locoDataGrid.UpdateLocoDataGridSerialize(null, null);

			foreach (var loco in ListOfLocomotives.locomotives)
				foreach (var station in Station.stations)
					if (loco.CurrentStation.Name == station.Name)
						station.StoppedLocomotives.Add(loco);

			window.TrainsDataGrid.UpdateTrainsDataGrid(null,null);
			window.StationDataGrid.UpdateStationDataGridDeserialize(null, null);
			window.UpdateComboBoxes();

			window.wagonTabItem.IsEnabled = true;
			window.TrainsTabItem.IsEnabled = window.wagonTabItem.IsEnabled;
			window.scheduleTabItem.IsEnabled = window.wagonTabItem.IsEnabled;
			window.WagonAddButton.IsEnabled = false;
			window.WagonRandomButton.IsEnabled = window.WagonAddButton.IsEnabled;
			window.WagonsManyRandomButton.IsEnabled = window.WagonAddButton.IsEnabled;
			MainWindow.Visualize(window.visualizationCanvas, window.DateTimeLabel, MainWindow.TIME);

			Close();
			if (deserializeResult)
				MessageBox.Show("Десериализация прошла успешно!");
		}
	}
}
