using Cathei.BakingSheet.Unity;

namespace BakingSheetImpl
{
    public interface IDataModel
    {
        public DirectAssetPath Icon { get; }
        public string Name { get; }
        public string Desc { get; }
    }
}
