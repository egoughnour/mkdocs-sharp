using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Example.XmlDocSamples.Tests;

/// <summary>
/// Tests that validate the example project has comprehensive XML documentation coverage
/// using Roslyn to analyze the source code.
/// </summary>
[TestClass]
public class XmlDocCoverageTests
{
    private static Compilation? _compilation;
    private static readonly string _projectPath = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Example.XmlDocSamples"));

    [ClassInitialize]
    public static void Initialize(TestContext context)
    {
        _compilation = CreateCompilation();
    }

    private static Compilation CreateCompilation()
    {
        var sourceFiles = Directory.GetFiles(_projectPath, "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains("obj") && !f.Contains("bin"));

        var syntaxTrees = sourceFiles.Select(file =>
            CSharpSyntaxTree.ParseText(
                File.ReadAllText(file),
                path: file,
                options: new CSharpParseOptions(LanguageVersion.CSharp12)));

        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .ToList();

        return CSharpCompilation.Create(
            "Example.XmlDocSamples.Analysis",
            syntaxTrees,
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }


    [TestMethod]
    public void AllPublicTypesHaveXmlDocumentation()
    {
        var undocumented = new List<string>();

        foreach (var tree in _compilation!.SyntaxTrees)
        {
            var root = tree.GetRoot();
            var model = _compilation.GetSemanticModel(tree);

            var typeDeclarations = root.DescendantNodes()
                .OfType<BaseTypeDeclarationSyntax>()
                .Where(t => IsPublicOrProtected(t.Modifiers));

            foreach (var typeDecl in typeDeclarations)
            {
                if (!HasXmlDocumentation(typeDecl))
                {
                    var symbol = model.GetDeclaredSymbol(typeDecl);
                    undocumented.Add($"Type: {symbol?.ToDisplayString() ?? typeDecl.Identifier.Text}");
                }
            }
        }

        Assert.AreEqual(0, undocumented.Count,
            $"Found {undocumented.Count} undocumented public types:\n{string.Join("\n", undocumented.Take(20))}");
    }

    [TestMethod]
    public void AllPublicMethodsHaveXmlDocumentation()
    {
        var undocumented = new List<string>();

        foreach (var tree in _compilation!.SyntaxTrees)
        {
            var root = tree.GetRoot();
            var model = _compilation.GetSemanticModel(tree);

            var methods = root.DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(m => IsPublicOrProtected(m.Modifiers));

            foreach (var method in methods)
            {
                if (!HasXmlDocumentation(method))
                {
                    var symbol = model.GetDeclaredSymbol(method);
                    undocumented.Add($"Method: {symbol?.ToDisplayString() ?? method.Identifier.Text}");
                }
            }
        }

        Assert.AreEqual(0, undocumented.Count,
            $"Found {undocumented.Count} undocumented public methods:\n{string.Join("\n", undocumented.Take(20))}");
    }

    [TestMethod]
    public void AllPublicPropertiesHaveXmlDocumentation()
    {
        var undocumented = new List<string>();

        foreach (var tree in _compilation!.SyntaxTrees)
        {
            var root = tree.GetRoot();
            var model = _compilation.GetSemanticModel(tree);

            var properties = root.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Where(p => IsPublicOrProtected(p.Modifiers));

            foreach (var prop in properties)
            {
                if (!HasXmlDocumentation(prop))
                {
                    var symbol = model.GetDeclaredSymbol(prop);
                    undocumented.Add($"Property: {symbol?.ToDisplayString() ?? prop.Identifier.Text}");
                }
            }
        }

        Assert.AreEqual(0, undocumented.Count,
            $"Found {undocumented.Count} undocumented public properties:\n{string.Join("\n", undocumented.Take(20))}");
    }

    [TestMethod]
    public void AllPublicConstructorsHaveXmlDocumentation()
    {
        var undocumented = new List<string>();

        foreach (var tree in _compilation!.SyntaxTrees)
        {
            var root = tree.GetRoot();
            var model = _compilation.GetSemanticModel(tree);

            var constructors = root.DescendantNodes()
                .OfType<ConstructorDeclarationSyntax>()
                .Where(c => IsPublicOrProtected(c.Modifiers));

            foreach (var ctor in constructors)
            {
                if (!HasXmlDocumentation(ctor))
                {
                    var symbol = model.GetDeclaredSymbol(ctor);
                    undocumented.Add($"Constructor: {symbol?.ToDisplayString() ?? ctor.Identifier.Text}");
                }
            }
        }

        Assert.AreEqual(0, undocumented.Count,
            $"Found {undocumented.Count} undocumented public constructors:\n{string.Join("\n", undocumented.Take(20))}");
    }

    [TestMethod]
    public void AllPublicFieldsHaveXmlDocumentation()
    {
        var undocumented = new List<string>();

        foreach (var tree in _compilation!.SyntaxTrees)
        {
            var root = tree.GetRoot();
            var model = _compilation.GetSemanticModel(tree);

            var fields = root.DescendantNodes()
                .OfType<FieldDeclarationSyntax>()
                .Where(f => IsPublicOrProtected(f.Modifiers));

            foreach (var field in fields)
            {
                if (!HasXmlDocumentation(field))
                {
                    var variables = string.Join(", ", field.Declaration.Variables.Select(v => v.Identifier.Text));
                    undocumented.Add($"Field: {variables}");
                }
            }
        }

        Assert.AreEqual(0, undocumented.Count,
            $"Found {undocumented.Count} undocumented public fields:\n{string.Join("\n", undocumented.Take(20))}");
    }

    [TestMethod]
    public void AllEnumMembersHaveXmlDocumentation()
    {
        var undocumented = new List<string>();

        foreach (var tree in _compilation!.SyntaxTrees)
        {
            var root = tree.GetRoot();
            var model = _compilation.GetSemanticModel(tree);

            var enumDeclarations = root.DescendantNodes()
                .OfType<EnumDeclarationSyntax>()
                .Where(e => IsPublicOrProtected(e.Modifiers));

            foreach (var enumDecl in enumDeclarations)
            {
                foreach (var member in enumDecl.Members)
                {
                    if (!HasXmlDocumentation(member))
                    {
                        undocumented.Add($"Enum member: {enumDecl.Identifier.Text}.{member.Identifier.Text}");
                    }
                }
            }
        }

        Assert.AreEqual(0, undocumented.Count,
            $"Found {undocumented.Count} undocumented enum members:\n{string.Join("\n", undocumented.Take(20))}");
    }

    [TestMethod]
    public void AllDelegatesHaveXmlDocumentation()
    {
        var undocumented = new List<string>();

        foreach (var tree in _compilation!.SyntaxTrees)
        {
            var root = tree.GetRoot();
            var model = _compilation.GetSemanticModel(tree);

            var delegates = root.DescendantNodes()
                .OfType<DelegateDeclarationSyntax>()
                .Where(d => IsPublicOrProtected(d.Modifiers));

            foreach (var del in delegates)
            {
                if (!HasXmlDocumentation(del))
                {
                    var symbol = model.GetDeclaredSymbol(del);
                    undocumented.Add($"Delegate: {symbol?.ToDisplayString() ?? del.Identifier.Text}");
                }
            }
        }

        Assert.AreEqual(0, undocumented.Count,
            $"Found {undocumented.Count} undocumented delegates:\n{string.Join("\n", undocumented.Take(20))}");
    }

    [TestMethod]
    public void AllEventsHaveXmlDocumentation()
    {
        var undocumented = new List<string>();

        foreach (var tree in _compilation!.SyntaxTrees)
        {
            var root = tree.GetRoot();
            var model = _compilation.GetSemanticModel(tree);

            var events = root.DescendantNodes()
                .OfType<EventDeclarationSyntax>()
                .Where(e => IsPublicOrProtected(e.Modifiers));

            foreach (var evt in events)
            {
                if (!HasXmlDocumentation(evt))
                {
                    var symbol = model.GetDeclaredSymbol(evt);
                    undocumented.Add($"Event: {symbol?.ToDisplayString() ?? evt.Identifier.Text}");
                }
            }

            // Also check event fields
            var eventFields = root.DescendantNodes()
                .OfType<EventFieldDeclarationSyntax>()
                .Where(e => IsPublicOrProtected(e.Modifiers));

            foreach (var evtField in eventFields)
            {
                if (!HasXmlDocumentation(evtField))
                {
                    var variables = string.Join(", ", evtField.Declaration.Variables.Select(v => v.Identifier.Text));
                    undocumented.Add($"Event field: {variables}");
                }
            }
        }

        Assert.AreEqual(0, undocumented.Count,
            $"Found {undocumented.Count} undocumented events:\n{string.Join("\n", undocumented.Take(20))}");
    }

    [TestMethod]
    public void AllIndexersHaveXmlDocumentation()
    {
        var undocumented = new List<string>();

        foreach (var tree in _compilation!.SyntaxTrees)
        {
            var root = tree.GetRoot();
            var model = _compilation.GetSemanticModel(tree);

            var indexers = root.DescendantNodes()
                .OfType<IndexerDeclarationSyntax>()
                .Where(i => IsPublicOrProtected(i.Modifiers));

            foreach (var indexer in indexers)
            {
                if (!HasXmlDocumentation(indexer))
                {
                    var symbol = model.GetDeclaredSymbol(indexer);
                    var containingType = indexer.Ancestors().OfType<TypeDeclarationSyntax>().FirstOrDefault();
                    undocumented.Add($"Indexer: {containingType?.Identifier.Text}.this[...]");
                }
            }
        }

        Assert.AreEqual(0, undocumented.Count,
            $"Found {undocumented.Count} undocumented indexers:\n{string.Join("\n", undocumented.Take(20))}");
    }

    [TestMethod]
    public void AllOperatorsHaveXmlDocumentation()
    {
        var undocumented = new List<string>();

        foreach (var tree in _compilation!.SyntaxTrees)
        {
            var root = tree.GetRoot();
            var model = _compilation.GetSemanticModel(tree);

            var operators = root.DescendantNodes()
                .OfType<OperatorDeclarationSyntax>()
                .Where(o => IsPublicOrProtected(o.Modifiers));

            foreach (var op in operators)
            {
                if (!HasXmlDocumentation(op))
                {
                    var symbol = model.GetDeclaredSymbol(op);
                    undocumented.Add($"Operator: {symbol?.ToDisplayString() ?? op.OperatorToken.Text}");
                }
            }

            var conversionOperators = root.DescendantNodes()
                .OfType<ConversionOperatorDeclarationSyntax>()
                .Where(c => IsPublicOrProtected(c.Modifiers));

            foreach (var conv in conversionOperators)
            {
                if (!HasXmlDocumentation(conv))
                {
                    var symbol = model.GetDeclaredSymbol(conv);
                    undocumented.Add($"Conversion operator: {symbol?.ToDisplayString() ?? "implicit/explicit"}");
                }
            }
        }

        Assert.AreEqual(0, undocumented.Count,
            $"Found {undocumented.Count} undocumented operators:\n{string.Join("\n", undocumented.Take(20))}");
    }

    [TestMethod]
    public void ProjectContainsExpectedSyntaxElements()
    {
        var syntaxElements = new Dictionary<string, int>();

        foreach (var tree in _compilation!.SyntaxTrees)
        {
            var root = tree.GetRoot();

            CountSyntax<ClassDeclarationSyntax>(root, "Classes", syntaxElements);
            CountSyntax<StructDeclarationSyntax>(root, "Structs", syntaxElements);
            CountSyntax<RecordDeclarationSyntax>(root, "Records", syntaxElements);
            CountSyntax<InterfaceDeclarationSyntax>(root, "Interfaces", syntaxElements);
            CountSyntax<EnumDeclarationSyntax>(root, "Enums", syntaxElements);
            CountSyntax<DelegateDeclarationSyntax>(root, "Delegates", syntaxElements);
            CountSyntax<MethodDeclarationSyntax>(root, "Methods", syntaxElements);
            CountSyntax<PropertyDeclarationSyntax>(root, "Properties", syntaxElements);
            CountSyntax<IndexerDeclarationSyntax>(root, "Indexers", syntaxElements);
            CountSyntax<OperatorDeclarationSyntax>(root, "Operators", syntaxElements);
            CountSyntax<ConversionOperatorDeclarationSyntax>(root, "ConversionOperators", syntaxElements);
            CountSyntax<EventFieldDeclarationSyntax>(root, "Events", syntaxElements);
            CountSyntax<ConstructorDeclarationSyntax>(root, "Constructors", syntaxElements);
        }

        // Verify we have a comprehensive set of examples
        var report = new StringBuilder();
        report.AppendLine("Syntax Element Coverage:");
        foreach (var kvp in syntaxElements.OrderBy(k => k.Key))
        {
            report.AppendLine($"  {kvp.Key}: {kvp.Value}");
        }

        Console.WriteLine(report.ToString());

        // Assert minimum coverage
        Assert.IsTrue(syntaxElements.GetValueOrDefault("Classes") >= 5, "Should have at least 5 class examples");
        Assert.IsTrue(syntaxElements.GetValueOrDefault("Structs") >= 2, "Should have at least 2 struct examples");
        Assert.IsTrue(syntaxElements.GetValueOrDefault("Records") >= 2, "Should have at least 2 record examples");
        Assert.IsTrue(syntaxElements.GetValueOrDefault("Interfaces") >= 2, "Should have at least 2 interface examples");
        Assert.IsTrue(syntaxElements.GetValueOrDefault("Enums") >= 2, "Should have at least 2 enum examples");
        Assert.IsTrue(syntaxElements.GetValueOrDefault("Delegates") >= 1, "Should have at least 1 delegate example");
        Assert.IsTrue(syntaxElements.GetValueOrDefault("Methods") >= 10, "Should have at least 10 method examples");
        Assert.IsTrue(syntaxElements.GetValueOrDefault("Properties") >= 10, "Should have at least 10 property examples");
    }

    private static void CountSyntax<T>(SyntaxNode root, string name, Dictionary<string, int> counts)
        where T : SyntaxNode
    {
        var count = root.DescendantNodes().OfType<T>().Count();
        counts[name] = counts.GetValueOrDefault(name) + count;
    }

    private static bool IsPublicOrProtected(SyntaxTokenList modifiers)
    {
        return modifiers.Any(m =>
            m.IsKind(SyntaxKind.PublicKeyword) ||
            m.IsKind(SyntaxKind.ProtectedKeyword));
    }

    private static bool HasXmlDocumentation(SyntaxNode node)
    {
        var trivia = node.GetLeadingTrivia();
        return trivia.Any(t =>
            t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
            t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia));
    }
}
