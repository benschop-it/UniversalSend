using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using UniversalSend.Misc;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Interfaces {
    public interface IConfirmReceiptViewModel {
        RelayCommand AcceptCommand { get; }
        RelayCommand CancelCommand { get; }
        ConfirmReceiptPageParameter Parameter { get; }
        ISendRequestDataV2 SendRequestDataV2 { get; }

        void Initialize(ConfirmReceiptPageParameter parameter);
    }
}