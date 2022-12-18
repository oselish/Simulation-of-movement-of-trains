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

 

namespace lab6OOP
{
	/// <summary>
	/// Логика взаимодействия для AppendWagon.xaml
	/// </summary>
	public partial class AppendWagon : Window
	{
		object locoComboBoxSelectedItem;
		MainWindow window;
		public AppendWagon(MainWindow window)
		{
			this.window = window;
			this.locoComboBoxSelectedItem = window.locoComboBox.SelectedItem;
			InitializeComponent();

			//Добавление значений в комбобокс "тип вагона"
			for (int i = 0; i < Passenger.WagonTypes.Length; i++)
				wagonTypeComboBox.Items.Add(Passenger.WagonTypes[i]);
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{

			if (wagonTypeComboBox.SelectedItem == null && wagonIDTextBox.Text == "")
				MessageBox.Show("Не выбрано ни одно свойство!");
			else if (wagonTypeComboBox.SelectedItem == null)
				MessageBox.Show("Выберите тип вагона");
			else if (wagonIDTextBox.Text == "")
				MessageBox.Show("Введите ID вагона");
			else
			{
				var ID = wagonIDTextBox.Text;
				if (ListOfWagons.IDisUnique(ID))
				{
					string wagonType = wagonTypeComboBox.SelectedItem.ToString();

					Locomotive loco = MainWindow.FindLocomotive(locoComboBoxSelectedItem);
					ListOfWagons.Insert(new Passenger(wagonType, loco, ID), loco);
					
					window.wagonDataGrid.ItemsSource = ListOfWagons.wagonsForDataGrid;
					window.TrainsDataGrid.UpdateTrainsDataGrid(sender,e);
					MainWindow.Visualize(window.visualizationCanvas, window.DateTimeLabel, MainWindow.TIME);
					Close();
				}
				else
				{
					MessageBox.Show("Вагон с таким уникальным номером уже существует");
				}
			}
		}
	}
}
