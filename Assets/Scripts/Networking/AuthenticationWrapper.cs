using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

public static class AuthenticationWrapper
{
    public static AuthState CurrentAuthState { get; private set; } = AuthState.NotAuthenticated;
    public static async Task<AuthState> DoAuth(int tries = 5)
    {
        if (CurrentAuthState == AuthState.Authenticated)
            return CurrentAuthState;
        await SignInAnonymouslyAsync(tries);
        return CurrentAuthState;
    }

    private static async Task SignInAnonymouslyAsync(int tries)
    {
        CurrentAuthState = AuthState.NotAuthenticated;
        for (int currentTries = 0; currentTries < tries; currentTries++)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
            {
                CurrentAuthState = AuthState.Authenticated;
                break;
            }
            await Task.Delay(1000);
        }
    }

}
public enum AuthState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error,
    TimeOut
}