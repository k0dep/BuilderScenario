using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;

namespace BuilderScenario
{
    [TagAlias("ConditionalExpression")]
    public class ConditionalExpressionJob : IBuildJob
    {
        public IBuildJob Job { get; set; }
        public string Expression { get; set; }

        public bool Run(IJobExecuteService executer, IConfigMap configMap, IBuildLogger logger)
        {
            var refs = AppDomain.CurrentDomain.GetAssemblies();
            var refFiles = refs.Where(a => !a.IsDynamic).Select(a => a.Location).ToArray();
            var cSharp = (new CSharpCodeProvider()).CreateCompiler();
            var compileParams = new CompilerParameters(refFiles);
            compileParams.GenerateInMemory = true;
            compileParams.GenerateExecutable = false;

            const string code = @"class RuntimeExpression {{ public static bool Evaluate() {{ return {0}; }} }}";

            var compilerResult = cSharp.CompileAssemblyFromSource(compileParams, string.Format(code, configMap.Interpolate(Expression)));
            var asm = compilerResult.CompiledAssembly;
            
            if (compilerResult.Errors.Count > 0)
            {
                foreach (CompilerError error in compilerResult.Errors)
                {
                    logger.Error($"expression compilation error: {error}");
                }

                return false;
            }

            var @class = asm.GetType("RuntimeExpression");
            var method = @class.GetMethod("Evaluate");
            if ((bool)method.Invoke(null, new object[0]))
            {
                logger.Log($"expression is true. Invoke jobs");
                return executer.Execute(Job).IsSucces;
            }
            
            logger.Log("condition is false. skip running job");

            return true;
        }
    }
}