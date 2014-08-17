#region Copyright (C) 2014 Applified.NET 
// Copyright (C) 2014 Applified.NET
// http://www.applified.net

// This file is part of Applified.NET.

// Applified.NET is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.

// You should have received a copy of the GNU Affero General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion

using System.IO;
using CookComputing.XmlRpc;
using Microsoft.Owin;

namespace Applified.IntegratedFeatures.Blog.XmlRpcOwin
{
    public class OwinXmlRpcHttpResponse : IHttpResponse
    {
        private readonly IOwinResponse _response;

        public OwinXmlRpcHttpResponse(IOwinResponse response)
        {
            _response = response;
            Output = new StreamWriter(response.Body);
            OutputStream = response.Body;
        }

        public long ContentLength
        {
            set { _response.ContentLength = value; }
            get { return _response.ContentLength.HasValue ? _response.ContentLength.Value : 0; }
        }

        public string ContentType
        {
            set { _response.ContentType = value; }
            get { return _response.ContentType; }
        }

        public TextWriter Output { get; private set; }
        
        public Stream OutputStream { get; private set; }
        
        public bool SendChunked { get; set; }
        
        public int StatusCode
        {
            set { _response.StatusCode = value; }
            get { return _response.StatusCode; }
        }

        public string StatusDescription { get; set; }
    }
}
