using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace ControleVisita.Models.IdentityExtensions
{
    public static class IdentityExtensions
    {

        public static int Getcodgrupo(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.GroupSid);

            if (claim == null)
                return 0;

            return int.Parse(claim.Value);
        }

        public static string GetUsuario(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Name);

            if (claim == null)
                return "";

            return claim.Value;
        }

        public static string GetEmpresa(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.UserData);

            if (claim == null)
                return "";

            return claim.Value;
        }
    }
}