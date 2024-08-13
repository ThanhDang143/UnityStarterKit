using System.Collections.Generic;
using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;
using Sirenix.Utilities;

namespace BakingSheetImpl
{
    public class SheetContainer : SheetContainerBase
    {
        public SheetContainer() : base(UnityLogger.Default) { }

        #region Declare
        public DemoSheet Demo { get; private set; }
        #endregion

        public Dictionary<string, IDataModel> CacheData()
        {
            Dictionary<string, IDataModel> result = new();

            #region CacheData
            Demo.ForEach(d => result.Add(d.Id, d));
            #endregion

            return result;
        }
    }
}