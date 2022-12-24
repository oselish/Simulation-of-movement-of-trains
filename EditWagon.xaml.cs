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
	/// Логика взаимодействия для EditWagon.xaml
	/// </summary>
	public partial class EditWagon : Window
	{
		MainWindow window;
		Locomotive loco;
		public EditWagon(MainWindow window)
		{
			this.window = window;
			InitializeComponent();
			foreach (var type in Passenger.WagonTypes)
				wagonTypeComboBox.Items.Add(type);

			loco = MainWindow.FindLocomotive(window.locoComboBox.SelectedItem);
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
				var wagonType = wagonTypeComboBox.SelectedItem.ToString();
				string ID = wagonIDTextBox.Text;
				var selectedWagon = window.wagonDataGrid.SelectedItem as WagonForDataGrid;

				if (ListOfLocomotives.IDisUnique(ID) || ID == selectedWagon.ID)
				{
					Passenger edited = new Passenger(wagonType,loco,ID);

					var confirmEditing = new ConfirmationOfEditingWagon(window.wagonDataGrid, selectedWagon.ID, edited, window.TrainsDataGrid);
					confirmEditing.Show();
					MainWindow.Visualize(window.visualizationCanvas, window.DateTimeLabel, MainWindow.TIME);
					Close();
				}
				else
				{
					MessageBox.Show("Локомотив с таким уникальным номером уже существует");
				}
			}
		}
	}
}
