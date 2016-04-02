using System.Collections.Generic;
using DevExpress.Mvvm;
using DevExpress.Xpf.WindowsUI.Navigation;
using Speculator.DataModel;

namespace Speculator.ViewModel
{
    //A View Model for a GroupedItemsPage
    public class GroupedItemsViewModel : ViewModelBase, INavigationAware
    {
        IEnumerable<SampleDataItem> items;
        public GroupedItemsViewModel() { }
        public IEnumerable<SampleDataItem> Items
        {
            get { return items; }
            private set { SetProperty<IEnumerable<SampleDataItem>>(ref items, value, "Items"); }
        }
        public void LoadState(object navigationParameter)
        {
            Items = SampleDataSource.Instance.Items;
        }
        #region INavigationAware Members
        public void NavigatedFrom(NavigationEventArgs e)
        {
        }
        public void NavigatedTo(NavigationEventArgs e)
        {
            LoadState(e.Parameter);
        }
        public void NavigatingFrom(NavigatingEventArgs e)
        {
        }
        #endregion
    }
}
