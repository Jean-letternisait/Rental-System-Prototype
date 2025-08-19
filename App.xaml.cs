using cpsy200FinalProject.database;
using SQLite;

namespace cpsy200FinalProject
{
	public partial class App : Application
	{
		
		public App()
		{
			
        }

       
        

		protected override Window CreateWindow(IActivationState? activationState)
		{
			return new Window(new MainPage()) { Title = "cpsy200FinalProject" };
		}
	}
}
