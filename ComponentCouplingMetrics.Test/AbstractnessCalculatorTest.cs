using System;
using System.Linq;
using ComponentMetrics.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestData;

namespace ComponentCouplingMetrics.Test
{
    [TestClass]
    public class AbstractnessCalculatorTest
    {
        [TestMethod]
        [ExpectedException(typeof(AbstractnessCalculator.MissingComponent))]
        public void GivenNull_Throws()
        {
            new AbstractnessCalculator().CalculateFor(null);
        }

        [TestMethod]
        [ExpectedException(typeof(AbstractnessCalculator.EmptyComponent))]
        public void GivenEmptyComponent_Throws()
        {
            new AbstractnessCalculator().CalculateFor(new Component("c"));
        }

        [TestMethod]
        public void GivenNoAbstractClasses_MetricIsZero()
        {
            var component = new Component("c");
            component.Add(new Class("c1"));

            var abstractness = new AbstractnessCalculator().CalculateFor(component);
            
            Assert.AreEqual(0, abstractness);
        }

        [TestMethod]
        public void GivenAllClassesAreAbstract_MetricIsOne()
        {
            var component = new Component("c");
            component.Add(Class.Abstract("c1"));
            
            var abstractness = new AbstractnessCalculator().CalculateFor(component);
            
            Assert.AreEqual(1, abstractness);        
        }

        [TestMethod]
        public void Integration()
        {
            var component = new Component("c");
            component.Add(new Class("c1"));
            component.Add(new Class("c2"));
            component.Add(Class.Abstract("c3"));
            component.Add(new Class("c4"));
            
            var abstractness = new AbstractnessCalculator().CalculateFor(component);
            
            Assert.AreEqual(0.333, abstractness, 0.001);   
        }
    }

    public class AbstractnessCalculator
    {
        public double CalculateFor(Component component)
        {
            if (component == null)
                throw new MissingComponent();
            
            if (!component.Classes.Any())
                throw new EmptyComponent();

            var abstractClasses = (double)component.Classes.Count(c => c.IsAbstract);
            var concreteClasses = component.Classes.Count(c => !c.IsAbstract);

            if (concreteClasses == 0)
                return 1;
            
            return abstractClasses / concreteClasses;
        }
        
        public sealed class MissingComponent : Exception {}
        public sealed class EmptyComponent : Exception {}
    }
}