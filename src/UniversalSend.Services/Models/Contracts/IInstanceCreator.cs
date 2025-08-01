using System;

namespace UniversalSend.Services.Models.Contracts {

    internal interface IInstanceCreator {

        #region Public Methods

        object Create(Type instanceType, object[] args);

        #endregion Public Methods
    }
}