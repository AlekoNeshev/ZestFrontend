using ZestFrontend.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
namespace ZestFrontend.Views;

public partial class NavigationView : ContentView
{


    
    public NavigationView(NavigationViewModel navigationViewModel)
	{
		BindingContext = navigationViewModel;
		InitializeComponent();
	}
	

	
	void OnPointerEntered(object sender, PointerEventArgs e)
	{
		if (sender is View view)
		{
			// Change the color when the pointer enters the element
			view.BackgroundColor = Color.FromArgb("#7dd0ae"); // Change to your desired color
		}
	}

	void OnPointerExited(object sender, PointerEventArgs e)
	{
		if (sender is View view)
		{
			// Reset the color when the pointer exits the element
			view.BackgroundColor = Color.FromArgb("#f8f8ff");
		}
	}


	void OnPointerMoved(object sender, PointerEventArgs e)
	{
		// Handle the pointer moved event
	}

	private void ImageButton_Clicked(object sender, EventArgs e)
	{
		

		if (SecondImageButton.Rotation == 0)
		{
			
			SecondImageButton.RotateTo(180);
		}
		else
		{
			SecondImageButton.RotateTo(0);
		}

	}
}