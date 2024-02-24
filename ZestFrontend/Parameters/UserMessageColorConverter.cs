﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
namespace ZestFrontend.Parameters
{


	public class UserMessageColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isCurrentUser && isCurrentUser)
			{

				return Microsoft.Maui.Graphics.Color.FromArgb("#02c0b3");
			}
			else
			{
				return Microsoft.Maui.Graphics.Color.FromArgb("#f6c0b3");
			}
		}



		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}



	}

}