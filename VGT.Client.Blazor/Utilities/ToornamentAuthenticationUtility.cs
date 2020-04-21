using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VGT.Common.Models;

namespace VGT.Client.Blazor.Utilities
{
    public static class ToornamentAuthenticationUtility
    {
        public async static Task<bool> DoesLocalTokenExistAsync(ILocalStorageService LocalStorageService)
        {
            return await LocalStorageService.ContainKeyAsync("ToornamentToken");
        }

        public async static Task<ToornamentToken> RetrieveTokenFromLocalStorage(ILocalStorageService LocalStorageService)
        {
            return await LocalStorageService.GetItemAsync<ToornamentToken>("ToornamentToken");
        }

        public async static Task StoreTokenInLocalStorage(ILocalStorageService LocalStorageService, ToornamentToken TokenToAdd)
        {
            await LocalStorageService.SetItemAsync("ToornamentToken", TokenToAdd);
        }

        public async static Task<ToornamentToken> GetFreshTokenUsingRefreshToken()
        {
            throw new NotImplementedException();
        }

        public static void InitiateLoginFlowToGetNewToken(NavigationManager NavManager, Uri ClientRedirectURL)
        {
            NavManager.NavigateTo
            (
                "https://account.toornament.com/oauth2/authorize?response_type=code&" +
                "client_id=79c3270e1d8eee3741075a46152tddg45yv44g0csoss84k4s8g0o0g4wkwws0wo88gccss4wo&" +
                "redirect_uri=https://localhost:44366/Authentication/ToornamentAuthCallback&" +
                "scope=participant:manage_registrations&" +
                "state=" + HttpUtility.UrlEncode(ClientRedirectURL.AbsoluteUri),
                true
            );
        }
    }
}
