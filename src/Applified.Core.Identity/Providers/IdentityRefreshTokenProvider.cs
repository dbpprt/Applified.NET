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

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Owin.Security.Infrastructure;
//using PersonalPage.Model.Entities.Identity;

//namespace PersonalPage.Identity.Providers
//{
//    public class IdentityRefreshTokenProvider : IAuthenticationTokenProvider
//    {
//        public async Task CreateAsync(AuthenticationTokenCreateContext context)
//        {
//            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

//            if (string.IsNullOrEmpty(clientid))
//            {
//                return;
//            }

//            var refreshTokenId = Guid.NewGuid().ToString("n");

//            using (AuthRepository _repo = new AuthRepository())
//            {
//                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime"); 
               
//                var token = new RefreshToken() 
//                { 
//                    Id = Helper.GetHash(refreshTokenId),
//                    ClientId = clientid, 
//                    Subject = context.Ticket.Identity.Name,
//                    IssuedUtc = DateTime.UtcNow,
//                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime)) 
//                };

//                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
//                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;
                
//                token.ProtectedTicket = context.SerializeTicket();

//                var result = await _repo.AddRefreshToken(token);

//                if (result)
//                {
//                    context.SetToken(refreshTokenId);
//                }
             
//            }
//        }

//        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
//        {

//            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
//            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

//            string hashedTokenId = Helper.GetHash(context.Token);

//            using (AuthRepository _repo = new AuthRepository())
//            {
//                var refreshToken = await _repo.FindRefreshToken(hashedTokenId);

//                if (refreshToken != null )
//                {
//                    //Get protectedTicket from refreshToken class
//                    context.DeserializeTicket(refreshToken.ProtectedTicket);
//                    var result = await _repo.RemoveRefreshToken(hashedTokenId);
//                }
//            }
//        }

//        public void Create(AuthenticationTokenCreateContext context)
//        {
//            throw new NotImplementedException();
//        }

//        public void Receive(AuthenticationTokenReceiveContext context)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
//}
