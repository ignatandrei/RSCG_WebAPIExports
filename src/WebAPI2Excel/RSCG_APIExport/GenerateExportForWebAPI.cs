namespace RSCG_APIExport;

[Generator(LanguageNames.CSharp)]
public class GenerateExportForWebAPI : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource("MiddlewareExportToFile.g.cs", MyAdditionalFiles.MiddlewareExportToFile_gen_txt);
            ctx.AddSource("Extensions.g.cs", MyAdditionalFiles.Extensions_gen_txt);
        });

        var declarationsControllers = context.SyntaxProvider
        .CreateSyntaxProvider(
            predicate: static (s, _) => IsController(s),
            transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx)

            )
        .Where(static m => m is not null)!
        .SelectMany((data,_)=>data!);

        var compilationAndData
            = context.CompilationProvider.Combine(declarationsControllers.Collect());

        context.RegisterSourceOutput(compilationAndData,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static void Execute(Compilation item1, ImmutableArray<string> item2, SourceProductionContext spc)
    {
        var distinctArr=item2.ToArray().Distinct().ToArray();
        var exports=distinctArr.Select(it=>$"MiddlewareExportToFile.AddReturnType(typeof({it}[]));").ToArray();
        var data =string.Join("\r\n", exports);
        var header = $$"""
 namespace WebApiExportToFile;

 public static partial class Extensions
 {
    static partial  void AddReturnTypesFromGenerator(){
        {{data}} 
    }
 }
 """;


        spc.AddSource("middlewareExport.methods.g.cs", header);
    }

    static IEnumerable<TypeSyntax?> GetTypeRecursive(TypeSyntax ret)
    {
        if (ret is ArrayTypeSyntax ats)
        {
            yield return ats.ElementType;    
            yield break;
        }
        if (ret is GenericNameSyntax name)
        {
            if (name.TypeArgumentList.Arguments.Count > 0)
            {
                foreach (var data in name.TypeArgumentList.Arguments)
                {



                    foreach (var item in GetTypeRecursive(data).ToArray())
                    {
                        if (item != null)
                            yield return item;
                    }
                }
            }
            else
            {
                //todo: what I have missed?
                //var x = 1;
            }
        }

    }
    static string[]? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var sem = context.SemanticModel;
        var cds = (ClassDeclarationSyntax)context.Node;
        var methods = cds.DescendantNodes().OfType<MethodDeclarationSyntax>().ToArray();
        var types=new HashSet<TypeInfo>();
        foreach ( var method in methods )
        {
            
            var ret = method.ReturnType;
            var items= GetTypeRecursive(ret)
                .ToArray()
                .Where(it=>it!=null)
                .Distinct()
                .ToArray();
            foreach ( var item in items )
            {
                var info = sem.GetTypeInfo(item!);
                types.Add(info);
            }
            //var z = 1;
            //var m = sem.GetSymbolInfo(ret);
            //var type = sem.GetTypeInfo(ret).Type;
            //var x = 1;
        }
        return types.Select(t => t.Type)
            .Where(it=>it!=null)
            .Select(t=>t?.OriginalDefinition?.ToDisplayString()??"")
            .Where (it=>!string.IsNullOrWhiteSpace(it))
            .ToArray();
        //var s = types.First();
        //var q = s.Type;
        //return cds;
    }
    static bool IsController(SyntaxNode s)
    {
        if(!(s is ClassDeclarationSyntax cds))
            return false;
        var it = cds.Identifier ;
        if(!it.ValueText.Contains("Controller")) return false;
        
        return true;
                
    }
}
