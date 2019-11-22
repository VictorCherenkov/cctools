namespace CCTools.Model
{
    public interface ICssWebHelper
    {
        string GetNewConfigSpecFromWeb(string productName, string release, string srNum);
        SyncVobData GetSyncVobData(string vobName);
    }
}