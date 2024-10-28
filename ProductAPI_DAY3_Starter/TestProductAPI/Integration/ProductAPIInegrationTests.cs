using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProductAPI.Integration
{
    public class ProductAPIInegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
    }
}
