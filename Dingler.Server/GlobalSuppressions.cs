// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Usage", "CA2254:Template should be a static expression",
    Justification = "In this specific scenario this is meant to be a way to allow the orchestrating project to decide the logger implementation",
    Scope = "type",
    Target = "~T:Dingler.Server.StaticLogger")]
[assembly: SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "I don't like primary constructors")]
