using Material.Components.Maui.Interfaces;
using Material.Components.Maui.Tokens;
using Material.Components.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.Button
{
	public class CustomButton : IconButton
	{
		private bool _isInputTransparent;
		private void OnClicked(object sender, TouchEventArgs e)
		{
			if (_isInputTransparent)
			{
				return;
			}

			_isInputTransparent = true;

			if (IsToggleEnabled)
			{
				IsSelected = !IsSelected;
			}

			if (base.Command?.CanExecute(base.CommandParameter ?? (IsToggleEnabled ? ((object)IsSelected) : null)) ?? false)
			{
				base.Command?.Execute(base.CommandParameter ?? (IsToggleEnabled ? ((object)IsSelected) : null));
			}

			Task.Delay(100).ContinueWith(_ =>
			{
				_isInputTransparent = false;
				((Frame)Parent).InputTransparent = false;
			});

			((Frame)Parent).InputTransparent = true;
		}

	}
}
