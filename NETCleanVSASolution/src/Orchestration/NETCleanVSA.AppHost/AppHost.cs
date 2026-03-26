var builder = DistributedApplication.CreateBuilder(args);

var crmApi = builder.AddProject<Projects.CRM_Web_Api>("CRM-Api")
    .WithUrlForEndpoint("http", url =>
     {
         url.DisplayText = "CRM API";
         url.Url = "/scalar";
     });



builder.Build().Run();
