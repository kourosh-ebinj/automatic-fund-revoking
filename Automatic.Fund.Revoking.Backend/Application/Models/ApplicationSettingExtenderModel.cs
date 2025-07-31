using Core.Models;
using System;

namespace Application.Models
{
    public record ApplicationSettingExtenderModel : ApplicationSettingModel
    {
        public AppSettings App { get; set; }
        public BanksConfiguration Banks { get; init; }
        public RayanConfiguration Rayan { get; init; }

    }
    public record AppSettings
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public long FaceValue { get; set; }

    }
    
    public record MockServerConfiguration
    {
        public bool IsEnabled { get; set; }
        public string ServerUrl { get; set; }

    }

    public record BanksConfiguration
    {
        public MockServerConfiguration MockServer { get; set; }
        public PasargadConfiguration Pasargad { get; init; }
        public MarkaziConfiguration Markazi { get; init; }
    }

    public record MarkaziConfiguration
    {
        public double SatnaMin { get; set; }
        public double SatnaMax { get; set; }
        public double PayaMin { get; set; }
        public double PayaMax { get; set; }

    }

    public record PasargadConfiguration
    {
        public string BaseUrl { get; init; }
        public string Token { get; init; }
        public string TokenIssuer { get; init; }
        public int ProductIdAccountBalance { get; init; }
        public int ProductIdInternal { get; init; }
        public int ProductIdPaya { get; init; }
        public int ProductIdSatna { get; set; }
        public int ProductIdKYC { get; set; }
        public int OrgCodePaya { get; init; }
        public int OrgCodeSatna { get; init; }
        public string RandomString { get; set; }

    }

    public record RayanConfiguration
    {
        public string BaseUrl { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public RayanBackOfficeConfiguration BackOffice { get; init; }
    
    }

    public record RayanBackOfficeConfiguration
    {
        public string Username { get; init; }
        public string Password { get; init; }
    }

}
