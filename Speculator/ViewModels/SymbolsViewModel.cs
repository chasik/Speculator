using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Docking;
using Speculator.Views;

namespace Speculator.ViewModels
{
    [POCOViewModel]
    public class SymbolsViewModel
    {
        public void AddSymbol(object commandParams)
        {
            var docPanel = new DocumentPanel
            {
                Caption = "document uuuuu",
                Content = new SymbolPanelView {DataContext = new SymbolPanelViewModel()}
            };
            (commandParams as DocumentGroup)?.Items.Add(docPanel);
        }}
}