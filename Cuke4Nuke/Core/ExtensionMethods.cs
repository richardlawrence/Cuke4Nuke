using System;

namespace Cuke4Nuke.Core
{
    public static class ExtensionMethods
    {
        public static string FullNameWithArgTypes(this System.Reflection.MethodInfo methodInfo)
        {
            // Is there a better way of getting a fully qualified signature that includes the parameter types?
            var signatureWithReturnType = methodInfo.ToString();
            var positionOfSpaceAfterReturnType = signatureWithReturnType.IndexOf(' ');
            var signatureWithoutReturnType = signatureWithReturnType.Substring(positionOfSpaceAfterReturnType + 1);
            var methodName = String.Format("{0}.{1}", methodInfo.DeclaringType.FullName, signatureWithoutReturnType);
            return methodName;
        }
    }
}
