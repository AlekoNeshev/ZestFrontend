/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.ViewModels;
using ZestFrontend.Views;

namespace ZestFrontend.Services
{
	public class NavigationViewService : INavigationViewService
	{
		private readonly NavigationViewModel _navigationViewModel;

		private NavigationView _navigationView;

		public NavigationViewService(NavigationViewModel navigationViewModel)
		{
			_navigationViewModel = navigationViewModel;
		}

		public NavigationView GetNavigationView()
		{
			if (_navigationView == null)
			{
				_navigationView = new NavigationView(_navigationViewModel);
			}
			return _navigationView;
		}
	}
}
*/