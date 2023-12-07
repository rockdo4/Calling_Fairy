public class ClearStageInfo
{
    public static int ClearStgInfo { get; set; }

    public int SetClearStageInfo()
    {
        return ClearStgInfo;
    }
    public void  GetStageInfo(int stageID)
    {
        ClearStgInfo = stageID;
    }
}
