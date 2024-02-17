﻿using GestorEnfermeriaJoyfe.Application.UserApp;
using GestorEnfermeriaJoyfe.Infraestructure.UserPersistence;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.UserAdapters
{
    public static class UserQueryAdapter
    {
        private static readonly UserMapper userMapper = new();
        private static readonly MySqlUserRepository userRepository = new(userMapper);

        public static async Task<Response> AuthUser(IDictionary<string, object> data)
        {
            try
            {
                string email = (string)data["email"];
                string password = (string)data["password"];

                UserAuthenticator userAuthenticator = new(userRepository);

                bool authenticated = await userAuthenticator.AuthenticateUser(email, password);
                
                return authenticated ? Response.Ok("Usuario autenticado") : Response.Fail("Usuario no autenticado");
            }
            catch (Exception e)
            {
                return Response.Fail(e.Message);
            }
           
        }

        public static Task<Response> FindUser(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

        public static Task<Response> GetAllUsers(IDictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }
    }
}
