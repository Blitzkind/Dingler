using System.Reflection;
using System.Reflection.Emit;

namespace Dingler.Game.Configuration
{
    public static class UnityTypeResolver
    {
        public static void Initialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                if (!args.Name.StartsWith("UnityEngine,"))
                    return null;

                var assemblyName = new AssemblyName(args.Name);
                var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                    assemblyName,
                    AssemblyBuilderAccess.Run);
                var moduleBuilder = assemblyBuilder.DefineDynamicModule("DummyUnityEngine");

                var keyCodeTypeBuilder = moduleBuilder.DefineType(
                    "UnityEngine.KeyCode",
                    TypeAttributes.Public |
                    TypeAttributes.Sealed |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.AutoClass,
                    typeof(Enum));

                keyCodeTypeBuilder.DefineField("value__", typeof(int),
                    FieldAttributes.Private |
                    FieldAttributes.SpecialName |
                    FieldAttributes.RTSpecialName);

                var enumValues = new[]
                {
                    ("F1", 282),
                    ("F4", 285),
                    ("F5", 286),
                    ("F8", 289),
                    ("F10", 291),
                    ("X", 120),
                    ("Space", 32),
                    ("LeftControl", 320)
                };

                foreach (var (name, value) in enumValues)
                {
                    keyCodeTypeBuilder.DefineField(
                        name,
                        keyCodeTypeBuilder,
                        FieldAttributes.Public |
                        FieldAttributes.Static |
                        FieldAttributes.Literal)
                    .SetConstant(value);
                }

                keyCodeTypeBuilder.CreateType();

                return assemblyBuilder;
            };
        }
    }
}
