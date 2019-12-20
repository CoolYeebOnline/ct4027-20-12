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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CT4027 {
	/// <summary>
	/// Interaction logic for selector.xaml
	/// </summary>


	public partial class SwatchPanel : UserControl {

		protected int borderWidth = 2;
		protected SolidColorBrush selectedBrush = new SolidColorBrush(Colors.Black);
		protected SolidColorBrush unselectedBrush = new SolidColorBrush(Colors.White);
		protected Border selectedSwatch = null;

		public delegate void SelectedItemDelegate(int selectedIndex, ImageSource source);
		public event SelectedItemDelegate SelectedItem;

		public int SpriteWidth { get; private set; } = 32;
		public int SpriteHeight { get; private set; } = 32;

		public SwatchPanel() {
			InitializeComponent();

			if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) {
				return;
			}


			// AddSprite("Sprites/dirt.png");
			// AddSprite("Sprites/dirt2.png");
			// AddSprite("Sprites/dirt3.png");

			AddTileSheet("Sprites/terrain_atlas.png", SpriteWidth, SpriteHeight);
		}

		public void AddTileSheet(string localTileSheetPath, int width, int height) {
			var imageSource = new BitmapImage(new Uri("pack://application:,,,/" + localTileSheetPath));
			for (int y = 0; y < imageSource.PixelHeight; y += height) {
				for (int x = 0; x < imageSource.PixelHeight; x += height) {
					var croppedBitmap = new CroppedBitmap(imageSource, new Int32Rect(x, y, width, height));
					AddSprite(new Image() { Source = croppedBitmap });
				}
			}
		}

		public void AddSprite(Image image) {
			var border = new Border();
			border.BorderThickness = new Thickness(borderWidth);
			border.BorderBrush = unselectedBrush;
			border.Child = image;
			border.MouseDown += OnMouseDown;

			stackpanel.Children.Add(border);
		}

		private void OnMouseDown(object sender, MouseButtonEventArgs e) {
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


		}
	}
}	
