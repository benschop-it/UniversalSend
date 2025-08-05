using Microsoft.Extensions.DependencyInjection;
using UniversalSend.Models.Data;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Interfaces.Internal;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Misc;
using UniversalSend.Models.Tasks;

namespace UniversalSend.Models {

    public static class RegisterServices {

        public static void Register(IServiceCollection services) {
            services.AddSingleton<IDispatcherProvider, DispatcherProvider>();
            services.AddSingleton<IDeviceManager, DeviceManager>();
            services.AddSingleton<IFavoriteManager, FavoriteManager>();
            services.AddSingleton<IHistoryManager, HistoryManager>();
            services.AddSingleton<IInfoDataManager, InfoDataManager>();
            services.AddSingleton<INetworkHelper, NetworkHelper>();
            services.AddSingleton<IReceiveManager, ReceiveManager>();
            services.AddSingleton<IReceiveTaskManager, ReceiveTaskManager>();
            services.AddSingleton<IRegister, Register>();
            services.AddSingleton<IRegisterDataManager, RegisterDataManager>();
            services.AddSingleton<IRegisterRequestDataManager, RegisterRequestDataManager>();
            services.AddSingleton<IRegisterResponseDataManager, RegisterResponseDataManager>();
            services.AddSingleton<ISendManager, SendManager>();
            services.AddSingleton<ISendTaskManager, SendTaskManager>();
            services.AddSingleton<ISettings, Settings>();
            services.AddSingleton<IStorageHelper, StorageHelper>();
            services.AddSingleton<ISystemHelper, SystemHelper>();
            services.AddSingleton<ITokenFactory, TokenFactory>();
            services.AddSingleton<IUniversalSendFileManager, UniversalSendFileManager>();
            services.AddSingleton<ISendRequestDataManager, SendRequestDataManager>();
            services.AddSingleton<IHttpClientHelper, HttpClientHelper>();
            services.AddSingleton<IFileRequestDataManager, FileRequestDataManager>();
            services.AddSingleton<IFileResponseDataManager, FileResponseDataManager>();

            services.AddTransient<IContentDialogManager, ContentDialogManager>();
            services.AddTransient<IDevice, Device>();
            services.AddTransient<IFavorite, Favorite>();
            services.AddTransient<IFileRequestDataV1, FileRequestDataV1>();
            services.AddTransient<IHistory, History>();
            services.AddTransient<IReceiveTask, ReceiveTask>();
            services.AddTransient<IRegisterDataV2, RegisterDataV2>();
            services.AddTransient<IRegisterRequestDataV1, RegisterRequestDataV1>();
            services.AddTransient<IRegisterRequestDataV2, RegisterRequestDataV2>();
            services.AddTransient<IAnnouncementV1, AnnouncementV1>();
            services.AddTransient<IAnnouncementV2, AnnouncementV2>();
            services.AddTransient<ISendTaskV1, SendTaskV1>();
            services.AddTransient<IUniversalSendFileV1, UniversalSendFileV1>();
            services.AddTransient<ISendRequestDataV1, SendRequestDataV1>();
            services.AddTransient<ISendRequestDataV2, SendRequestDataV2>();
        }
    }
}