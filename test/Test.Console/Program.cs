using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mondol.FileService;
using Microsoft.Extensions.DependencyInjection;
using Mondol;
using Mondol.FileService.Authorization.Codecs;
using Mondol.FileService.Client;
using Mondol.FileService.Server;

namespace Test.Console
{
    class Program
    {
        const int ownerType = 1;
        const int ownerID = 1;
        const int timeoutSecond = 1111;

        static IServiceProvider Service;

        static async Task Main(string[] args)
        {
            try
            {
                var services = new ServiceCollection();
                services.AddOptions();
                services.AddFileService(cfg =>
                {
                    cfg.Host = "localhost:5000";
                    //cfg.AppSecret = "{F43D9A20-69DE-45D6-E298-7DACB57B9C10}";
                    cfg.AppSecret = "xxxxxxxxxxxxxxx";
                });

                Service = services.BuildServiceProvider();

                await OnMain();

                System.Console.WriteLine("press key continue...");
                System.Console.ReadLine();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
            System.Console.ReadKey();
        }

        private static async Task OnMain()
        {
            var hcFac = Service.GetRequiredService<IHttpClientFactory>();
            hcFac.CreateClient();

            var fsMgr = Service.GetRequiredService<IFileServiceManager>();

            var oToken = fsMgr.GenerateOwnerTokenString(ownerType, ownerID, TimeSpan.FromSeconds(1111));
            //var setMax = await fsMgr.SetOwnerQuotaAsync(ownerType, ownerID, 1021ul * 1024 * 1024 * 50);
            var upres =await fsMgr.UploadAsync(ownerType, ownerID, @"D:\Softs\FeiQ\FeiQ.exe");
            var fileInfo= await fsMgr.GetFileInfoAsync(upres.Data.FileToken);
            //System.Console.WriteLine(rs.ErrorCode);
        }
    }
}
