using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
namespace ZestFrontend.ValueConverters
{


	public class UserMessageColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isCurrentUser && isCurrentUser)
			{
				GradientStop gradientStop = new GradientStop(Color.FromArgb("#7dd0ae"), (float)1.0);
				GradientStop gradientStop2 = new GradientStop(Color.FromArgb("#FFADD8E6"), (float)0.0);
				GradientStopCollection gradientStops = [gradientStop, gradientStop2];	
				LinearGradientBrush linearGradientBrush = new LinearGradientBrush(gradientStops);
				return linearGradientBrush;
			}
			else
			{
				GradientStop gradientStop = new GradientStop(Color.FromArgb("#FF228B22"), (float)0.0);
				GradientStop gradientStop2 = new GradientStop(Color.FromArgb("#FFADD8E6"), (float)1.0);
				GradientStopCollection gradientStops = [gradientStop, gradientStop2];
				LinearGradientBrush linearGradientBrush = new LinearGradientBrush(gradientStops);
				return Color.FromArgb("#ffffff");
			}
		}



		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}



	}

}
