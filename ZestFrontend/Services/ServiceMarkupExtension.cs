using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.ViewModels;

namespace ZestFrontend.Services
{
	public class ServiceMarkupExtension : IMarkupExtension
	{
		private readonly CommunityService _service;

		public ServiceMarkupExtension(CommunityService service)
		{
			_service = service;
		}

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			// Instantiate the view model with the resolved service dependency
			var viewModel = new NavigationViewModel(_service);

			return viewModel;
		}
	}

}
