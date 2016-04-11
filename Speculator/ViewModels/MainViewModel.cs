using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

namespace Speculator.ViewModels
{
    [POCOViewModel]
    public class MainViewModel
    {
        public virtual INavigationService NavigationService => null;

        public void NaveButtonClick(string viewName)
        {
            NavigationService.Navigate(viewName, null, this);
        }
    }
}