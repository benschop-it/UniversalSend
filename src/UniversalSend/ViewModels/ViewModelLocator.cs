using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace UniversalSend.Interfaces {

    public class ViewModelLocator {

        public ConfirmReceiptViewModel ConfirmReceipt => ServiceLocator.Current.GetInstance<ConfirmReceiptViewModel>();
    }
}