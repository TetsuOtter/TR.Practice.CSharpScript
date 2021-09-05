using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

using System.Threading.Tasks;

using NUnit.Framework;

namespace TR.Practice
{
  public class CSharpScriptTest
  {
    #region CreateTestAsync
    const int CreateTestAsync_SourceMin = 1;
    const int CreateTestAsync_SourceMax = 100;
    const int CreateTestAsync_SourceCount = 5;
    const string CreateTestAsync_ResultVariableName = "result";

    public record CreateTestAsyncTestSource(int A, int B)
    {
      public int Result { get; set; }
    }

    [Test, Sequential]
    public async Task CreateTestAsync(
      [Random(CreateTestAsync_SourceMin, CreateTestAsync_SourceMax, CreateTestAsync_SourceCount)] int source_a,
      [Random(CreateTestAsync_SourceMin, CreateTestAsync_SourceMax, CreateTestAsync_SourceCount)] int source_b)
    {
      CreateTestAsyncTestSource globalObject = new(source_a, source_b);

      Script<int>? createdScript = CSharpScript.Create<int>($"int {CreateTestAsync_ResultVariableName} = Result = A * B;", null, typeof(CreateTestAsyncTestSource));

      ScriptState<int>? result = await createdScript.RunAsync(globalObject);

      Assert.AreEqual(globalObject.A * globalObject.B, result?.GetVariable(CreateTestAsync_ResultVariableName).Value);
      Assert.AreEqual(globalObject.A * globalObject.B, globalObject.Result);
    }
    #endregion
  }
}