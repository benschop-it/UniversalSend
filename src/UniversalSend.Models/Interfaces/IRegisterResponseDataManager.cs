namespace UniversalSend.Models.Interfaces {
    public interface IRegisterResponseDataManager {
        IRegisterResponseData GetRegisterReponseData(bool announcement);

        IRegisterResponseData DeserializeRegisterResponseData(string json);
    }
}