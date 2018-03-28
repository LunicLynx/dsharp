using System;
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

            var x;
            var y = x.Resolve("A");
        }

        [Fact]
        public void ResolveTypeInParentScope()
        {
            /*
             * class A {}
             * namespace C { class B : A {} }
             */
            // Base of B resolves to A

            var x;
            var y = x.Resolve("A");
        }

        [Fact]
        public void ResolveTypeWithQualifiedName()
        {
            /*
             * namespace A { class B {} }
             * namespace C { class D : A.B {} }
             */


            var root = new Scope();
            root.Add("A").Add("B");
            var scope = root.Add("C");
            var y = scope.Resolve("A.B");

            var actual = y.FullName;
            Assert.Equal("A.B", actual);
        }

        [Fact]
        public void ResolveTypeWithUsing()
        {
            /*
             * namespace A { class B {} }
             * namespace C { using A; class D : B {} }
             */
            var x;
            var y = x.Resolve("B");
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
    }
}



namespace A { class B { public class C { } } }
namespace D.E { using A; class F : B.C { } }


