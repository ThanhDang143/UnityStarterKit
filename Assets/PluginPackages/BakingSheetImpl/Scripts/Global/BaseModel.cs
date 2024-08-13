using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;

namespace BakingSheetImpl
{
    public class BaseModel : SheetRow, IDataModel
    {
        public DirectAssetPath Icon { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
