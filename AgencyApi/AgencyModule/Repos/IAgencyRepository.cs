﻿namespace AgencyApi.AgencyModule.Repos;

public interface IAgencyRepository
{
    Models.Agency? GetAgency(int id);
}