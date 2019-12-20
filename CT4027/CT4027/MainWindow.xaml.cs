using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NReco.PdfGenerator;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes; 


namespace CT4027
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private int GridWidth = 5;
		private int GridHeight = 5;
		protected Border selectedSwatch = null;
		protected int borderWidth = 2;
		protected SolidColorBrush selectedBrush = new SolidColorBrush(Colors.Black);
		protected SolidColorBrush unselectedBrush = new SolidColorBrush(Colors.White);
		private Image[,] images;

		private ImageSource penImageSource;

		public delegate void SelectedItemDelegate(int selectedIndex, ImageSource source);
		public event SelectedItemDelegate SelectedItem;

		public MainWindow() {

			InitializeComponent();

			images = new Image[GridWidth, GridHeight];

			for (int x = 0; x < GridWidth; x++) {
				for (int y = 0; y < GridHeight; y++) {
					Image image = new Image();
					Grid.SetColumn(image, x);
					Grid.SetRow(image, y);

					grid.Children.Add(image);

					images[x, y] = image;
				}
			}

			grid.MouseDown += OnMouseDown;
		}
		private void OnMouseDown(object sender, MouseButtonEventArgs e) {
			int x = (int)e.GetPosition(grid).X / 32;
			int y = (int)e.GetPosition(grid).Y / 32;

			var border = sender as Border;
			if (selectedSwatch != null) {
				selectedSwatch.BorderBrush = unselectedBrush;
			}
			selectedSwatch = border;
			border.BorderBrush = selectedBrush;
			int selectedIndex = stackpanel.Children.IndexOf(border);

			if (selectedSwatch != null) {
				ImageSource source = (border.Child as Image).Source;
				SelectedItem?.Invoke(selectedIndex, source);
			}

			UseTool(x, y);
		}

		private void UseTool(int gridX, int gridY) {
			if (gridX > 4 || gridY > 4) {
				return;
			}
			switch (selectedTool) {
				case Tool.Pen:
					images[gridX, gridY].Source = penImageSource;
					break;
				case Tool.Erase:
					images[gridX, gridY].Source = null;
					break;
			}

		}
		private void HandleNew(object sender, RoutedEventArgs e) {

		}

		private void HandleClose(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}

		private void HandlePen(object sender, RoutedEventArgs e) {
			selectedTool = Tool.Pen;
		}

		private void HandleErase(object sender, RoutedEventArgs e) {
			selectedTool = Tool.Erase;
		}

		private void HandleSave(object sender, RoutedEventArgs e) {
			JsonSerializer serializer = new JsonSerializer();
			serializer.Converters.Add(new JavaScriptDateTimeConverter());
			serializer.NullValueHandling = NullValueHandling.Ignore;

			using (StreamWriter sw = new StreamWriter(@"E:\grid\grid.json"))
			using (JsonWriter writer = new JsonTextWriter(sw)) {

				MessageBox.Show("Saving...");

			}


		}

		public enum Tool {
			Pen,
			Erase
		}
		private Tool selectedTool = Tool.Pen;


	}



}

