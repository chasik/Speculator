using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;
using Speculator.ViewModels.ExtensionMethods;

namespace Speculator.ViewModels.DataTemplateSelectors
{
    public class GlassTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GlassBigDataTemplate { get; set; }
        public DataTemplate GlassNormalDataTemplate { get; set; }
        public DataTemplate GlassSmallDataTemplate { get; set; }
        public DataTemplate GlassMicroDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var dataContext = (item as RowData).View.DataContext;

            var symbolPanelViewModel = dataContext as SymbolPanelViewModel;

            if (symbolPanelViewModel == null)
                return null;

            switch (symbolPanelViewModel.Zoom)
            {
                case 1:
                    return GlassMicroDataTemplate;
                case 2:
                    return GlassSmallDataTemplate;
                case 3:
                    return GlassNormalDataTemplate;
                case 4:
                    return GlassBigDataTemplate;
                default:
                    return GlassNormalDataTemplate;
            }
        }
    }
}
