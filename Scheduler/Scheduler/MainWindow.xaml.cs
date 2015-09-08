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

namespace Scheduler
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public void DrawSchedule(Calendar calendar)
		{
			for (int gameWeek = 0; gameWeek < Calendar.GameWeeks; gameWeek++)
			{
				for (int game = 0; game < calendar.Schedule[gameWeek].Games.Count; game++)
				{
					var block = new TextBlock();
					block.Style = this.Resources.FindName("Content") as Style;
					block.SetValue(Grid.RowProperty, game + 1);
					block.SetValue(Grid.ColumnProperty, gameWeek);
					block.Text = calendar.Schedule[gameWeek].Games[game].ToString();

					this.Schedule.Children.Add(block);
				}
			}

		}
	}
}
