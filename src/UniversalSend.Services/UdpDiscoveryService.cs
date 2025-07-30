using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace UniversalSend.Services {

    internal class UdpDiscoveryService {

        #region Private Fields

        private const string MULTICAST_GROUP = "224.0.0.167";
        private DatagramSocket _udpSocket;
        private readonly IRegisterResponseDataManager _registerResponseDataManager;
        private readonly IDeviceManager _deviceManager;
        private readonly IRegister _register;
        private ISettings _settings;

        #endregion Private Fields

        public UdpDiscoveryService(
            IRegisterResponseDataManager registerResponseDataManager,
            IDeviceManager deviceManager,
            IRegister register,
            ISettings settings

        ) {
            _registerResponseDataManager = registerResponseDataManager ?? throw new ArgumentNullException(nameof(registerResponseDataManager));
            _deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
            _register = register ?? throw new ArgumentNullException(nameof(register));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        #region Public Methods

        public async Task StartUdpListenerAsync() {
            _udpSocket = new DatagramSocket();
            _udpSocket.MessageReceived += OnUdpMessageReceived;

            var port = _settings.GetSettingContentAsString(Constants.Network_Port);
            var multicast = _settings.GetSettingContentAsString("Network_MulticastAddress");

            // Must bind to a specific port before joining multicast
            await _udpSocket.BindServiceNameAsync(port);

            // Join multicast group
            _udpSocket.JoinMulticastGroup(new HostName(multicast));

            await SendAnnouncementAsync();
        }

        public void StopUdpListener() {
            if (_udpSocket != null) {
                _udpSocket.MessageReceived -= OnUdpMessageReceived;
                _udpSocket.Dispose();
                _udpSocket = null;
            }
        }

        public async Task SendAnnouncementAsync() {
            var payload = _registerResponseDataManager.GetRegisterReponseData(true);

            string json = JsonConvert.SerializeObject(payload);
            var port = _settings.GetSettingContentAsString(Constants.Network_Port);
            var multicast = _settings.GetSettingContentAsString(Constants.Network_MulticastAddress);

            using (var socket = new DatagramSocket()) {
                try {
                    var outputStream = await socket.GetOutputStreamAsync(new HostName(multicast), port);

                    using (var writer = new DataWriter(outputStream)) {
                        writer.WriteString(json);
                        await writer.StoreAsync();
                    }
                } catch (Exception ex) {
                    Debug.WriteLine($"Failed to send UDP announcement: {ex.Message}");
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private async void OnUdpMessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args) {
            using (var reader = new DataReader(args.GetDataStream())) {
                reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                reader.InputStreamOptions = InputStreamOptions.Partial;

                await reader.LoadAsync(1024);
                string message = reader.ReadString(reader.UnconsumedBufferLength);

                IRegisterResponseData payload = _registerResponseDataManager.DeserializeRegisterResponseData(message);
                if (payload == null) {
                    Debug.WriteLine("Ignore self!");
                    return;
                }

                Debug.WriteLine($"UDP message received: {payload.Announcement}, {payload.Alias}, {payload.DeviceModel}, {payload.DeviceType}, {payload.Fingerprint}");

                if (payload.Announcement) {
                    // Send HTTP POST response
                    Debug.WriteLine("Register via HTTP");
                    await RegisterViaHttpAsync(args.RemoteAddress.ToString(), payload.Fingerprint);
                } else {
                    Debug.WriteLine("Register new device, no announcement!");
                    IDevice device = _deviceManager.GetDeviceFromResponseData(payload, args.RemoteAddress.CanonicalName);
                    _register.NewDeviceRegisterV1Event(device);
                }
            }
        }

        private async Task RegisterViaHttpAsync(string ip, string remoteFingerprint) {
            IRegisterResponseData payload = _registerResponseDataManager.GetRegisterReponseData(false);
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            try {
                using (var client = new HttpClient()) {
                    await client.PostAsync($"http://{ip}:53317/api/localsend/v1/register", content);
                }
            } catch (Exception ex) {
                Debug.WriteLine($"RegisterViaHttpAsync failed: {ex.Message}");
            }
        }

        #endregion Private Methods
    }
}