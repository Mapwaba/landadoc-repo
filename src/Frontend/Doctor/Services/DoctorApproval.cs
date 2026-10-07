using LandaDoc.Frontend.Shared.Services.ApiClients;
using LandaDoc.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace LandaDoc.Doctor.Services;

// Whether the signed-in doctor may use the app yet. Until an admin approves their profile, the
// layout shows a waiting message instead of every page except Profile (see MainLayout).
public class DoctorApproval(IAdminApiClient admin, AuthenticationStateProvider auth)
{
    public enum State
    {
        Unknown,     // not checked yet, not a doctor, or the Admin service couldn't be reached
        Approved,
        Pending,
        Suspended,
        NoProfile,   // account exists but the doctor hasn't created their profile yet
    }

    public State Current { get; private set; } = State.Unknown;

    public bool MustWait => Current is State.Pending or State.Suspended or State.NoProfile;

    // Re-asks the Admin service unless already approved, so an approval made while the doctor is
    // using the app takes effect on their next click. An unreachable service leaves the state
    // Unknown (pages stay open and the backend still refuses what a pending doctor can't do),
    // so an approved doctor isn't locked out while Render wakes the service up.
    public async Task RefreshAsync()
    {
        if (Current == State.Approved) return;

        var user = (await auth.GetAuthenticationStateAsync()).User;
        if (user.Identity?.IsAuthenticated != true || !user.IsInRole("Doctor"))
        {
            Current = State.Unknown;
            return;
        }

        var lookup = await admin.LookUpMyProfileAsync();
        if (!lookup.Reachable) return; // keep whatever we knew

        Current = lookup.Profile?.Status switch
        {
            null => State.NoProfile,
            DoctorApprovalStatus.Approved => State.Approved,
            DoctorApprovalStatus.Suspended => State.Suspended,
            _ => State.Pending,
        };
    }

    // On login/logout: forget the previous account's state
    public void Reset() => Current = State.Unknown;
}
