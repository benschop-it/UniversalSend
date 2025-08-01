﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Serialization;
using UniversalSend.Services.Models.Schemas;

namespace UniversalSend.Services.Http {

    internal class ContentSerializer {

        #region Internal Methods

        internal object FromContent(string content, MediaType contentMediaType, Type contentType) {
            if (contentMediaType == MediaType.JSON) {
                return JsonConvert.DeserializeObject(content, contentType);
            } else if (contentMediaType == MediaType.XML) {
                return XmlDeserializeObject(content, contentType);
            }

            throw new NotImplementedException();
        }

        internal byte[] ToAcceptContent(object contentObject, RestServerRequest req) {
            if (contentObject == null) {
                return new byte[0];
            }

            if (req.AcceptMediaType == MediaType.JSON) {
                return req.AcceptEncoding.GetBytes(JsonConvert.SerializeObject(contentObject));
            } else if (req.AcceptMediaType == MediaType.XML) {
                return req.AcceptEncoding.GetBytes(XmlSerializeObject(contentObject));
            }

            return new byte[0];
        }

        #endregion Internal Methods

        #region Private Methods

        private static object XmlDeserializeObject(string content, Type toType) {
            var serializer = new XmlSerializer(toType);
            object result;

            using (TextReader reader = new StringReader(content)) {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        private static string XmlSerializeObject(object toSerialize) {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter()) {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        #endregion Private Methods
    }
}