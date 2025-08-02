using System;
using UniversalSend.Models.Common;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Models.Contracts;

namespace UniversalSend.Services.InstanceCreators {

    internal class PerCallInstanceCreator : IInstanceCreator {

        #region Private Fields

        private readonly ILogger _logger;

        #endregion Private Fields

        #region Public Constructors

        public PerCallInstanceCreator() {
            _logger = LogManager.GetLogger<PerCallInstanceCreator>();
        }

        #endregion Public Constructors

        #region Public Methods

        public object Create(Type instanceType, params object[] args) {
            _logger.Debug($"Creating Per Call type {instanceType.FullName}");

            return Activator.CreateInstance(instanceType, args);
        }

        #endregion Public Methods

    }
}