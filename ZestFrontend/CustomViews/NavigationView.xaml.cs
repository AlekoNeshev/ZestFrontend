using ZestFrontend.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
namespace ZestFrontend.CustomViews;

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
			view.BackgroundColor = Color.FromArgb("#7dd0ae"); 
		}
	}

	void OnPointerExited(object sender, PointerEventArgs e)
	{
		if (sender is View view)
		{
			view.BackgroundColor = Color.FromArgb("#f8f8ff");
		}
	}


	void OnPointerMoved(object sender, PointerEventArgs e)
	{
		
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