namespace NServiceBus
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implementors will receive the list of active Profiles.
    /// Implementors must implement <see cref="IHandleProfile" />.
    /// </summary>
    [ObsoleteEx(ReplacementTypeOrMember = nameof(IConfigureThisEndpoint), RemoveInVersion = "8", TreatAsErrorFromVersion = "7")]
    public interface IWantTheListOfActiveProfiles
    {
        /// <summary>
        /// ActiveProfiles list will be set by the infrastructure.
        /// </summary>
        IEnumerable<Type> ActiveProfiles { get; set; }
    }
}