using System.Threading.Tasks;
using VoTanPhatVCBRDemo.Models.TokenAuth;
using VoTanPhatVCBRDemo.Web.Controllers;
using Shouldly;
using Xunit;

namespace VoTanPhatVCBRDemo.Web.Tests.Controllers
{
    public class HomeController_Tests: VoTanPhatVCBRDemoWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}