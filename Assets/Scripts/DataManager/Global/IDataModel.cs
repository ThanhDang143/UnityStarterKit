using Cathei.BakingSheet.Unity;

namespace DataManager
{
    public interface IDataModel
    {
        public DirectAssetPath Icon { get; }
        public string Name { get; }
        public string Desc { get; }
    }
}
