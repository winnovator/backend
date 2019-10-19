using System.Diagnostics.CodeAnalysis;

namespace WInnovator.Helper
{
    [ExcludeFromCodeCoverage]
    public static class VersionInfo
    {
        public static string assemblyVersion { get
            {
                return $"{ GitVersionInformation.Major }.{ GitVersionInformation.Minor }.{ GitVersionInformation.Patch }-{ GitVersionInformation.ShortSha }";
            }
        }

        public static string branchName {  get
            {
                return $"{ GitVersionInformation.BranchName }";
            } 
        }

    }
}
