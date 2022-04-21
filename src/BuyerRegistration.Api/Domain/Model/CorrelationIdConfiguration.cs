﻿namespace BuyerRegistration.Api.Domain.Model
{
    public class CorrelationIdConfiguration
    {
        public string ServiceName { get; }
        public string DomainAcronym { get; }

        public CorrelationIdConfiguration(string serviceName, string domainAcronym)
        {
            ServiceName = serviceName;
            DomainAcronym = domainAcronym;
        }


    }
}
