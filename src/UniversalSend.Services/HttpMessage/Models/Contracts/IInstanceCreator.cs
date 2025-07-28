using System;

namespace UniversalSend.Services.Models.Contracts
{
    interface IInstanceCreator
    {
        object Create(Type instanceType, object[] args);
    }
}
