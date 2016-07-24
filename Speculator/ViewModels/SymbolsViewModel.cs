using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Docking;
using Speculator.Messages;

namespace Speculator.ViewModels
{
    [POCOViewModel]
    public class SymbolsViewModel
    {
        public virtual ObservableCollection<DocumentPanel> DocPanels { get; set; }
        public SymbolsViewModel()
        {
            Messenger.Default.Register<AddSymbolDocPanelMessage>(this, message =>
            {
                if (DocPanels == null)
                    DocPanels = new ObservableCollection<DocumentPanel> {message.DocPanel};
                else
                    DocPanels.Add(message.DocPanel);
            });
        }
    }
}