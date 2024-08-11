namespace ATFramework2._0;

public static class TestDataWorker
{
    private static string GenerateGuid(string format, int length)
    {
        string guid = Guid.NewGuid().ToString(format);
        if (length > 0 && length <= guid.Length)
        {
            guid = guid.Substring(0, length);
        }
        return guid;
    }

    #region General
    public static string GenerateString(string customPart, string guidFormat = "N", int guidLength = 32)
    {
        string guid = GenerateGuid(guidFormat, guidLength);
        return $"{customPart}_{guid}";
    }
    public static int GenerateInt(string customPart, string guidFormat = "N", int guidLength = 32)
    {
        string guid = GenerateGuid(guidFormat, guidLength);
        string combined = $"{customPart}_{guid}".GetHashCode().ToString();
        return Math.Abs(int.Parse(combined));
    }
    public static double GenerateDouble(string customPart, string guidFormat = "N", int guidLength = 32)
    {
        string guid = GenerateGuid(guidFormat, guidLength);
        string combined = $"{customPart}_{guid}".GetHashCode().ToString();
        return Math.Abs(double.Parse(combined.Substring(0, 5))) / 100.0;
    }
    #endregion

    #region DateTime
    public static DateTime GenerateCurrentDate() => DateTime.Now;
    public static DateTime GenerateRandomDate(DateTime startDate, DateTime endDate)
    {
        if (startDate >= endDate)
            throw new ArgumentException("Start date must be earlier than end date.");

        TimeSpan timeSpan = endDate - startDate;
        Random random = new Random();
        TimeSpan newSpan = new TimeSpan(0, random.Next(0, (int)timeSpan.TotalMinutes), 0);
        return startDate + newSpan;
    }
    public static string GenerateFormattedDate(string format = "yyyy-MM-dd") => DateTime.Now.ToString(format);
    #endregion
    
    #region PhoneNumber
    public enum CountryCode { USA, UK, Ukraine, Canada, Australia }
    private static readonly Dictionary<CountryCode, string> CountryCodes = new Dictionary<CountryCode, string>
    {
        { CountryCode.USA,       "+1"   },
        { CountryCode.UK,        "+44"  },
        { CountryCode.Ukraine,   "+380" },
        { CountryCode.Canada,    "+1"   },
        { CountryCode.Australia, "+61"  }
    };
    public static string GeneratePhoneNumber(CountryCode countryCode = CountryCode.USA, int length = 10)
    {
        Random random = new Random();
        string phoneNumber = CountryCodes[countryCode];
        for (int i = 0; i < length; i++)
        {
            phoneNumber += random.Next(0, 10).ToString();
        }
        return phoneNumber;
    }
    #endregion

    #region Email
    public enum EmailDomain { Example, Test, Dev, Sample, Demo }
    private static readonly Dictionary<EmailDomain, string> EmailDomains = new Dictionary<EmailDomain, string>
    {
        { EmailDomain.Example, "example.com" },
        { EmailDomain.Test,    "test.com"    },
        { EmailDomain.Dev,     "dev.com"     },
        { EmailDomain.Sample,  "sample.com"  },
        { EmailDomain.Demo,    "demo.com"    }
    };
    public static string GenerateEmail(string customPart, EmailDomain domain = EmailDomain.Example, bool includeGuidInName = true, bool includeGuidInDomain = false)
    {
        string nameGuidPart = includeGuidInName ? GenerateGuid("N", 8) : string.Empty;
        string emailName = includeGuidInName ? $"{customPart}_{nameGuidPart}" : customPart;
        
        string domainName = EmailDomains[domain];
        if (includeGuidInDomain)
        {
            string domainGuidPart = GenerateGuid("N", 8); 
            domainName = $"{domainGuidPart}.{domainName}";
        }

        return $"{emailName}@{domainName}";
    }
    #endregion
}
