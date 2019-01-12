using System.Collections.Generic;

namespace ASubstitute.Test.DummyTypes {
    public interface ITestInterface {
        #region Testing returning values
        void ReturnsNothing();
        int ReturnsInt();

        Point ReturnsValueType();

        List<string> ReturnsReferenceType();

        #endregion

        int AddTwoIntegers(int a, int b);
        string ConcatenateStrings(string a, string b, string c);

        int MixOfTypes(int n, string s, object o);
        object MixOfTypes(bool b, object o, List<string> list);

        int OverloadedMethod(int a);
        int OverloadedMethod(int a, int b);
        int OverloadedMethod(int a, int b, int c);
        int OverloadedMethod(string a);

        int PropertyA { get; }
        string PropertyB { get; set; }

        int MethodWithDefaultParameter(int p = 1);
    }
}