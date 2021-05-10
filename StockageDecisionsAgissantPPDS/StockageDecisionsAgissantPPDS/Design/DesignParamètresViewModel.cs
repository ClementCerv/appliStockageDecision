using System.IO;
using JetBrains.Annotations;
using StockageDecisionsAgissantPPDS.ViewModel;

namespace StockageDecisionsAgissantPPDS.Design
{
    [UsedImplicitly]
    internal class DesignParamètresViewModel : ParamètresViewModel
    {
        public DesignParamètresViewModel(FileInfo logFile) : base(logFile)
        {
        }
    }
}
