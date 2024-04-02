using MauiIcons.Core;
using MauiIcons.Fluent.Filled;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.ValueConverters
{
	public class DislikeButtonColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is LikeDTO like && like != null)
			{
				
				 if (like.Value == false)
				{
					return (ImageSource)new MauiIcon() { Icon=FluentFilledIcons.ArrowDown20Filled, IconColor=Color.FromArgb("#FF3232"), IconSize = 20 };

					
				}
				else
				{

					return (ImageSource)new MauiIcon() { Icon=FluentFilledIcons.ArrowDown20Filled, IconColor=Color.FromArgb("#000000"), IconSize = 20 };
				}
			}
			else
			{

				return (ImageSource)new MauiIcon() { Icon=FluentFilledIcons.ArrowDown20Filled, IconColor=Color.FromArgb("#000000"), IconSize = 20 };
			}
		}


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
