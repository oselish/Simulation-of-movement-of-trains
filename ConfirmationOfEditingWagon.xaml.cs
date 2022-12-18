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
	/// Логика взаимодействия для ConfirmationOfEditingWagon.xaml
	/// </summary>
	public partial class ConfirmationOfEditingWagon : Window
	{
		public DataGrid wagonDataGrid;
		public DataGrid TrainsDataGrid;
		public string ID;
		public Passenger wagon;

		public ConfirmationOfEditingWagon(DataGrid wagonDataGrid,string ID, Passenger wagon, DataGrid TrainsDataGrid)
		{
			this.TrainsDataGrid = TrainsDataGrid;
			this.wagonDataGrid = wagonDataGrid;
			this.ID = ID;
			this.wagon = wagon;
			InitializeComponent();
		}

		private void YesButton_Click(object sender, RoutedEventArgs e)
		{
			ListOfWagons.Replace(ID, wagon);
			wagonDataGrid.ItemsSource = null;
			wagonDataGrid.ItemsSource = ListOfWagons.wagonsForDataGrid;
			TrainsDataGrid.UpdateTrainsDataGrid(sender,e);
			Close();
		}

		private void NoButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
