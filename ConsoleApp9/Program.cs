using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp9
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            // 创建并配置依赖注入容器
            var _serviceProvider = new ServiceCollection()
                .AddScoped<IStrategyService, CarStrategyService>()  // 注册 Car Travel 策略
                .AddScoped<IStrategyService, PublicTransportationStrategyService>() // 注册 Public Transport 策略
                .AddScoped<IStrategyService, BicycleStrategyService>() // 注册 Bicycle 策略
                .BuildServiceProvider();

            //我改写的
            IStrategyService _strategyService;
            _strategyService = new CarStrategyService();
            var result = _strategyService.ReplaceDefaultReference("Hello [身份证] 祝您 [出行方式] 愉快");

            Console.WriteLine(result);

            Console.WriteLine();

            IStrategyService _strategyService_2;

            //客户选择出行方式为开车
            var TravleMode = CTravelFlag.Bicycle;
            using (var scope = _serviceProvider.CreateScope())
            {
                _strategyService_2 = scope.ServiceProvider.GetService<IEnumerable<IStrategyService>>().FirstOrDefault(s => s.TravelFlag == TravleMode);
                if (_strategyService_2 == null)
                {
                    Console.WriteLine("No matching travel strategy found.");
                    return;
                }
                var result_2 = _strategyService_2.ReplaceDefaultReference("Hello [身份证] 祝您 [出行方式] 愉快_2");
                Console.WriteLine(result_2);
            }
        }
    }

    public interface IStrategyService
    {
        CTravelFlag TravelFlag { get; }
        Task<string> ReplaceReference(string content);
        string ReplaceDefaultReference(string content);
    }

    [Flags]
    public enum CTravelFlag
    {
        None = 0,
        Car = 1,
        PublicTransportation = 1 << 1,      //2
        Bicycle = 1 << 2,                   //4
        Walking = 1 << 3,                   //8
        Taxi = 1 << 4,                      //16
        Train = 1 << 5,                     //32

        All = Car | PublicTransportation | Bicycle | Walking | Taxi | Train,
    }

    public abstract class StrategyService : IStrategyService
    {
        public virtual CTravelFlag TravelFlag => CTravelFlag.All;

        public virtual string ReplaceDefaultReference(string content)
        {
            //throw new NotImplementedException();
            Console.WriteLine("所有出行方式都得带身份证和钱包");
            content = content.Replace("[身份证]", "王先生");
            return content;
        }

        public virtual Task<string> ReplaceReference(string content)
        {
            throw new NotImplementedException();
        }
    }

    public class CarStrategyService : StrategyService
    {
        public override CTravelFlag TravelFlag => CTravelFlag.Car;
        public override string ReplaceDefaultReference(string content)
        {
            Console.WriteLine("客户选择了开车出行");
            content = base.ReplaceDefaultReference(content);
            return content.Replace("[出行方式]", "驾车出行");
        }
        public override async Task<string> ReplaceReference(string content)
        {
            throw new NotImplementedException();
        }
    }
    public class PublicTransportationStrategyService : StrategyService
    {
        public override CTravelFlag TravelFlag => CTravelFlag.PublicTransportation;
        public override string ReplaceDefaultReference(string content)
        {
            Console.WriteLine("客户选择了公共交通");
            content = base.ReplaceDefaultReference(content);
            return content.Replace("[出行方式]", "公共交通出行");
        }
        public override async Task<string> ReplaceReference(string content)
        {
            throw new NotImplementedException();
        }
    }
    public class BicycleStrategyService : StrategyService
    {
        public override CTravelFlag TravelFlag => CTravelFlag.Bicycle;
        public override string ReplaceDefaultReference(string content)
        {
            Console.WriteLine("客户选择了自行车");
            content = base.ReplaceDefaultReference(content);
            return content.Replace("[出行方式]", "自行车出行");
        }
        public override async Task<string> ReplaceReference(string content)
        {
            throw new NotImplementedException();
        }
    }

}
