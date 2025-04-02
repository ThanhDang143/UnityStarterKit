namespace ThanhDV.SaveSystem
{
    public interface ISavable
    {
        void Save(SaveData saveData);
        void Load(ref SaveData saveData);
    }
}
