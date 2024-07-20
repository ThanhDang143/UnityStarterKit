using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;

namespace DataManager
{
    public class SheetContainer : SheetContainerBase
    {
        public SheetContainer() : base(UnityLogger.Default) { }

        // use name of each matching sheet name from source
        public DemoSheet Demo { get; private set; }
    }
}