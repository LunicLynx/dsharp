using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Xunit;

namespace DSharp.Tests
{
    // TDD writing tests for what you know
    public class UnitTest1
    {
        [Fact]
        public void ResolveTypeInSameScope()
        {
            /*
            class A {}
            class B : A {}
            */
            // Base of B resolves to A

            // arrange
            var root = new Scope();
            root.Add(new ClassModel("", "A"));

            // act
            var decl = root.Resolve("A");

            // assert
            var actual = decl.FullName;
            Assert.Equal("A", actual);
        }

        [Fact]
        public void ResolveTypeInParentScope()
        {
            /*
             * class A {}
             * namespace C { class B : A {} }
             */
            // Base of B resolves to A

            // arrange
            var root = new Scope();
            var scopeC = root.Add("C");

            // act
            var decl = scopeC.Resolve("A");

            // assert
            var actual = decl.FullName;
            Assert.Equal("A", actual);
        }

        [Fact]
        public void ResolveTypeWithQualifiedName()
        {
            /*
             * namespace A { class B {} }
             * namespace C { class D : A.B {} }
             */

            // arrange
            var root = new Scope();
            root.Add("A").Add("B");
            var scopeC = root.Add("C");

            // act
            var decl = scopeC.Resolve("A.B");

            // assert
            var actual = decl.FullName;
            Assert.Equal("A.B", actual);
        }

        [Fact]
        public void ResolveTypeWithUsing()
        {
            /*
             * namespace A { class B {} }
             * namespace C { using A; class D : B {} }
             */

            // arrange
            var root = new Scope();
            var scopeA = root.Add("A");
            var scopeC = root.Add("C");

            // act
            var decl = scopeC.Resolve("B");

            // assert
            var actual = decl.FullName;
            Assert.Equal("A.B", actual);
        }

        [Fact]
        public void DontResolveNamespaceWithPartialQualifiedNameAndUsing()
        {
            /*
             * namespace A.B { class C { } }
             * namespace D.E { using A; class F : B.C { } }
             */
            // B cant be resolved qualified names are not combined with usings

            var x;
            var y = x.Resolve("B.C");
        }

        [Fact]
        public void ResolveNestedClassWithQualifiedNameAndUsing()
        {
            /*
             * namespace A { class B { public class C { } } }
             * namespace D.E { using A; class F : B.C { } }
             */
            // B cant be resolved qualified names are not combined with usings

            var x;
            var y = x.Resolve("B.C");
        }

        [Fact]
        public void ResolveWithPartialQualifiedNameInParten()
        {
            /*
             * namespace A.B { class C { } }
             * namespace A.D { class E : B.C { } }
             */
            var x;
            var y = x.Resolve("B.C");
        }

        [Fact]
        public void ResolveWithLocalUsingBeforeParent()
        {
            // use using before fall through to parent
            /*
             * namespace A { class B { } }
             * namespace C { class B { } namespace D { using A; class E : B { } } }
             */

            // Resolve B from inside D finds A.B before C.B
        }
    }

    public class Scope
    {

        public Scope()
        {
            
        }

        public Scope(Scope parent)
        {
            
        }

        public Scope Add(string name)
        {
            return null;
        }


        public IDeclaration Resolve(string name)
        {
            return null;
        }
    }

    public interface IDeclaration
    {
        string FullName { get; }
    }

    public class ClassModel : IDeclaration
    {
        public string Namespace { get; }
        public string Name { get; }

        public string FullName => $"{Namespace}.{Name}";

        public ClassModel(string @namespace, string name)
        {
            Namespace = @namespace;
            Name = name;
        }
    }
}
