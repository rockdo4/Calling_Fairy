using System;

public interface IDataPersistence
{
    /// <summary>
    /// 自身のデータをSaveDataオブジェクトから読み込み、状態を更新します。
    /// </summary>
    /// <param name="data">読み込むデータ</param>
    void LoadData(SaveData data);

    /// <summary>
    /// 自身のデータをSaveData dataオブジェクトにセットします。
    /// </summary>
    /// <param name="data">一括保存データ　SaveLoadSystem.SaveData</param>
    /// <param name="onSave">保存後に呼び出されるコールバック</param>
    void SetSaveData(SaveData data, Action onSave = null);
}
