using System;
using UniversalSend.Services.Models.Contracts;

namespace UniversalSend.Services.InstanceCreators {

    internal class PerCallInstanceCreator : IInstanceCreator {

        public object Create(Type instanceType, params object[] args) {
            return Activator.CreateInstance(instanceType, args);
        }
    }
}