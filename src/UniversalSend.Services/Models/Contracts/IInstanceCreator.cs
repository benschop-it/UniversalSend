using System;

namespace UniversalSend.Services.Models.Contracts {

    internal interface IInstanceCreator {

        object Create(Type instanceType, object[] args);
    }
}