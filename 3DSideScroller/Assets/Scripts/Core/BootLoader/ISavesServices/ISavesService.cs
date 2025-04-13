public interface ISavesService : IService
{
    public void SaveValue(string name, object value);

    public object GetValue(string name);

    public bool IsAllSaved { get; }
}
