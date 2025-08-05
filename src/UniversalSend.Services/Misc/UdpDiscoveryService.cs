using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Common;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Interfaces;
using UniversalSend.Strings;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace UniversalSend.Services.Misc {

    internal class UdpDiscoveryService : IUdpDiscoveryService {

        #region Private Fields

        private const string MULTICAST_GROUP = "224.0.0.167";
        private readonly ILogger _logger;
        private readonly IDeviceManager _deviceManager;
        private readonly IRegister _register;
        private readonly IRegisterResponseDataManager _registerResponseDataManager;
        private ISettings _settings;
        private DatagramSocket _udpSocket;

        #endregion Private Fields

        #region Public Constructors

        public UdpDiscoveryService(
            IRegisterResponseDataManager registerResponseDataManager,
            IDeviceManager deviceManager,
            IRegister register,
            ISettings settings

        ) {
            _logger = LogManager.GetLogger<UdpDiscoveryService>();
            _registerResponseDataManager = registerResponseDataManager ?? throw new ArgumentNullException(nameof(registerResponseDataManager));
            _deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
            _register = register ?? throw new ArgumentNullException(nameof(register));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task SendAnnouncementAsyncV2() {
            var payload = _registerResponseDataManager.GetAnnouncementV2(true);

            string json = JsonConvert.SerializeObject(payload);

            var port = payload.Port;
            var multicast = _settings.GetSettingContentAsString(Constants.Network_MulticastAddress);

            _logger.Debug($"SendAnnouncementAsync called. Port = {port}, Multicast = {multicast}.");

            using (var socket = new DatagramSocket()) {
                try {
                    var outputStream = await socket.GetOutputStreamAsync(new HostName(multicast), port.ToString());

                    using (var writer = new DataWriter(outputStream)) {
                        writer.WriteString(json);
                        await writer.StoreAsync();
                    }
                } catch (Exception ex) {
                    _logger.Debug($"Failed to send UDP announcement: {ex.Message}");
                }
            }
        }

        public async Task StartUdpListenerAsync() {
            _udpSocket = new DatagramSocket();
            _udpSocket.MessageReceived += OnUdpMessageReceivedV2;

            var port = _settings.GetSettingContentAsString(Constants.Network_Port);
            var multicast = _settings.GetSettingContentAsString("Network_MulticastAddress");

            // Must bind to a specific port before joining multicast
            await _udpSocket.BindServiceNameAsync(port);

            // Join multicast group
            _udpSocket.JoinMulticastGroup(new HostName(multicast));

            await SendAnnouncementAsyncV2();
        }

        public void StopUdpListener() {
            if (_udpSocket != null) {
                _udpSocket.MessageReceived -= OnUdpMessageReceivedV2;
                _udpSocket.Dispose();
                _udpSocket = null;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private async void OnUdpMessageReceivedV2(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args) {
            using (var reader = new DataReader(args.GetDataStream())) {
                reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                reader.InputStreamOptions = InputStreamOptions.Partial;

                await reader.LoadAsync(1024);
                string message = reader.ReadString(reader.UnconsumedBufferLength);

                IAnnouncementV2 payload = _registerResponseDataManager.DeserializeAnnouncementV2(message);
                if (payload == null) {
                    _logger.Debug("Ignore self!");
                    return;
                }

                _logger.Debug($"UDP message received: {payload.Announce}, {payload.Alias}, {payload.DeviceModel}, {payload.DeviceType}, {payload.Fingerprint}");

                if (payload.Announce) {
                    // Send HTTP POST response
                    _logger.Debug("Register via HTTP");
                    await RegisterViaHttpAsync(args.RemoteAddress.ToString(), payload.Port, "2.1");
                } else {
                    _logger.Debug("Register new device, no announcement!");
                    IDevice device = _deviceManager.GetDeviceFromResponseDataV2(payload, args.RemoteAddress.CanonicalName);
                    _register.NewDeviceRegisterEvent(device);
                }
            }
        }

        private async Task RegisterViaHttpAsync(string ip, int port, string version) {
            IRegisterResponseDataV2 payload = _registerResponseDataManager.GetRegisterResponseDataV2();
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            try {
                using (var client = new HttpClient()) {
                    await client.PostAsync($"http://{ip}:{port.ToString()}/api/localsend/{version}/register", content);
                }
            } catch (Exception ex) {
                _logger.Debug($"RegisterViaHttpAsync failed: {ex.Message}");
            }
        }

        #endregion Private Methods

    }
}