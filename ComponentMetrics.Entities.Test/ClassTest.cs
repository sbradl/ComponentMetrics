using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComponentMetrics.Entities.Test
{
    [TestClass]
    public class ClassTest
    {
        [TestMethod]
        [ExpectedException(typeof(Class.InvalidName))]
        public void ClassNameCannotBeEmpty()
        {
            var unused = new Class("   ");
        }
        
        [TestMethod]
        public void ClassName()
        {
            var cls = new Class("class 1");
            
            Assert.AreEqual("class 1", cls.Name);
        }
        
        [TestMethod]
        public void GivenNewClass_DependenciesShouldBeEmpty()
        {
            Assert.AreEqual(0, new Class("c").Dependencies.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Class.SelfReferencesNotAllowed))]
        public void CannotAddDependencyToSelf()
        {
            var cls = new Class("c");
            cls.AddDependencyOn(cls);
        }
        
        [TestMethod]
        public void AddDependencyToOtherClass()
        {
            var cls1 = new Class("c1");
            var cls2 = new Class("c2");
            
            cls1.AddDependencyOn(cls2);

            var dependency = cls1.Dependencies.Single();
            Assert.AreSame(cls2, dependency);
        }

        [TestMethod]
        public void ConcreteClass()
        {
            var cls = new Class("ConcreteClass");
            
            Assert.IsFalse(cls.IsAbstract);
        }

        [TestMethod]
        public void AbstractClass()
        {
            var cls = Class.Abstract("AbstractClass");
            
            Assert.IsTrue(cls.IsAbstract);
        }

        [TestMethod]
        public void GivenNoNamespace_FullnameIsClassName()
        {
            var cls = new Class("C");
            
            Assert.AreEqual(cls.Name, cls.FullName);
        }


        [TestMethod]
        public void GivenNamespace_FullnameIsNamespaceAndClassName()
        {
            var cls = new Class("C");
            cls.Namespace = "Ns";
            
            Assert.AreEqual("Ns.C", cls.FullName);
        }

    }
}