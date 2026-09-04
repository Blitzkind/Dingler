// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "This hides intent, I don't like that.")]
[assembly: SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>")]
[assembly: SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
[assembly: SuppressMessage("Performance", "CA1862:Use the 'StringComparison' method overloads to perform case-insensitive string comparisons", Justification = "EF Core doesn't work with that.", Scope = "member", Target = "~M:Dingler.Game.Repositories.AccountRepository.GetIdByUsernameAsync(System.String)~System.Threading.Tasks.Task{System.UInt64}")]
