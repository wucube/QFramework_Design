using NUnit.Framework;

namespace QFramework.UnitTest
{
    public class SimpleIOCTest
    {
        [Test] //获取对象,并不是单例
        public void SimpleIOC_RegisterResolveTest()
        {
            var simpleIOC = new SimpleIOC();
            simpleIOC.Register<SimpleIOC>();
            var obj = simpleIOC.Resolve<SimpleIOC>();
            
            //创建了实例
            Assert.IsNotNull(obj);
            //两者不相同，说明创建了实例
            Assert.AreNotEqual(simpleIOC,obj);
            
        }

        [Test] //获取已注册过类型的对象
        public void SimpleIOC_ResolveRegisteredType()
        {
            var simpleIOC = new SimpleIOC();
            var obj = simpleIOC.Resolve<SimpleIOC>();
            //为空
            Assert.IsNull(obj);
        }

        [Test]//SimpleIOC重复注册
        public void SimpleIOC_RegisterTwice()
        {
            var simpleIOC = new SimpleIOC();
            
            simpleIOC.Register<SimpleIOC>();
            simpleIOC.Register<SimpleIOC>();
            
            Assert.IsTrue(true);
        }

        [Test] //注册为单例
        public void SimpleIOC_RegisterInstance()
        {
            var simpleIOC = new SimpleIOC();
            simpleIOC.RegisterInstance(new SimpleIOC());

            var instanceA = simpleIOC.Resolve<SimpleIOC>();
            var instanceB = simpleIOC.Resolve<SimpleIOC>();
            //两个对象相同
            Assert.AreEqual(instanceA,instanceB);
        }

        [Test] //注册依赖，通过接口获取实例
        public void SimpleIOC_RegisterDependency()
        {
            var simpleIOC = new SimpleIOC();
            
            simpleIOC.Register<ISimpleIoc,SimpleIOC>();
            
            var obj = simpleIOC.Resolve<ISimpleIoc>();
            //类型是否相同
            Assert.AreEqual(obj.GetType(),typeof(SimpleIOC));
        }

        [Test] //注册依赖单例
        public void SimpleIOC_RegisterInstanceDependency()
        {
            var simpleIOC = new SimpleIOC();
            
            simpleIOC.RegisterInstance<ISimpleIoc>(simpleIOC);

            var obj = simpleIOC.Resolve<ISimpleIoc>();
            var obj1 = simpleIOC.Resolve<ISimpleIoc>();
            
            Assert.AreEqual(simpleIOC,obj);
            Assert.AreEqual(simpleIOC,obj1);
        }

        class SomeDependencyA { }

        class SomeDependencyB { }

        class SomeCtrl
        {
            [SimpleIOCInject]//自定义特性
            public SomeDependencyA A { get; set; }
            
            [SimpleIOCInject]
            public SomeDependencyB B { get; set; }
        }

        [Test] //注册实例
        public void SimpleIOCInject()
        {
            var simpleIOC = new SimpleIOC();
            
            simpleIOC.RegisterInstance(new SomeDependencyA());
            simpleIOC.Register<SomeDependencyB>();

            var someCtrl = new SomeCtrl();
             
            simpleIOC.Inject(someCtrl);
            
            Assert.IsNotNull(someCtrl.A);
            Assert.IsNotNull(someCtrl.B);
            Assert.AreEqual(someCtrl.A.GetType(),typeof(SomeDependencyA));
            Assert.AreEqual(someCtrl.B.GetType(),typeof(SomeDependencyB));
        }
        
        [Test] //清空
        public void SimpleIOCClear()
        {
            var simpleIOC = new SimpleIOC();
            
            //注册依赖
            simpleIOC.RegisterInstance(new SomeDependencyA());
            simpleIOC.RegisterInstance<ISimpleIoc>(simpleIOC);
            simpleIOC.Register<SomeDependencyB>();
            
            //清空
            simpleIOC.Clear();

            //获取对象
            var someDependencyA = simpleIOC.Resolve<SomeDependencyA>();
            var someDependencyB = simpleIOC.Resolve<SomeDependencyB>();
            var ioc = simpleIOC.Resolve<ISimpleIoc>();
           
            //判断对象是否为空
            Assert.IsNull(someDependencyA);
            Assert.IsNull(someDependencyB);
            Assert.IsNull(ioc);

        }
    }
}