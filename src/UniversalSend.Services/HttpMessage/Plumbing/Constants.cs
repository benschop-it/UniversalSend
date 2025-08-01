using System.Linq;
using System.Text;

namespace UniversalSend.Services.HttpMessage.Plumbing {

    internal class Constants {

        #region Internal Fields

        internal static readonly byte ColonByte;
        internal static readonly byte CRByte;
        internal static readonly byte LFByte;
        internal static readonly byte SpaceByte;

        #endregion Internal Fields

        #region Public Constructors

        static Constants() {
            DefaultHttpEncoding = Encoding.GetEncoding("iso-8859-1");

            SpaceByte = Constants.DefaultHttpEncoding.GetBytes(new[] { ' ' }).Single();
            CRByte = Constants.DefaultHttpEncoding.GetBytes(new[] { '\r' }).Single();
            LFByte = Constants.DefaultHttpEncoding.GetBytes(new[] { '\n' }).Single();
            ColonByte = Constants.DefaultHttpEncoding.GetBytes(new[] { ':' }).Single();
        }

        #endregion Public Constructors

        #region Internal Properties

        internal static Encoding DefaultHttpEncoding { get; }

        #endregion Internal Properties
    }
}