namespace VProtect.Core.Runtime
{
    internal static class RTMap
    {
        public static readonly string VMData = "VProtect.Runtime.Data.VMData";

        public static readonly string Entry = "VProtect.Runtime.Entry";
        public static readonly string Entry_Execute = "Execute";

        public static readonly string Constants = "VProtect.Runtime.Dynamic.Constants";

        public static string Mutation = "Mutation";
        public static string Mutation_Placeholder = "Placeholder";
        public static string Mutation_LocationIndex = "LocationIndex";
        public static string Mutation_Value_T = "Value";
        public static string Mutation_Value_T_Arg0 = "Value";
        public static string Mutation_Crypt = "Crypt";

        public static readonly string AnyCtor = ".ctor";
        public static readonly string AnyCCtor = ".cctor";
    }
}
